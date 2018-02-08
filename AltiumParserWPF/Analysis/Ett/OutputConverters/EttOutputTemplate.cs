using System.Collections.Generic;

namespace AltiumParserWPF.Analysis.Ett.OutputConverters
{
    public abstract class EttOutputTemplate
    {
        public List<ConnectionUnion> Unions;
        public string Container;
        public string Name;

        protected void CombineList(List<string> list)
        {
            foreach (var line in list)
            {
                if (line.Contains("\n"))
                {
                    Container += line;
                }
                else
                {
                    Container += line + "\n";
                }
            }
        }
    }
}
