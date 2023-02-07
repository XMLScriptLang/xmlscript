using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace xmlscript.FinalNodes
{
    public class CreateNode : Node
    {
        public string attrTypeTarget = null;

        public List<Node> argumentNodes = new List<Node>();

        public override Node FromXmlTag(XmlNode node)
        {
            // todo: parse-time-existance check option
            if (node.Attributes == null || node.Attributes["type"] == null) throw new Exception("CreateNode is missing type.");
            attrTypeTarget = node.Attributes["type"].Value;

            foreach (XmlNode childNode in node.ChildNodes)
            {                
                argumentNodes.Add(Program.ParseNode(childNode));
            }

            return this;
        }

        public override object Visit(Scope scope)
        {
            var type = ResolveType(attrTypeTarget);
            if (type == null)
            {
                string assemblyName = attrTypeTarget.Substring(0, attrTypeTarget.LastIndexOf('.'));
                string typeName = attrTypeTarget.Substring(attrTypeTarget.LastIndexOf('.'));

                try
                {
                    Activator.CreateInstance(assemblyName, typeName); // create dummy object to load assembly
                    return Visit(scope);
                }catch(Exception e)
                {
                    throw new Exception("Unable to resolve type target " + attrTypeTarget);
                }
            }

            object[] args = new object[argumentNodes.Count];
            Type[] argTypes = new Type[argumentNodes.Count];
            for (int i = 0; i < argumentNodes.Count; i++)
            {
                args[i] = argumentNodes[i].Visit(scope);
                argTypes[i] = args[i].GetType();
            }

            var method = type.GetConstructor(types: argTypes);
            if (method == null) throw new Exception("Unable to resolve constructor target. Wrong number or type of arguments?");

            return method.Invoke(args);
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            List<string> methodArgs = new List<string>();

            foreach(Node argNode in argumentNodes)
            {
                methodArgs.Add(argNode.Transpile(scope));
            }

            return $"new {attrTypeTarget}({methodArgs.Join(", ")}){Utils.SemicolonOptional(args)}";
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
