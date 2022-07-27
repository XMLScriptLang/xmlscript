using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class PassNode : Node
    {
        public override Node FromXmlTag(XmlNode node)
        {
            return this;
        }

        public override object Visit(Scope scope)
        {
            return null;
        }
    }
}
