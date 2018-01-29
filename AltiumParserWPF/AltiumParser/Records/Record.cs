using System;
using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.AltiumParser.Records
{
    public abstract class Record
    {
        public bool IsConnectable;
        public int Id;
        public string RecordString;
        public List<string> Parameters;
        public Dot Connection;

        public List<Wire> ConnectedWires;
        public List<Net> ConnectedNets;
        public List<Port> ConnectedPorts;
        public List<PowerPort> ConnectedPowerPorts;

        protected void AllocateValues(object child)
        {
            var fields = GetType().GetFields(System.Reflection.BindingFlags.Public
                                                  | System.Reflection.BindingFlags.Instance
                                                  | System.Reflection.BindingFlags.DeclaredOnly);

            foreach (var parameter in Parameters)
            {
                if (parameter.Contains("="))
                {
                    var temp = parameter.Split(new[] { "=" }, StringSplitOptions.None);
                    var name = temp[0].Replace('.', '_');
                    var value = temp[1];

                    foreach (var field in fields)
                    {
                        var fieldname = field.Name.ToUpper();

                        if (fieldname == name)
                        {
                            var converted = Convert.ChangeType(value, field.FieldType);
                            field.SetValue(child, converted);
                        }
                    }
                }
            }
        }

        protected void ExtractParameters()
        {
            Parameters = new List<string>();

            var strings = RecordString.Split(new[] { "|" }, StringSplitOptions.None);
            foreach (var line in strings)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Parameters.Add(line);
                }
            }
        }

        protected void TrimRecord(string record)
        {
            var charArray = record.ToCharArray();
            var counter = 0;
            foreach (var symbol in charArray)
            {
                if (symbol == '\0' ) 
                {
                    var  temp = new string(charArray.Take(counter).ToArray());
                    RecordString = temp;
                    break;
                }

                counter++;
            }
        }

        public void CheckNets(AltiumParser parser)
        {
            ConnectedNets = new List<Net>();

            foreach (var net in parser.Nets)
            {
                if (net.Connection.IsMatch(Connection))
                {
                    ConnectedNets.Add(net);
                }
            }

            if (ConnectedWires.Count != 0)
            {
                foreach (var wire in ConnectedWires)
                {
                    foreach (var net in parser.Nets)
                    {
                        if (wire.DotIsOnLine(net.Connection))
                        {
                            if (!ConnectedNets.Exists(x=>x.UniqueId == net.UniqueId))
                            {
                                ConnectedNets.Add(net);
                            }
                        }
                    }                    
                }
            }
        }

        public void CheckPorts(AltiumParser parser)
        {
            ConnectedPorts = new List<Port>();

            foreach (var port in parser.Ports)
            {
                foreach (var connectiondot in port.ConnectionsDots)
                {
                    if (connectiondot.IsMatch(Connection))
                    {
                        ConnectedPorts.Add(port);
                    }
                }
            }

            if (ConnectedWires.Count != 0)
            {
                foreach (var wire in ConnectedWires)
                {
                    foreach (var port in parser.Ports)
                    {
                        foreach (var connectiondot in port.ConnectionsDots)
                        {
                            if (wire.ContainsDot(connectiondot))
                            {
                                if (!ConnectedPorts.Exists(x => x.UniqueId == port.UniqueId))
                                {
                                    ConnectedPorts.Add(port);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPowerPorts(AltiumParser parser)
        {
            ConnectedPowerPorts = new List<PowerPort>();

            foreach (var powerport in parser.PowerPorts)
            {
                if (powerport.Connection.IsMatch(Connection))
                {
                    ConnectedPowerPorts.Add(powerport);
                }
            }

            if (ConnectedWires.Count != 0)
            {
                foreach (var wire in ConnectedWires)
                {
                    foreach (var powerport in parser.PowerPorts)
                    {
                        if (wire.DotIsOnLine(powerport.Connection))
                        {
                            if (!ConnectedPowerPorts.Exists(x=>x.UniqueId == powerport.UniqueId))
                            {
                                ConnectedPowerPorts.Add(powerport);
                            }
                        }
                    }
                }
            }
        }


        public void CheckWires(AltiumParser parser)
        {
            ConnectedWires = new List<Wire>();

            foreach (var wire in parser.Wires)
            {
                foreach (var wireDot in wire.WireDotList)
                {
                    if (wireDot.IsMatch(Connection))
                    {
                        ConnectedWires.Add(wire);
                        break;
                    }
                }
            }

            if (ConnectedWires.Count != 0)
            {
                var isupdated = true;

                while (isupdated)
                {
                    isupdated = false;

                    foreach (var wire in parser.Wires)
                    {
                        if (!ConnectedWires.Exists( w => w.UniqueId == wire.UniqueId)) 
                        {
                            for(int i = 0; i < ConnectedWires.Count; i++)
                            {
                                var connectedWire = ConnectedWires.ElementAt(i);
                                foreach (var connectedwireDot in connectedWire.WireDotList)
                                {
                                    foreach (var wireDot in wire.WireDotList)
                                    {
                                        if (connectedwireDot.IsMatch(wireDot)) 
                                        {
                                            foreach (var junction in parser.Junctions)
                                            {
                                                if (junction.Connection.IsMatch(wireDot)) 
                                                {
                                                    if (!ConnectedWires.Exists(w => w.UniqueId == wire.UniqueId))
                                                    {
                                                        ConnectedWires.Add(wire);
                                                        isupdated = true;
                                                    }
                                                }

                                                if (isupdated)
                                                {
                                                    break;
                                                }
                                            }

                                            if (isupdated)
                                            {
                                                break;
                                            }
                                        }

                                        if (isupdated)
                                        {
                                            break;
                                        }
                                    }

                                    if (isupdated)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
