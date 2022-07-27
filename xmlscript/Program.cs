using System.Diagnostics;
using System.Xml;
using xmlscript.FinalNodes;

namespace xmlscript
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                while(true)
                {
                    Console.WriteLine("Path to run: ");
                    string path = Console.ReadLine();

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);

                    if(doc.GetElementsByTagName("xmlscript").Count > 0)
                    {
                        Stopwatch sw = new();

                        sw.Start();
                        Node n = ParseNode(doc.GetElementsByTagName("xmlscript")[0]);
                        sw.Stop();
                        Debug.WriteLine("Parsing took " + sw.ElapsedMilliseconds + "ms");
                        sw.Restart();
                        n.Visit(new Scope());
                        sw.Stop();
                        Debug.WriteLine("Interpreting took " + sw.ElapsedMilliseconds + "ms");
                    }else
                    {
                        Console.WriteLine("No root element found! Needs to be named xmlscript!");
                    }
                }
            }
        }

        public static Node ParseNode(XmlNode node)
        {
            switch(node.Name)
            {
                case "string":
                    return new StringNode().FromXmlTag(node);
                case "number":
                case "double":
                case "num":
                    return new NumberNode().FromXmlTag(node);
                case "call":
                    return new CallNode().FromXmlTag(node);
                case "col":
                case "xmlscript":
                    return new NodeCollection().FromXmlTag(node);
                case "if":
                    return new IfNode().FromXmlTag(node);
                case "getvar":
                case "readvar":
                    return new VarGetNode().FromXmlTag(node);
                case "setvar":
                    return new VarAssignNode().FromXmlTag(node);
                case "for":
                    return new ForNode().FromXmlTag(node);
                case "pass":
                case "nop":
                    return new PassNode().FromXmlTag(node);
                default:
                    if(node.Attributes != null && node.Attributes["ignore"] != null && node.Attributes["ignore"].Value != "true") throw new Exception("Unknown node: " + node.Name + ", you can add the ignore=\"true\" attribute to ignore this.");
                    return null;
            }
        }
    }
}