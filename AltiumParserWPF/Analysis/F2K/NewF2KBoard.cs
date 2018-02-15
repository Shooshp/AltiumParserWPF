using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.AltiumParser.Records;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis.F2K
{
    class NewF2KBoard : PCB
    {
        public DUT DUT;

        public NewF2KBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            ActiveChanels = new List<Chanel>();
            FreeChanels = new List<Chanel>();
            GetActiveChanels();

            DUT = DeterminateDut();
            Connections = new List<ConnectionUnion>();
            Connections = GetConnections();
        }


        private void GetActiveChanels()
        {
            foreach (var component in Board.Components)
            {
                if ((component.DesignItemId.Contains("FORMULA-256") || component.DesignItemId.Contains("FORMULA256")) && component.CurrentPartId < 17)
                {
                    foreach (var pin in component.PinList)
                    {
                        if (ChanelMap.F2KMapping.Exists(x => x.Socket == pin.Designator))
                        {
                            var tempname = ChanelMap.F2KMapping.Single(x => x.Socket == pin.Designator).Chanel;
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
        }

        public DUT DeterminateDut()
        {
            var connectionCounters = new  List<ConnectionCounter>();

            foreach (var component in Board.Components)
            {
                if (!(component.DesignItemId.Contains("FORMULA-256")||component.DesignItemId.Contains("FORMULA256") || component.SourceLibraryName.Contains("KONT_TOCH")))
                {
                    connectionCounters.Add(new ConnectionCounter(component));

                    foreach (var pin in component.PinList)
                    {
                        var connection = "";

                        if (pin.ConnectedNets.Count !=0)
                        {
                            connection = pin.ConnectedNets.ElementAt(0).Text.ToUpper();
                        }
                        else
                        {
                            if (pin.ConnectedPorts.Count != 0)
                            {
                                connection = pin.ConnectedPorts.ElementAt(0).Name.ToUpper();
                            }
                        }

                        if (ActiveChanels.Exists(x => x.ConnectionName == connection))
                        {
                            connectionCounters.Single(x => x.Component == component).Count++;
                        }
                    }
                }

            }

            var resulresultlist = connectionCounters.OrderByDescending(x => x.Count).ToList();
            var combinedComponent = CombineMultyPartComponent(resulresultlist.ElementAt(0).Component.Designator.Text);
            var result = new DUT(combinedComponent);
            return result;
        }

        public List<ConnectionUnion> GetConnections()
        {
            var tempconnections = new List<ConnectionUnion>();

            foreach (var entryPoint in DUT.Connections)
            {
                tempconnections.Add(new ConnectionUnion(entryPoint.Name));

                foreach (var chanel in ActiveChanels)
                {
                    if (entryPoint.Connection.Exists(x => x == chanel.ConnectionName))
                    {
                        chanel.ConnectedObjects.Add(DUT.Name + ":" + entryPoint.Name);
                        tempconnections.Single(x => x.Name == entryPoint.Name).Add(chanel);
                    }
                }
            }

            tempconnections.RemoveAll(x => x.Chanels.Count == 0);

            var sortedconnections = tempconnections.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();

            return sortedconnections;
        }
    }

    public class ConnectionCounter
    {
        public Component Component;
        public int Count;

        public ConnectionCounter(Component component)
        {
            Component = component;
            Count = 0;
        }
    }
}
