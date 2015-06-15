using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Application.ViewModel
{
    public class TableViewModel
    {
        public string TableName { get; set; }
        public string MappingName { get; set; }
        public string Type { get; set; }
        public ICollection<ColumnViewModel> Columns { get; set; }

        public TableViewModel(ICollection<ColumnViewModel> columns)
        {
            Columns = (columns != null) ? columns : new List<ColumnViewModel>();
        }
    }
}
