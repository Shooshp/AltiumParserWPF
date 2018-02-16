using System;
using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.AltiumParser.Records;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Analysis.F2K
{
    class NewF2KBoard : PCB
    {
        public DUT DUT;
        public List<BlackBox> BlackBoxes;

        public NewF2KBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            ActiveChanels = new List<Chanel>();
            FreeChanels = new List<Chanel>();
            BlackBoxes = new List<BlackBox>();
            GetActiveChanels();

            DUT = DeterminateDut();
            GetBlackBoxes();
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

            foreach (var blackBox in BlackBoxes)
            {
                for (int i = 0; i < 4; i++)
                {
                    var commutationPair = new string[2];

                    for (int j = 0; j < 2; j++)
                    {
                        commutationPair[j] = blackBox.CommutationPairs[i, j];
                    }

                    foreach (var chanel in ActiveChanels)
                    {
                        if (commutationPair.Contains(chanel.ConnectionName))
                        {
                            foreach (var entryPoint in DUT.Connections)
                            {
                                if (entryPoint.Connection.Exists(x => commutationPair.Contains(x)))
                                {
                                    tempconnections.Add(new ConnectionUnion(entryPoint.Name));
                                    chanel.ConnectedObjects.Add(DUT.Name + ":" + entryPoint.Name + "via BlackBox " + blackBox.Component.Designator.Text);
                                    tempconnections.Single(x => x.Name == entryPoint.Name).Add(chanel);
                                }
                            }
                        }
                    }
                }
            }

            var sortedconnections = tempconnections.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();

            return sortedconnections;
        }

        private void GetBlackBoxes()
        {
            foreach (var component in Board.Components)
            {
                if (component.PinList.Count == 16)
                {
                    var match = true;

                    foreach (var pin in component.PinList)
                    {
                        if (Convert.ToInt32(pin.Designator) % 2 == 0 && pin.ConnectedPowerPorts.Count != 0)
                        {
                            if (pin.ConnectedPowerPorts.Exists(x=>x.Text.ToUpper().Contains("GND")))
                            {
                                match = true;
                            }
                            else
                            {
                                match = false;
                            }
                        }
                        else
                        {
                            match = false;
                        }
                    }

                    if(match)
                    {
                        BlackBoxes.Add(new BlackBox(component));
                    }
                }
            }

            foreach (var blackBox in BlackBoxes)
            {
                blackBox.empty = true;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (blackBox.CommutationPairs[i, j] != null)
                        {
                            blackBox.empty = false;
                        }
                    }
                }
            }

            BlackBoxes.RemoveAll(x => x.empty);
        }
    }

    public class BlackBox
    {
        public Component Component;
        public string[,] CommutationPairs;
        public bool empty;

        public BlackBox(Component component)
        {
            Component = component;
            GetCommutationPairs();
        }

        private void GetCommutationPairs()
        {
            CommutationPairs = new string[4,2];

            var tepmpinlist = Component.PinList.OrderBy(x => Convert.ToInt32(x.Designator)).ToList();
            for (var i = 1; i < 14; i+=4)
            {
                var firstPin = tepmpinlist.Single(x => Convert.ToInt32(x.Designator) == i);
                var seconPin = tepmpinlist.Single(x => Convert.ToInt32(x.Designator) == i + 2);

                if (firstPin.ConnectionsList.Count != 0)
                {
                    CommutationPairs[i / 4, 0] = firstPin.ConnectionsList?.First();

                    if (seconPin.ConnectionsList.Count != 0)
                    {
                        CommutationPairs[i / 4, 1] = seconPin.ConnectionsList?.First();
                    }
                    else
                    {
                        CommutationPairs[i / 4, 0] = null;
                    }
                }
            }
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
