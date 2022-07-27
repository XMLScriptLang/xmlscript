using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace xmlscript.FinalNodes
{
    public class CallNode : Node
    {
        public string attrTypeTarget = null;
        public string attrMethodTarget = null;

        public List<Node> argumentNodes = new List<Node>();

        public override Node FromXmlTag(XmlNode node)
        {
            // todo: parse-time-existance check option
            if (node.Attributes == null || node.Attributes["type"] == null || node.Attributes["method"] == null) throw new Exception("CallNode is missing type and/or method attribute.");
            attrTypeTarget = node.Attributes["type"].Value;
            attrMethodTarget = node.Attributes["method"].Value;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                argumentNodes.Add(Program.ParseNode(childNode));
            }

            return this;
        }

        public override object Visit(Scope scope)
        {
            var type = ResolveType(attrTypeTarget);
            if (type == null) throw new Exception("Unable to resolve type target " + attrTypeTarget);

            object[] args = new object[argumentNodes.Count];
            Type[] argTypes = new Type[argumentNodes.Count];
            for (int i = 0; i < argumentNodes.Count; i++)
            {
                args[i] = argumentNodes[i].Visit(scope);
                argTypes[i] = args[i].GetType();
            }

            var method = type.GetMethod(attrMethodTarget, types: argTypes);
            if (method == null) throw new Exception("Unable to resolve method target " + attrMethodTarget);

            method.Invoke(this, args);
            return null;
        }

        public static Type ResolveType(string name)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
                    if (t.FullName == name || t.Name == name) return t;
                }
            }

            return null;
        }
    }
}
