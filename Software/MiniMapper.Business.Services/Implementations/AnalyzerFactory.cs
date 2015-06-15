using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniMapper.Business.Services.Abstractions;

namespace MiniMapper.Business.Services.Implementations
{
    public static class AnalyzerFactory
    {
        public static IDatabaseAnalyzer Create(string database)
        {
            if (database == "Firebird")
                return new FirebirdAnalyzer();
            else if (database == "MySql")
                return new MySqlAnalyzer();
            else if (database == "Oracle")
                return new OracleAnalyzer();
            else if (database == "Postgresql")
                return new PostgresqlAnalyzer();
            else if (database == "SqlLite")
                return new SqlLiteAnalyzer();
            else if (database == "SqlServer")
                return new SqlServerAnalyzer();
            else
                return null;
        }
    }
}
