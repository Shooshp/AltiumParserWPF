using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltiumParserWPF.AltiumParser.Records;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis
{
    public abstract class PCB
    {
        public static AltiumParser.AltiumParser Board;
        public List<Chanel> ActiveChanels;
        public List<ConnectionUnion> FreeChanels;
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

        public static Component CombineMultyPartComponent(string designator)
        {
            if (Board.Components.Exists(x => x.Designator.Text == designator))
            {
                if (Board.Components.Count(x => x.Designator.Text == designator) > 1)
                {
                    var temlist = Board.Components.Where(x => x.Designator.Text == designator).OrderByDescending(y => y.CurrentPartId).ToList();
                    var component = temlist.ElementAt(0);

                    foreach (var part in temlist)
                    {
                        if (part != component)
                        {
                            component.PinList.AddRange(part.PinList);
                        }
                    }

                    return component;
                }
                else
                {
                    return Board.Components.Single(x => x.Designator.Text == designator);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
