using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Application.ViewModel
{
    public class DatabaseViewModel
    {
        public string Name { get; set; }
        public ICollection<TableViewModel> Tables { get; set; }

        public DatabaseViewModel(ICollection<TableViewModel> tables)
        {
            Tables = (tables != null) ? tables : new List<TableViewModel>();
        }
    }
}
