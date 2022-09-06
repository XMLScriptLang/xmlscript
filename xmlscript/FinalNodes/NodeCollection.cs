using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class NodeCollection : Node
    {
        public List<Node> Nodes = new List<Node>();
        public bool ownScope = false;
        public NodeCollection(bool ownScope = false)
        {
            this.ownScope = ownScope;
        }

        public override Node FromXmlTag(XmlNode node)
        {
            foreach(XmlNode childNode in node.ChildNodes)
            {
                Nodes.Add(Program.ParseNode(childNode));
            }

            return this;
        }

        public override object Visit(Scope scope)
        {
            foreach (Node node in Nodes) node.Visit((ownScope ? Scope.FromParent(scope) : scope));
            return null;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            string output = "";
            foreach (Node node in Nodes) output += node.Transpile(scope) + "\n";
            return output;
        }
    }
}
