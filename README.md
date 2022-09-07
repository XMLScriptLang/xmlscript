# xmlscript
XMLScript is an interpreted language where you write your code in an XML-like document

### Transpilation
You can transpile your XMLScript to C# by appending -t at the end of the path input. However, XMLScript does not check for logical errors like putting a if in a for. It might also generated slightly wrong code (ex. missing semicolon) - even if they are very minor, they still have to be fixed so create an issue.

Formatting related issues will be closed. Proper formatting by the transpiler is definitely not a priority. You can use dotnet-format to format it automatically, if you want to.

### Is this meant to be a serious project?
No, not really. Its just for fun.
