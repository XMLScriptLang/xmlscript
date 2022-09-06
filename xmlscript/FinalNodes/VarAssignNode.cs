using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class VarAssignNode : Node
    {
        public string attrName;
        public Node valueNode;

        public override Node FromXmlTag(XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["name"] == null) throw new Exception("name attribute missing on VarAssignNode");
            attrName = node.Attributes["name"].Value;

            if (node.ChildNodes.Count < 1) throw new Exception("No child node in VarAssignNode!");
            valueNode = Program.ParseNode(node.ChildNodes[0]);
            return this;
        }

        public override object Visit(Scope scope)
        {
            scope.Set(attrName, valueNode.Visit(scope));
            return null;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            return $"var {attrName} = {valueNode.Transpile(scope)}{Utils.SemicolonOptional(args)}";
        }
    }
}
