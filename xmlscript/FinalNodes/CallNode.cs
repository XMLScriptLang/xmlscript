using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
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
        public Node onNode = null;

        public List<Node> argumentNodes = new List<Node>();

        public override Node FromXmlTag(XmlNode node)
        {
            // todo: parse-time-existance check option
            if (node.Attributes == null || node.Attributes["type"] == null || node.Attributes["method"] == null) throw new Exception("CallNode is missing type and/or method attribute.");
            attrTypeTarget = node.Attributes["type"].Value;
            attrMethodTarget = node.Attributes["method"].Value;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if(childNode.Name == "on")
                {
                    onNode = Program.ParseNode(childNode);
                    continue;
                }
                
                argumentNodes.Add(Program.ParseNode(childNode));
            }

            return this;
        }

        public override object Visit(Scope scope)
        {
            var type = attrTypeTarget.ResolveType();
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

            object executeOn = null;
            
            if (onNode != null)
            {
                executeOn = onNode.Visit(scope);
            }
            
            return method.Invoke(executeOn, args);
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            List<string> methodArgs = new List<string>();

            foreach(Node argNode in argumentNodes)
            {
                methodArgs.Add(argNode.Transpile(scope));
            }

            return $"{attrTypeTarget}.{attrMethodTarget}({methodArgs.Join(", ")}){Utils.SemicolonOptional(args)}";
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
