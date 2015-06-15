using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Business.Entities.Enumerators
{
    public enum ECascadeRule
    {
        [Description("Full")]
        Full = 1,
        [Description("Partial")]
        Partial = 2,
        [Description("None")]
        None = 3
    }
}
