using System.Collections.Generic;
using System.Diagnostics;
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

    [DebuggerDisplay("EntryPoint:{Name}, Connection:{Connection}")]
    public class EntryPoint
    {
        public string Name;
        public List<string> Connection;

        public EntryPoint(SheetEntry entry)
        {
            Name = entry.Name.ToUpper(); 
            Connection = new List<string>();

            if (entry.ConnectedNets.Count!=0)
            {
                foreach (var net in entry.ConnectedNets)
                {
                    Connection.Add(net.Text.ToUpper());
                }
            }
            else
            {
                if (entry.ConnectedPorts.Count!=0)
                {
                    foreach (var port in entry.ConnectedPorts)
                    {
                        Connection.Add(port.Name.ToUpper());
                    }
                }
            }
        }

        public EntryPoint(Pin pin)
        {
            Name = pin.Name?.ToUpper() + "(pin:" + pin.Designator + ")";
            Connection = new List<string>();

            if (pin.ConnectedNets.Count != 0)
            {
                foreach (var net in pin.ConnectedNets)
                {
                    Connection.Add(net.Text.ToUpper());
                }
            }
            else
            {
                if (pin.ConnectedPorts.Count != 0)
                {
                    foreach (var port in pin.ConnectedPorts)
                    {
                        Connection.Add(port.Name.ToUpper());
                    }
                }
            }
        }
    }
}
