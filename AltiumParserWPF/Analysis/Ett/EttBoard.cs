using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis
{
    public abstract class EttBoard
    {
        public int DutCount;
        public int GroupeCount;
        public List<Chanel> Chanels;
        public List<DUT> Duts;
        public AltiumParser.AltiumParser Board;
        public List<ConnectionUinion> Connections;

        protected int GetDutNumber()
        {
            Duts = new List<DUT>();

            foreach (var sheet in Board.SheetSymbols)
            {
                if (sheet.SheetName.Text.ToUpper().Contains("DUT"))
                {
                    Duts.Add(new DUT(sheet));
                }
            }

            return Duts.Count;
        }
    }

    [DebuggerDisplay("Name:{Name}, Count:{Chanels.Count}")]
    public class ConnectionUinion
    {
        public string Name;
        public List<Chanel> Chanels;

        public ConnectionUinion(string name)
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

            if (Chanels.Count>1)
            {
                line += "const u8 " + Name.ToUpper() + "[" + Chanels.Count + "] = \n{\n \t";
                var counter = 1;
                foreach (var chanel in Chanels)
                {
                    line += chanel.Name.Replace("CH ", "");
                    if (counter!=Chanels.Count)
                    {
                        line += ", ";
                        if (counter%10 == 0)
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
                line = "typedef Ch" + Chanels.ElementAt(0).Name.Replace("CH ", "") + " " + Name.ToUpper() + ";\n";
            }

            return line;
        }
    }
}
