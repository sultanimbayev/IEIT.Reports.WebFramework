using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    public static class Utils
    {
        public static string RemoveAtEnd(this string source, string ending)
        {
            if (source.EndsWith(ending))
            {
                return source.Remove(source.Length - ending.Length);
            }
            return source;
        }
    }
}
