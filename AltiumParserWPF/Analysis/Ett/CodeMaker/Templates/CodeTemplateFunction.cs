using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class CodeTemplateFunction : CodeBlock
    {
        public CodeTemplateFunction(string name, List<string> code)
        {
            var title= "static " + name;
            AddCode(code, title);
        }
    }
}
