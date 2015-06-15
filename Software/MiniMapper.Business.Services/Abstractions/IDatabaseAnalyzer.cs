using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniMapper.Business.Entities.Core;

namespace MiniMapper.Business.Services.Abstractions
{
    public interface IDatabaseAnalyzer
    {
        Database Analyze();
        bool Test();
    }
}
