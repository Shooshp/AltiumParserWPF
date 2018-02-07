using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker.Templates
{
    public class CodeTemplateClass : CodeBlock
    {
        public CodeTemplateClass(List<string> code, string name)
        {
            var title = "class " + name;
            AddCode(code, title, true);

            Code[Code.Count - 2] += ";";
        }
    }
}
