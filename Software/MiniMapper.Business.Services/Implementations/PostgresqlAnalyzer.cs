using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiniMapper.Business.Entities.Core;
using MiniMapper.Business.Services.Abstractions;
using MiniMapper.Infrastructure.Utils;
using Npgsql;

namespace MiniMapper.Business.Services.Implementations
{
    public class PostgresqlAnalyzer : IDatabaseAnalyzer
    {
        public Connection connection { get; set; }

        public PostgresqlAnalyzer() { }

        public PostgresqlAnalyzer(Connection connection)
        {
            this.connection = connection;
        }

        public Database Analyze()
        {
            Database database = null;

            DbConnection dbConnection = null;

            DbDataReader schema = null;
            DbDataReader tables = null;
            DbDataReader columns = null;

            try
            {
                dbConnection = new NpgsqlConnection(GetConnectionString(connection));

                schema = GetSchema(dbConnection, connection.Database, "public");

                while (schema.NextResult())
                {
                    database = new Database();
                    database.Name = schema["schema_name"].ToString();
                    connection.Databases.Add(database);

                    tables = GetTables(dbConnection, connection.Database, "public");

                    while (tables.NextResult())
                    {
                        Table table = new Table();
                        table.TableName = tables["table_name"].ToString();
                        table.MappingName = RemoveSpecialCharacters(tables["table_name"].ToString());
                        table.TableType = "";
                        database.Tables.Add(table);

                        columns = GetColumns(dbConnection,
                                             connection.Database,
                                             "public",
                                             tables["table_name"].ToString());

                        while (columns.NextResult())
                        {
                            Column column = new Column();

                            column.TableName = columns["table_name"].ToString();
                            column.MappingName = RemoveSpecialCharacters(columns["table_name"].ToString());
                            column.ReferenceTableName = columns["reference_table"].ToString();
                            column.ReferenceMappingName = RemoveSpecialCharacters(columns["reference_table"].ToString());
                            column.MappingType = GetMappingType(columns);
                            //var defaultValue = columns["default_value"].ToString();
                            //column.DefaultValue = (!defaultValue.StartsWith("nextval")) ? defaultValue : null;
                            //column.Sequence = (defaultValue.StartsWith("nextval")) ? defaultValue.Substring(defaultValue.IndexOf("'"), defaultValue.LastIndexOf("'")) : null;
                            column.IsIdentity = (columns["is_identity"].ToString() == "YES") ? true : false;
                            column.IsNullable = (columns["is_nullable"].ToString() == "YES") ? true : false;
                            column.Precision = Convert.ToInt64(columns["precision"]);
                            column.Scale = Convert.ToInt64(columns["scale"]);
                            column.Length = Convert.ToInt64(columns["length"]);
                            column.MatchOption = columns["match_option"].ToString();
                            column.UpdateRule = columns["update_rule"].ToString();
                            column.DeleteRule = columns["delete_rule"].ToString();

                        }

                        columns.Close();

                    }

                    tables.Close();
                }

                schema.Close();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if(columns != null)
                    columns.Close();

                if(tables != null)
                    tables.Close();

                if(schema != null)
                    schema.Close();

                if(dbConnection != null)
                    dbConnection.Close();
            }

            return database;
        }

        public bool Test()
        {
            DbConnection dbConnection = null;

            try
            {
                dbConnection = new NpgsqlConnection(GetConnectionString(connection));
                dbConnection.Open();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(dbConnection != null)
                    dbConnection.Close();
            }

            return true;
        }

        private string GetConnectionString(Connection connection)
        {
            string connectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3}",
                connection.Server,
                connection.Database,
                connection.User,
                connection.Password
            );

