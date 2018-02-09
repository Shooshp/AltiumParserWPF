using System.Collections.Generic;
using System.Text;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis
{
    public abstract class PCB
    {
        public AltiumParser.AltiumParser Board;
        public List<Chanel> ActiveChanels;
        public List<Chanel> FreeChanels;
        public List<ConnectionUnion> Connections;

        public static string RemoveSpecialCharacters(string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append("_");
                }
            }
            return sb.ToString();
        }
    }
}
