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

            var parser = new AltiumParser.AltiumParser(filename);

            var type = PcbAnalysis.GetPsbType(parser);

            switch (type)
            {
                case PcbAnalysis.PcbTypes.EttNew:
                    var pcb = new NewEttBoard(parser);
                    pcb.PrintReport();
                    break;
            }
        }
    }
}
