using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class CodeTemplateFor : CodeBlock
    {
        public CodeTemplateFor(string itteratorname, string maxlimit, List<string> code)
        {
            var title = "for(u8 " + itteratorname + "=0; " + itteratorname + "<" + maxlimit + "; " + itteratorname +
                        "++)";
            AddCode(code, title);
        }
    }
}
