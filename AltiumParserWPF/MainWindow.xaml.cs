using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
            myWindow.SizeChanged += MyWindowOnSizeChanged;

            //var filename = @"\\S14\нц сэо\Проекты\Тип ЭРИ_5576РТ1У\Проект ПП (Altium) ЭТТ\5576RT1U_ETT\5576РТ1У_QFP-64-S_ЕТТ_V3\5576РТ1У_QFP-64-S_ЕТТ_V3.SchDoc";
            var filename = @"F:\MAX4508ESE_ETT_v2\MAX4508ESE_ETT_v2.SchDoc";

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

            ConnectionList.ItemsSource = Connections;
            ConnectionList.SelectionChanged += ConnectionListOnSelectionChanged;
        }

        private void ConnectionListOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var selectedindex = ConnectionList.SelectedIndex;
            if (selectedindex != -1)
            {
                SelectedUnion.ItemsSource = Connections.ElementAt(selectedindex).Chanels;
            }
        }

        private void MyWindowOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var args = sizeChangedEventArgs;
            var width = (args.NewSize.Width-25)/2;
            var leftLisThickness = new  Thickness(left: 0, bottom: 50, right: width, top:0);
            var rightListThickness = new Thickness(left: width, bottom: 50, right:0, top: 0);
            ConnectionList.Margin = leftLisThickness;
            SelectedUnion.Margin = rightListThickness;
        }
    }
}
