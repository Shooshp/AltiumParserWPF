using System;
using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.AltiumParser.Records;

namespace AltiumParserWPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var filename = @"E:\SN74LVCH245ADBR_ETT_v2\SN74LVCH245ADBR_ETT_v2\SN74LVCH245ADBR_ETT_v2.SchDoc";
            //var filename = @"E:\test\test.SchDoc";


            var small = "DIN41612R.РОЗ.УГЛ.48";
            var big = "DIN41612R.РОЗ.УГЛ.96";

            var parser = new AltiumParser.AltiumParser(filename);

            foreach (var DUT in parser.SheetSymbols)
            {
                Console.WriteLine("=== " + DUT.SheetName.Text.ToUpper() + " ===");
                foreach (var entry in DUT.SheetEntriesList)
                {
                    if (entry.ConnectedNets.Count != 0)
                    {                      
                        foreach (var component in parser.Components)
                        {
                            foreach (var pin in component.PinList)
                            {
                                foreach (var net in pin.ConnectedNets)
                                {
                                    if (entry.ConnectedNets.ElementAt(0).Text == net.Text) 
                                    {
                                        Console.WriteLine(entry.Name + " Connected to " + pin.Name);
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
