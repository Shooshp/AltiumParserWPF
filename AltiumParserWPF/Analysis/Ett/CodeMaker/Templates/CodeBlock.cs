using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public abstract class CodeBlock
    {
        public string Title;
        public List<string> Code;

        protected void AddCode(List<string> code, string title)
        {
            Code = new List<string>();
            Title = title;

            Code.Add("");
            Code.Add("\t" + Title);
            Code.Add("\t" + "{");

            foreach (var line in code)
            { 
                if(!string.IsNullOrEmpty(line))
                {
                    Code.Add("\t\t" + line);
                }
            }

            Code.Add("\t" + "}");
            Code.Add("");
        }
    }
}
