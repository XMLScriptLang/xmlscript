using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class VarGetNode : Node
    {
        public string attrName;

        public override Node FromXmlTag(XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["name"] == null) throw new Exception("name attribute missing on VarGetNode");
            attrName = node.Attributes["name"].Value;
            return this;
        }

        public override object Visit(Scope scope)
        {
            return scope.Get(attrName);
        }
    }
}
