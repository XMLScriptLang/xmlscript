using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlscript
{
    public static class Extensions
    {
        public static string Join<T>(this IEnumerable<T> i, string sep)
        {
            string output = "";

            foreach(T item in i)
            {
                output += item.ToString() + sep;
            }

            if(output.Length >= sep.Length) output = output.Substring(0, output.Length-sep.Length);
            return output;
        }
    }

    public static class Utils
    {
        public static string SemicolonOptional(Dictionary<string, object> args)
        {
            return (args != null && args["NoSemicolon"] != null && ((bool)args["NoSemicolon"])) ? "" : ";";
        }
    }
}
