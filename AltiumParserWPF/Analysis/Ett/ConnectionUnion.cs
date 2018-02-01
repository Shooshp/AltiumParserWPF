using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.Analysis.Ett
{
    [DebuggerDisplay("Name:{Name}, Count:{Chanels.Count}")]
    public class ConnectionUnion
    {
        public string Name;
        public List<Chanel> Chanels;

        public ConnectionUnion(string name)
        {
            Name = name;
            Chanels = new List<Chanel>();
        }

        public void Add(Chanel chanel)
        {
            Chanels.Add(chanel);
        }

        public override string ToString()
        {
            var line = "";

            if (Chanels.Count > 1)
            {
                line += "const u8 " + Name.ToUpper() + "[" + Chanels.Count + "] = \n{\n \t";
                var counter = 1;
                foreach (var chanel in Chanels)
                {
                    line += chanel.ChanelName.Replace("CH ", "");
                    if (counter != Chanels.Count)
                    {
                        line += ", ";
                        if (counter % 10 == 0)
                        {
                            line += "\n \t";
                        }
                    }
                    else
                    {
                        line += "\n};\n";
                    }

                    counter++;
                }
            }
            else
            {
                line = "typedef Ch" + Chanels.ElementAt(0).ChanelName.Replace("CH ", "") + " " + Name.ToUpper() + ";\n";
            }

            return line;
        }
    }
}
