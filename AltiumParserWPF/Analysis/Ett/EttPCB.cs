using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.Analysis.Ett
{
    public abstract class EttPCB : PCB
    {
        public int DutCount;
        public List<DUT> Duts;

        public int GetDutNumber()
        {
            Duts = new List<DUT>();

            var temp = new List<DUT>();
            foreach (var sheet in Board.SheetSymbols)
            {
                if (sheet.SheetName.Text.ToUpper().Contains("DUT") || sheet.SheetFile.Text.ToUpper().Contains("DUT"))
                {
                    temp.Add(new DUT(sheet));
                }
            }
            Duts = temp.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();

            return Duts.Count;
        }
    }
}
