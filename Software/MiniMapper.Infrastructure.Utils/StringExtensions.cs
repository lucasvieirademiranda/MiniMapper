using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMapper.Infrastructure.Utils
{
    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace(str[0], char.ToUpper(str[0]));
        }
    }
}
