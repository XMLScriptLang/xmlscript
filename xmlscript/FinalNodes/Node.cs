using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public abstract class Node
    {
        public abstract object Visit(Scope scope);
        public abstract Node FromXmlTag(XmlNode node);
    }
}
