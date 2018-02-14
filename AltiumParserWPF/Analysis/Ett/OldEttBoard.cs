using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett
{
    public class OldEttBoard : EttPCB
    {
        public OldEttBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            DutCount = GetDutNumber();
            FreeChanels = new List<Chanel>();
            ActiveChanels = new List<Chanel>();
            GetActiveChanels();

            Connections = new List<ConnectionUnion>();
            Connections = GetConnections();
        }

        private void GetActiveChanels()
        {
            foreach (var component in Board.Components)
            {
                if (component.DesignItemId.Contains("CONN_DIN_48") || component.DesignItemId.Contains("CONN_DIN_96"))
                {
                    foreach (var pin in component.PinList)
                    {
                        var tempname = "";
                        if (component.DesignItemId.Contains("CONN_DIN_48"))
                        {
                            tempname = "J2_" + pin.Name;
                        }
                        if (component.DesignItemId.Contains("CONN_DIN_96"))
                        {
                            tempname = "J1_" + pin.Name;
                        }

                        if (ChanelMap.EttMapping.Exists(x => x.Socket == tempname))
                        {
                            if (pin.ConnectedNets.Count != 0)
                            {
                                ActiveChanels.Add(new Chanel(tempname, pin.ConnectedNets.ElementAt(0).Text.ToUpper()));
                            }
                            else
                            {
                                if (pin.ConnectedPorts.Count != 0)
                                {
                                    ActiveChanels.Add(new Chanel(tempname, pin.ConnectedPorts.ElementAt(0).Name.ToUpper()));
                                }
                            }
                        }
                    }
                }
            }

            foreach (var avaliblechanel in ChanelMap.EttMapping)
            {
                if (!ActiveChanels.Exists(x => x.ChanelName == avaliblechanel.Socket))
                {
                    FreeChanels.Add(new Chanel(avaliblechanel.Socket, "free"));
                }
            }
        }

        public List<ConnectionUnion> GetConnections()
        {
            var tempconnections = new List<ConnectionUnion>();

            foreach (var dut in Duts)
            {
                foreach (var entryPoint in dut.Connections)
                {
                    if (!tempconnections.Exists(x => x.Name == entryPoint.Name))
                    {
                        tempconnections.Add(new ConnectionUnion(entryPoint.Name));
                    }

                    foreach (var chanel in ActiveChanels)
                    {
                        if (chanel.ConnectionName == entryPoint.Connection)
                        {
                            chanel.ConnectedObjects.Add(dut.Name + ":" + entryPoint.Name);
                            tempconnections.Single(x => x.Name == entryPoint.Name).Add(chanel);
                        }
                    }
                }
            }

            tempconnections.RemoveAll(x => x.Chanels.Count == 0);

            foreach (var connectionUinion in tempconnections)
            {
                var counter = 1;
                var tempname = "";
                foreach (var chanel in connectionUinion.Chanels)
                {
                    if (chanel.ChanelName != tempname)
                    {
                        tempname = chanel.ChanelName;
                    }
                    else
                    {
                        counter++;
                    }
                }

                if (counter == DutCount)
                {
                    connectionUinion.Chanels.RemoveRange(1, DutCount - 1);
                }
            }

            foreach (var chanel in ActiveChanels)
            {
                var isConnected = false;

                foreach (var connectionUinion in tempconnections)
                {
                    foreach (var connectedChanel in connectionUinion.Chanels)
                    {
                        if (connectedChanel.ChanelName == chanel.ChanelName)
                        {
                            isConnected = true;
                        }
                    }
                }

                if (!isConnected)
                {
                    tempconnections.Add(new ConnectionUnion(chanel.ConnectionName));
                    tempconnections.Single(x => x.Name == chanel.ConnectionName).Add(chanel);
                }
            }

            foreach (var connectionUnion in tempconnections)
            {
                if (connectionUnion.Chanels.Count == 1)
                {
                    connectionUnion.ConnectionType = ConnectionType.Global;
                }
                else
                {
                    connectionUnion.ConnectionType = ConnectionType.Array;
                }

                connectionUnion.Name = RemoveSpecialCharacters(connectionUnion.Name);
            }

            var sortedconnections = tempconnections.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();

            return sortedconnections;
        }
    }
}
