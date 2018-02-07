using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.AltiumParser.Records;

namespace AltiumParserWPF.Analysis.Ett
{
    public class DUT
    {
        public string Name;
        private SheetSymbol Sheet;
        public List<EntryPoint> Connections;

        public DUT(SheetSymbol sheetSymbol)
        {
            Sheet = sheetSymbol;
            Name = Sheet.SheetName.Text.ToUpper();
            Connections = new List<EntryPoint>();

            foreach (var sheetEntry in Sheet.SheetEntriesList)
            {
                Connections.Add(new EntryPoint(sheetEntry));
            }
        }
    }

    public class EntryPoint
    {
        public string Name;
        public string Connection;
        public SheetEntry Entry;

        public EntryPoint(SheetEntry entry)
        {
            Entry = entry;
            Name = Entry.Name;

            if (Entry.ConnectedNets.Count!=0)
            {
                Connection = Entry.ConnectedNets.ElementAt(0).Text.ToUpper();
            }
            else
            {
                if (Entry.ConnectedPorts.Count!=0)
                {
                    Connection = Entry.ConnectedPorts.ElementAt(0).Name.ToUpper();
                }
            }
        }
    }
}
