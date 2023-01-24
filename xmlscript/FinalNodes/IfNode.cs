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
        public bool ret = false;

        public override Node FromXmlTag(XmlNode node)
        {
            if(!(node.Attributes == null || node.Attributes["return"] == null))
            {
                //Console.WriteLine("return attr found: " + node.Attributes["return"].ToString());
                ret = node.Attributes["return"].Value.ToString() == "true" ? true : false;
            }

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "condition" || childNode.Name == "cond")
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
            Console.WriteLine(ret);
            if((bool)ConditionalNode.Visit(scope))
            {
                var v = FunctionNode.Visit(scope);
                if(ret) return v;
            }else
            {
                var v = ElseFunctionNode?.Visit(scope);
                if (ret) return v;
            }

            return null;
        }

        public override string Transpile(Scope scope, Dictionary<string, object> args = null)
        {
            if (!ret)
            {
                return $@"if({ConditionalNode.Transpile(scope)}) {{
    {FunctionNode.Transpile(scope)}
}}{(ElseFunctionNode != null ? $@"else {{
    {ElseFunctionNode.Transpile(scope)}
}}" : "")}";
            }else
            {
                // return an inline if
                return $@"(({ConditionalNode.Transpile(scope)}) ? ({FunctionNode.Transpile(scope)}) : ({(ElseFunctionNode != null ? ElseFunctionNode.Transpile(scope) : "null")}))";
            }
        }
    }
}
