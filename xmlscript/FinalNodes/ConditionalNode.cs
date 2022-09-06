using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmlscript.FinalNodes
{
    public class ConditionalNode : Node
    {
        public List<Node> Nodes = new List<Node>();
        public Node LeftSide, RightSide;
        public string Op;

        public override Node FromXmlTag(XmlNode node)
        {

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "left")
                {
                    if(childNode.ChildNodes.Count > 0) LeftSide = Program.ParseNode(childNode.ChildNodes[0]);
                    if (childNode.ChildNodes.Count > 1) Console.WriteLine("[warn] Left/Right conditional nodes had more than one child. Omitted.");
                } else if (childNode.Name == "right")
                {
                    if (childNode.ChildNodes.Count > 0) RightSide = Program.ParseNode(childNode.ChildNodes[0]);
                    if (childNode.ChildNodes.Count > 1) Console.WriteLine("[warn] Left/Right conditional nodes had more than one child. Omitted.");
                }
                else if (childNode.Name == "op")
                {
                    Op = childNode.InnerText;
                }
            }

            if (LeftSide == null || RightSide == null) throw new Exception("Unfinished conditional node.");

            return this;
        }

        public override object Visit(Scope scope)
        {
            var leftRes = LeftSide.Visit(scope);
            var rightRes = RightSide.Visit(scope);

            switch (Op)
            {
                case "==":
                    if (leftRes.Equals(rightRes)) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                case "!=":
                    if (!leftRes.Equals(rightRes)) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                case ">":
                case ">=":
                case "<":
                case "<=":
                    if (leftRes is not double || rightRes is not double) throw new Exception("Left and right side must be numeric for this operator. (left is " + leftRes.GetType().Name + " and right is " + rightRes.GetType().Name + ")");

                    switch(Op)
                    {
                        case ">":
                            if ((double)leftRes > (double)rightRes) {
                                return true;
                            } else {
                                return false;
                            }
                            break;
                        case "<":
                            if ((double)leftRes < (double)rightRes) {
                                return true;
                            } else {
                                return false;
                            }
                            break;
                        case ">=":
                            if ((double)leftRes >= (double)rightRes) {
                                return true;
                            } else {
                                return false;
                            }
                            break;
                        case "<=":
                            if ((double)leftRes <= (double)rightRes) {
                                return true;
                            } else {
                                return false;
                            }
                            break;

                    }


                    break;
            }

            return null;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            return $"{LeftSide.Transpile(scope)} {Op} {RightSide.Transpile(scope)}";
        }
    }
}
