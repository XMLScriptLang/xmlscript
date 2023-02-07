using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace xmlscript
{
    public static class Extensions
    {
        private static Dictionary<string, Type> resolvedTypesCache = new();
        
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

        public static Type ResolveType(this string name)
        {
            if (resolvedTypesCache.ContainsKey(name)) return resolvedTypesCache[name];
            
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
                    if (t.FullName == name || t.Name == name)
                    {
                        resolvedTypesCache.Add(name, t);
                        return t;
                    }
                }
            }

            return null;
        }
    }

    public static class Utils
    {
        public static string SemicolonOptional(Dictionary<string, object> args)
        {
            return (args != null && args["NoSemicolon"] != null && ((bool)args["NoSemicolon"])) ? "" : ";";
        }

        public static Dictionary<string, object> Passdown(Dictionary<string, object> args)
        {
            return (args != null && args["Passdown"] != null && ((bool)args["Passdown"])) ? GetWithoutPassdown(args) : null;
        }

        public static Dictionary<string, object> GetWithoutPassdown(Dictionary<string,object> args)
        {
            args.Remove("Passdown");

            return args;
        }
    }
}
