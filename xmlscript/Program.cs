using System.Diagnostics;
using System.IO;
using System.Xml;
using xmlscript.FinalNodes;

namespace xmlscript
{
    public class Program
    {
        private static Node parserCache;
        
        public static void Main(string[] args)
        {
            // load xmlscript.lib into currentdomain
            var ___ = new XMLScript.Lib.__Dummy();
            ___.__DummyMethod();
            
            if(args.Length == 0)
            {
                Console.WriteLine("=== Welcome to XMLScript CLI ===");
                while(true)
                {
                    Console.WriteLine("Path to run: ");
                    string path = Console.ReadLine();
                    bool transpile = false, parseOnly = false;

                    if (path == "")
                    {
                        if (parserCache == null)
                        {
                            Console.WriteLine("No cached parse tree found!");
                            continue;
                        }
                        Stopwatch sw = new();
                        sw.Start();
                        parserCache.Visit(new Scope());
                        sw.Stop();
                        Debug.WriteLine("Interpreting took " + sw.ElapsedMilliseconds + "ms");
                        continue;
                    }

                    // read args
                    string[] pathArgs = path.Split(' ');

                    if (pathArgs.Length > 1)
                    {
                        path = pathArgs[0];
                        for (int i = 1; i < pathArgs.Length; i++)
                        {
                            if (pathArgs[i] == "-t")
                            {
                                transpile = true;
                            }
                            else if (pathArgs[i] == "-p")
                            {
                                parseOnly = true;
                            }
                        }
                    }

                    Run(path, new() { parseOnly = parseOnly, transpile = transpile});
                }
            }else
            {
                bool transpile = false;
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("File not found.\nSyntax: xmlscript <file> [... options]");
                    return;
                }

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] == "-t")
                    {
                        transpile = true;
                    }
                }

                Run(args[0], new() { parseOnly = false, transpile = transpile });
            }
        }

        public static void Run(string path, RunOptions opts)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            if (doc.GetElementsByTagName("xmlscript").Count > 0)
            {
                Stopwatch sw = new();

                sw.Start();
                Node n = ParseNode(doc.GetElementsByTagName("xmlscript")[0]);
                sw.Stop();
                Debug.WriteLine("Parsing took " + sw.ElapsedMilliseconds + "ms");

                if (opts.parseOnly)
                {
                    parserCache = n;
                    Console.WriteLine("Parsed successfully, enter empty path to execute cached.");
                    return;
                }

                if (!opts.transpile)
                {
                    sw.Restart();
                    n.Visit(new Scope());
                    sw.Stop();
                    Debug.WriteLine("Interpreting took " + sw.ElapsedMilliseconds + "ms");
                }
                else
                {
                    sw.Restart();
                    string output = n.Transpile(new Scope());
                    sw.Stop();
                    Debug.WriteLine("Transpilation took " + sw.ElapsedMilliseconds + "ms");
                    Console.WriteLine(output);
                }
            }
            else
            {
                Console.WriteLine("No root element found! Needs to be named xmlscript!");
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
                case "bool":
                case "boolean":
                    return new BooleanNode().FromXmlTag(node);
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
                case "new":
                case "create":
                    return new CreateNode().FromXmlTag(node);
                default:
                    if(node.Attributes != null && node.Attributes["ignore"] != null && node.Attributes["ignore"].Value != "true") throw new Exception("Unknown node: " + node.Name + ", you can add the ignore=\"true\" attribute to ignore this.");
                    return null;
            }
        }
    }

    public struct RunOptions
    {
        public bool parseOnly = false, transpile = false;

        public RunOptions()
        {
        }
    }
}