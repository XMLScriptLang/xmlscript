using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class StringNode : Node
    {
        public string val;

        public override Node FromXmlTag(XmlNode node)
        {
            this.val = node.InnerText;
            return this;
        }

        public override object Visit(Scope scope)
        {
            return val;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            return $"\"{val}\"";
        }
    }
}
