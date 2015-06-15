using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Application.ViewModel
{
    public class ConnectionViewModel
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public ICollection<DatabaseViewModel> Databases { get; set; }

        public ConnectionViewModel(ICollection<DatabaseViewModel> databases)
        {
            Databases = (databases != null) ? databases : new List<DatabaseViewModel>();
        }
    }
}
