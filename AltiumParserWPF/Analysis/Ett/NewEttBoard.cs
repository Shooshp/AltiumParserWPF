﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AltiumParserWPF.Analysis;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF
{
    public class NewEttBoard : EttBoard
    {
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

        private void GetConnections()
        {
            Connections = new List<ConnectionUinion>();

            foreach (var dut in Duts)
            {
                foreach (var entryPoint in dut.Connections)
                {
                    if (!Connections.Exists(x=>x.Name == entryPoint.Name))
                    {
                        Connections.Add(new ConnectionUinion(entryPoint.Name));
                    }

                    foreach (var chanel in Chanels)
                    {
                        if (chanel.Connection == entryPoint.Connection)
                        {
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
                    if (chanel.Name != tempname)
                    {
                        tempname = chanel.Name;
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
            foreach (var connection in Connections)
            {
                Console.WriteLine(connection);
            }

        }
    }
}