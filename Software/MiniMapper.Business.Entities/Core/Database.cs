using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Business.Entities.Core
{
    public class Database
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long ProjectId { get; set; }
        public virtual Connection Project { get; set; }
        public virtual ICollection<Table> Tables { get; set; }

        public Database()
        {
            this.Tables = new List<Table>();
        }
    }
}
