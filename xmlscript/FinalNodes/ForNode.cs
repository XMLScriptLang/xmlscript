using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class ForNode : Node
    {
        public List<Node> Nodes = new List<Node>();
        public Node InitNode, ConditionalNode, StepNode, FunctionNode;

        public override Node FromXmlTag(XmlNode node)
        {

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "init")
                {
                    if(childNode.ChildNodes.Count > 0) InitNode = Program.ParseNode(childNode.ChildNodes[0]);
                    if (childNode.ChildNodes.Count > 1) Console.WriteLine("[warn] For Init nodes had more than one child. Omitted.");
                } else if (childNode.Name == "cond")
                {
                    ConditionalNode = new ConditionalNode().FromXmlTag(childNode);
                }
                else if (childNode.Name == "step")
                {
                    if (childNode.ChildNodes.Count > 0) StepNode = Program.ParseNode(childNode.ChildNodes[0]);
                    if (childNode.ChildNodes.Count > 1) Console.WriteLine("[warn] For Step nodes had more than one child. Omitted.");
                }
                else if (childNode.Name == "do")
                {
                    FunctionNode = new NodeCollection(true).FromXmlTag(childNode);
                }
            }

            if (InitNode == null || ConditionalNode == null || StepNode == null || FunctionNode == null) throw new Exception("Unfinished for tag.");

            return this;
        }

        public override object Visit(Scope scope)
        {
            Scope newScope = Scope.FromParent(scope);
            InitNode.Visit(newScope);

            while(true)
            {
                if((bool)ConditionalNode.Visit(newScope))
                {
                    FunctionNode.Visit(newScope);
                }

                StepNode.Visit(newScope);
            }

            return null;
        }
    }
}