            return connectionString;
        }

        private DbDataReader GetSchema(DbConnection dbConnection,
                                       string catalog,
                                       string schema)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "SELECT * FROM information_schema.schemata WHERE table_catalog = @catalog AND schema_name = @schema";
            dbCommand.CommandType = CommandType.Text;

            DbParameter dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "catalog";
            dbParameter.Value = catalog;

            dbCommand.Parameters.Add(dbParameter);

            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "schema";
            dbParameter.Value = schema;

            dbCommand.Parameters.Add(dbParameter);

            return dbCommand.ExecuteReader();
        }

        private DbDataReader GetTables(DbConnection dbConnection,
                                       string catalog,
                                       string schema)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "SELECT * FROM information_schema.tables WHERE table_catalog = @catalog AND schema_name = @schema";
            dbCommand.CommandType = CommandType.Text;

            DbParameter dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "catalog";
            dbParameter.Value = catalog;

            dbCommand.Parameters.Add(dbParameter);


            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "schema";
            dbParameter.Value = schema;

            dbCommand.Parameters.Add(dbParameter);

            return dbCommand.ExecuteReader();
        }

        private DbDataReader GetColumns(DbConnection dbConnection,
                                        string catalog,
                                        string schema,
                                        string table)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = @"SELECT C.column_name AS table_name,
                                             NTC.reference_table AS reference_table,
                                             C.udt_name AS type,
                                             C.column_default AS default_value,
                                             C.is_nullable AS is_nullable,
                                             C.is_identity AS is_identity,
                                             C.character_maximum_length AS length,
                                             C.numeric_precision_radix AS precision,
                                             C.numeric_scale AS scale,
                                             NTC.match_option AS match_option,
                                             NTC.update_rule AS update_rule,
                                             NTC.delete_rule AS delete_rule
                                     FROM information_schema.columns AS C
                                     LEFT JOIN (
	                                     SELECT KCU.column_name AS column_name,
	                                            RC.match_option AS match_option,
	                                            RC.update_rule AS update_rule,
	                                            RC.delete_rule AS delete_rule,
	                                            CCU.table_name AS reference_table
	                                     FROM information_schema.table_constraints AS TC
	                                     RIGHT JOIN information_schema.key_column_usage AS KCU ON TC.constraint_name = KCU.constraint_name
	                                     RIGHT JOIN information_schema.constraint_column_usage AS CCU ON KCU.constraint_name = CCU.constraint_name
	                                     RIGHT JOIN information_schema.referential_constraints AS RC ON TC.constraint_name = RC.constraint_name
	                                     WHERE TC.constraint_type = 'FOREIGN KEY'
                                     ) AS NTC ON C.column_name = NTC.column_name
                                     WHERE table_catalog = @catalog
                                     AND table_schema = @schema
                                     AND table_name   = @table";
            dbCommand.CommandType = CommandType.Text;

            DbParameter dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "catalog";
            dbParameter.Value = catalog;

            dbCommand.Parameters.Add(dbParameter);

            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "schema";
            dbParameter.Value = schema;

            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "table";
            dbParameter.Value = table;

            return dbCommand.ExecuteReader();
        }

        private string RemoveSpecialCharacters(string word)
        {
            string pattern = @"[\!\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~\""]";

            string[] pieces = Regex.Split(word, pattern);

            string sanitizedWord = "";

            foreach (string piece in pieces)
                sanitizedWord += piece.Capitalize();

            return sanitizedWord;
        }

        private string GetMappingType(DbDataReader columns)
        {
            string type = columns["type"].ToString();
            bool isNullable = (columns["is_nullable"].ToString() == "YES") ? true : false;
            long length = Convert.ToInt64(columns["length"].ToString());
            
            string mappingType = string.Empty;

            if ((type == "smallint" || type == "smallserial"))
                mappingType = "short";
            else if ((type == "integer" || type == "serial"))
                mappingType = "int";
            else if ((type == "bigint" || type == "bigserial"))
                mappingType = "long";
            else if (type == "real")
                mappingType = "single";
            else if (type == "double precision")
                mappingType = "double";
            else if (type == "decimal" || type == "numeric" || type == "money")
                mappingType = "decimal";
            else if ((type == "character" || type == "char") && length > 1)
                mappingType = "string";
            else if (type == "char")
                mappingType = "char";
            else if ((type == "character varying" || type == "varchar"))
                mappingType = "string";
            else if (type == "text")
                mappingType = "string";
            else if (type == "timestamp" ||
                     type == "timestamp without time zone" ||
                     type == "date" ||
                     type == "time" ||
                     type == "time without time zone")
                mappingType = "DateTime";
            else if (type == "timestamp with time zone" ||
                     type == "time with time zone")
                mappingType = "DateTimeOffset";

            if (mappingType != "string" && isNullable)
                mappingType = mappingType + "?";

            return mappingType;
        }

    }
}
