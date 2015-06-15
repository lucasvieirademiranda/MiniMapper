using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Business.Entities.Core
{
    public class Table
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
        public virtual string MappingName { get; set; }
        public virtual string TableType { get; set; }
        public virtual long DatabaseId { get; set; }
        public virtual Database Database { get; set; }
        public virtual ICollection<Column> Columns { get; set; }

        public Table()
        {
            Columns = new List<Column>();
        }
    }
}
