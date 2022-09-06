# xmlscript
XMLScript is an interpreted language where you write your code in an XML-like document

Note: Currently, this project is in a kinda-abandoned state. I wont actively update it, but I might continue it at some point.

### Transpilation
You can transpile your XMLScript to C# by appending -t at the end of the path input. However, XMLScript does not check for logical errors like putting a if in a for. It might also generated slightly wrong code (ex. missing semicolon) - even if they are very minor, they still have to be fixed so create an issue.

Formatting related issues will be closed. Proper formatting by the transpiler is definitely not a priority. You can use dotnet-format to format it automatically, if you want to.
