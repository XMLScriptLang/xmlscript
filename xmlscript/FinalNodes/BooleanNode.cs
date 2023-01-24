using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class BooleanNode : Node
    {
        public bool val;

        public override Node FromXmlTag(XmlNode node)
        {
            if (!bool.TryParse(node.InnerText, out bool tVal)) throw new Exception("BooleanNode contains invalid boolean value (true/false are valid)");
            val = tVal;
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
