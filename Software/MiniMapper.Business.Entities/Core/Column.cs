using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniMapper.Business.Entities.Enumerators;

namespace MiniMapper.Business.Entities.Core
{
    public class Column
    {
        public long Id { get; set; }
        public string TableName { get; set; }
        public string MappingName { get; set; }
        public string ReferenceTableName { get; set; }
        public string ReferenceMappingName { get; set; }
        public string MappingType { get; set; }
        public string DefaultValue { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public string Sequence { get; set; }
        public long Length { get; set; }
        public long Precision { get; set; }
        public long Scale { get; set; }
        public string MatchOption { get; set; }
        public string UpdateRule { get; set; }
        public string DeleteRule { get; set; }
        public bool HasReference { get; set; }
        public bool HasInverse { get; set; }
        public bool IsReadonly { get; set; }
    }
}
