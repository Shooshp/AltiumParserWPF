using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class CodeTemplateClass : CodeBlock
    {
        public CodeTemplateClass(List<string> code, string name)
        {
            var title = "class " + name;
            AddCode(code, title);
        }
    }
}
