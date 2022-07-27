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
        public double val;

        public override Node FromXmlTag(XmlNode node)
        {
            if (!double.TryParse(node.InnerText, out double tVal)) throw new Exception("NumberNode contains invalid number");
            val = tVal;
            return this;
        }

        public override object Visit(Scope scope)
        {
            return val;
        }
    }
}
