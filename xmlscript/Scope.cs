using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlscript
{
    public class Scope
    {
        public Dictionary<string, object> Table = new();
        public Scope ParentScope { get; set; }

        public static Scope FromParent(Scope parent)
        {
            Scope newScope = new();
            newScope.ParentScope = parent;
            return newScope;
        }

        public object Get(string key)
        {
            if(!Table.ContainsKey(key))
            {
                if (ParentScope != null) return ParentScope.Get(key);
                return null;
            }

            return Table[key];
        }

        public void Set(string key, object value)
        {
            Table[key] = value;
        }
    }
}
