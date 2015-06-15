using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Business.Entities.Enumerators
{
    public enum EMatchOption
    {
        [Description("Cascade")]
        Cascade = 1,
        [Description("Set Null")]
        SetNull = 2,
        [Description("Set Default")]
        SetDefault = 3,
        [Description("Restrict")]
        Restrict = 4,
        [Description("No Action")]
        NoAction = 5
    }
}
