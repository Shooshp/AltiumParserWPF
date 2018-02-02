using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF
{
    public partial class MainWindow
    {
        public List<ConnectionUnion> Connections { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Connections = new List<ConnectionUnion>();

            var filename = @"F:\SN74LVCH245ADBR_ETT_v2\SN74LVCH245ADBR_ETT_v2.SchDoc";
            //var filename = @"F:\MAX4508ESE_ETT_v2\MAX4508ESE_ETT_v2.SchDoc";

            var parser = new AltiumParser.AltiumParser(filename);
            var type = PcbAnalysis.GetPsbType(parser);

            

            switch (type)
            {
                case PcbAnalysis.PcbTypes.EttNew:
                    var pcb = new NewEttBoard(parser);
                    Connections = pcb.Connections;
                    break;
            }

            foreach (var connection in Connections)
            {
                Console.WriteLine(connection);
            }

            DataContext = Connections;
        } 
    }
}
