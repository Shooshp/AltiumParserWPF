﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AltiumParserWPF.Analysis;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF
{
    public class NewEttBoard : PCB
    {
        public int DutCount;
        public List<DUT> Duts;

        public NewEttBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            DutCount = GetDutNumber();
            Chanels = new List<Chanel>();
            GetActiveChanels();
            GetConnections();
        }

        private void GetActiveChanels()
        {
            foreach (var component in Board.Components)
            {
                if (component.DesignItemId.Contains("DIN41612R.РОЗ.УГЛ.48")|| component.DesignItemId.Contains("DIN41612R.РОЗ.УГЛ.96"))
                {
                    foreach (var pin in component.PinList)
                    {
                        if (pin.Name.Contains("CH"))
                        {
                            if (pin.ConnectedNets.Count != 0)
                            {
                                Chanels.Add(new Chanel(pin.Name, pin.ConnectedNets.ElementAt(0).Text.ToUpper()));
                            }
                            else
                            {
                                if (pin.ConnectedPorts.Count != 0)
                                {
                                    Chanels.Add(new Chanel(pin.Name, pin.ConnectedPorts.ElementAt(0).Name.ToUpper()));
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<ConnectionUnion> GetConnections()
        {
            Connections = new List<ConnectionUnion>();

            foreach (var dut in Duts)
            {
                foreach (var entryPoint in dut.Connections)
                {
                    if (!Connections.Exists(x=>x.Name == entryPoint.Name))
                    {
                        Connections.Add(new ConnectionUnion(entryPoint.Name));
                    }

                    foreach (var chanel in Chanels)
                    {
                        if (chanel.ConnectionName == entryPoint.Connection)
                        {
                            chanel.ConnectedObjects.Add(dut.Name + ":" + entryPoint.Name);
                            Connections.Single(x => x.Name == entryPoint.Name).Add(chanel);
                        }
                    }
                }
            }

            Connections.RemoveAll(x => x.Chanels.Count == 0);

            foreach (var connectionUinion in Connections)
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
                    connectionUinion.Chanels.RemoveRange(1, DutCount-1);
                }
            }

            foreach (var chanel in Chanels)
            {
                var isConnected = false;

                foreach (var connectionUinion in Connections)
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
                    Connections.Add(new ConnectionUnion(chanel.ConnectionName));
                    Connections.Single(x=>x.Name == chanel.ConnectionName).Add(chanel);
                }
            }

            return Connections;
        }

        public void PrintReport()
        {
            Console.WriteLine("# Complite analysis of ETT PCB " + Path.GetFileName(Board.FilePath));
            Console.WriteLine("# Genereted by " + System.AppDomain.CurrentDomain.FriendlyName + " " 
                              + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("# Date: " + DateTime.Now);
            Console.WriteLine("# Disclamer: ");
            Console.WriteLine(
                "# Future usage of data generated by this application must be taken with extreme precautions.");
            Console.WriteLine("# Author does not take any responsibility for any kind of damage could be caused by usage of this data. \n\n");

            var result = Connections.OrderBy(x => x, new AlphanumComparatorFast());

            foreach (var connection in result)
            {
                Console.WriteLine(connection);
            }

        }

        private int GetDutNumber()
        {
            Duts = new List<DUT>();

            var temp = new List<DUT>();
            foreach (var sheet in Board.SheetSymbols)
            {
                if (sheet.SheetName.Text.ToUpper().Contains("DUT"))
                {
                    temp.Add(new DUT(sheet));
                }
            }
            Duts = temp.OrderBy(x => x, new AlphanumComparatorFast()).ToList();

            return Duts.Count;
        }
    }
}
