using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Business.Entities.Core
{
    public class Connection
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Provider { get; set; }
        public virtual string Server { get; set; }
        public virtual string Database { get; set; }
        public virtual string User { get; set; }
        public virtual string Password { get; set; }
        public virtual ICollection<Database> Databases { get; set; }

        public Connection()
        {
            this.Databases = new List<Database>();
        }
    }
}
