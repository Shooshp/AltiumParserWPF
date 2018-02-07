using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker.Templates
{
    public abstract class CodeBlock
    {
        public string Title;
        public List<string> Code;

        protected void AddCode(List<string> code, string title, bool tabulate)
        {
            Code = new List<string>();
            Title = title;

            var tabulation = "";

            if (tabulate)
            {
                tabulation = "\t";
            }

            Code.Add(tabulation + Title);
            Code.Add(tabulation + "{");

            foreach (var line in code)
            {
                Code.Add(tabulation + "\t" + line);
            }

            Code.Add(tabulation + "}");
            Code.Add("");
        }
    }
}
