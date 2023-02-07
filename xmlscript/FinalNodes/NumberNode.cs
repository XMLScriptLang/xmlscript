using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class NumberNode : Node
    {
        public object val;

        public override Node FromXmlTag(XmlNode node)
        {
            if (node.InnerText.EndsWith(" : i32"))
            {
                if (!int.TryParse(node.InnerText.Substring(0, node.InnerText.Length - 6), out int tVal)) throw new Exception("NumberNode contains invalid number");
                val = tVal;
            }
            else {
                if (!double.TryParse(node.InnerText, out double tVal)) throw new Exception("NumberNode contains invalid number");
                val = tVal;
            }
        
            return this;
        }

        public override object Visit(Scope scope)
        {
            return val;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            return val.ToString();
        }
    }
}
