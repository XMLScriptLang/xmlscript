using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class IfNode : Node
    {
        public List<Node> Nodes = new List<Node>();
        public Node FunctionNode, ElseFunctionNode;
        public Node ConditionalNode;

        public override Node FromXmlTag(XmlNode node)
        {

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "condition")
                {
                    ConditionalNode = new ConditionalNode().FromXmlTag(childNode);
                }
                else if (childNode.Name == "then")
                {
                    FunctionNode = new NodeCollection(true).FromXmlTag(childNode);
                }
                else if (childNode.Name == "else")
                {
                    ElseFunctionNode = new NodeCollection(true).FromXmlTag(childNode);
                }
            }

            if (ConditionalNode == null || FunctionNode == null) throw new Exception("Unfinished if tag.");

            return this;
        }

        public override object Visit(Scope scope)
        {
            if((bool)ConditionalNode.Visit(scope))
            {
                FunctionNode.Visit(scope);
            }else
            {
                ElseFunctionNode?.Visit(scope);
            }

            return null;
        }
    }
}
