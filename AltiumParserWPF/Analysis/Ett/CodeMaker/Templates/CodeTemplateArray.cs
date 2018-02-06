using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class CodeTemplateArray : CodeBlock
    {
        public CodeTemplateArray(string name, List<string> members)
        {
            var title = "const u8 " + name + "[" + members.Count + "] =";
            var tempLines = new List<string>();

            var counter = 1;
            var templine = "";

            foreach (var member in members)
            {
                templine += member + ", ";
                if (counter % 10 == 0)
                {
                    tempLines.Add(templine);
                }

                counter++;
            }

            if (!string.IsNullOrEmpty(templine))
            {
                tempLines.Add(templine);
            }

            AddCode(tempLines, title);
        }
    }
}
