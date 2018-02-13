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

        public DUT(Component component)
        {
            Name = component.DesignItemId;
            Connections = new List<EntryPoint>();

            foreach (var pin in component.PinList)
            {
                Connections.Add(new EntryPoint(pin));
            }
        }
    }

    public class EntryPoint
    {
        public string Name;
        public string Connection;

        public EntryPoint(SheetEntry entry)
        {
            Name = entry.Name.ToUpper(); 

            if (entry.ConnectedNets.Count!=0)
            {
                Connection = entry.ConnectedNets.ElementAt(0).Text.ToUpper();
            }
            else
            {
                if (entry.ConnectedPorts.Count!=0)
                {
                    Connection = entry.ConnectedPorts.ElementAt(0).Name.ToUpper();
                }
            }
        }

        public EntryPoint(Pin pin)
        {
            Name = pin.Name.ToUpper();


            if (pin.ConnectedNets.Count != 0)
            {
                Connection = pin.ConnectedNets.ElementAt(0).Text.ToUpper();
            }
            else
            {
                if (pin.ConnectedPorts.Count != 0)
                {
                    Connection = pin.ConnectedPorts.ElementAt(0).Name.ToUpper();
                }
            }
        }
    }
}
