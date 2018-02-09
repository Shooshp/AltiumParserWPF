using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AltiumParserWPF.Analysis;
using AltiumParserWPF.Analysis.Ett;
using AltiumParserWPF.Analysis.F2K;

namespace AltiumParserWPF.Windows
{
    public partial class ChanelSelectWindow
    {
        public List<ConnectionUnion> Connections { get; set; }
        private Window _startupWindow;
        private bool _codeclosing;

        private const string NewettSmall = "DIN41612R.РОЗ.УГЛ.48";
        private const string NewettBig = "DIN41612R.РОЗ.УГЛ.96";

        private const string OldettSmall = "CONN_DIN_48";
        private const string OldettBig = "CONN_DIN_96";

        private const string NewF2k = "FORMULA-256";

        public ChanelSelectWindow(string path, Window startupWindow)
        {
            _codeclosing = false;
            _startupWindow = startupWindow;
            Connections = new List<ConnectionUnion>();
            InitializeComponent();

            MyWindow.SizeChanged += MyWindowOnSizeChanged;

            var parser = new AltiumParser.AltiumParser(path);
            var type = GetPsbType(parser);

            PCB pcb;

            switch (type)
            {
                case PcbTypes.EttNew:
                    pcb = new NewEttBoard(parser);
                    Connections = pcb.Connections;
                    break;

                case PcbTypes.EttOld:
                    pcb = new OldEttBoard(parser);
                    Connections = pcb.Connections;
                    break;

                case PcbTypes.F2kNew:
                    pcb = new NewF2KBoard(parser);
                    break;
            }

            foreach (var connection in Connections)
            {
                Console.WriteLine(connection);
            }
            
            ConnectionList.ItemsSource = Connections;
            ConnectionList.SelectionChanged += ConnectionListOnSelectionChanged;
        }

        private static PcbTypes GetPsbType(AltiumParser.AltiumParser board)
        {
            if (board.BuildOfMaterials.Exists(x => x.DeviceType.Contains(NewettSmall))
                && board.BuildOfMaterials.Exists(x => x.DeviceType.Contains(NewettBig)))
            {
                return PcbTypes.EttNew;
            }

            if (board.BuildOfMaterials.Exists(x => x.DeviceType.Contains(OldettSmall))
                && board.BuildOfMaterials.Exists(x => x.DeviceType.Contains(OldettBig)))
            {
                return PcbTypes.EttOld;
            }

            if (board.BuildOfMaterials.Exists(x => x.DeviceType.Contains(NewF2k)))
            {
                return PcbTypes.F2kNew;
            }

            return PcbTypes.Unknown;
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
            SelectedUnion.Columns[2].Width = SelectedUnion.ActualWidth - SelectedUnion.Columns[1].ActualWidth -
                                             SelectedUnion.Columns[0].ActualWidth - 25;
        }

        private void MergeClick(object sender, RoutedEventArgs e)
        {
            var selected = ConnectionList.SelectedItems;
            var unions = new List<ConnectionUnion>();
            var tempnames = new List<string>();

            foreach (ConnectionUnion item in selected)
            {
                unions.Add(item);
                tempnames.Add(item.Name);
            }

            if (unions.Count != 1)
            {
                var predictedname = TextAnalysis.GetCommonInListOfStrings(tempnames);
                var tempunion = new ConnectionUnion("new");

                var dialog = new PopupDialog("Введите имя для нового массива.", predictedname);
                if (dialog.ShowDialog() == true)
                {
                    tempunion.Name = dialog.ResponseText;
                }

                var tempchanellist = new List<Chanel>();

                foreach (var union in unions)
                {
                    foreach (var chanel in union.Chanels)
                    {
                        tempchanellist.Add(chanel);
                    }

                    Connections.Remove(union);
                }      

                tempunion.Chanels = tempchanellist.OrderBy(x => x.ConnectionName, new AlphanumComparatorFast()).ToList();
                Connections.Add(tempunion);
                ConnectionList.ItemsSource = null;
                ConnectionList.ItemsSource = Connections;

                var selectedindex = ConnectionList.SelectedIndex;

                SelectedUnion.ItemsSource = null;
                if (selectedindex != -1)
                {
                    SelectedUnion.ItemsSource = Connections.ElementAt(selectedindex).Chanels;
                    ConnectionList.SelectedIndex = selectedindex;
                }
                else
                {
                    var lastindex = Connections.Count - 1;
                    SelectedUnion.ItemsSource = Connections.ElementAt(lastindex).Chanels;
                    ConnectionList.SelectedIndex = lastindex;
                }
            }
        }

        private void BreakClick(object sender, RoutedEventArgs e)
        {
            if (!(ConnectionList.SelectedItems.Count > 1))
            {
                var union = (ConnectionUnion)ConnectionList.SelectedItem;

                if (union.Chanels.Count > 1) 
                {
                    var tempunionlist = new List<ConnectionUnion>();

                    foreach (var chanel in union.Chanels)
                    {
                        var tempunion = new ConnectionUnion(chanel.ConnectionName);
                        tempunion.Chanels.Add(chanel);
                        tempunion.ConnectionType = ConnectionType.Global;
                        tempunionlist.Add(tempunion);
                    }

                    Connections.Remove(union);

                    foreach (var tempUnion in tempunionlist)
                    {
                        Connections.Add(tempUnion);
                    }

                    ConnectionList.ItemsSource = null;
                    ConnectionList.ItemsSource = Connections;
                }
            }
        }

        private void BreakChanelFromList(object sender, RoutedEventArgs e)
        {
            var selectedchanels = SelectedUnion.SelectedItems;
            var selectedunion = (ConnectionUnion)ConnectionList.SelectedItem;

            if (selectedunion.Chanels.Count > 1) 
            {
                var tempchanellist = new List<Chanel>();

                foreach (Chanel item in selectedchanels)
                {
                    tempchanellist.Add(item);
                }

                foreach (var chanel in tempchanellist)
                {
                    selectedunion.Chanels.Remove(chanel);
                    var tempunion = new ConnectionUnion(chanel.ConnectionName);
                    tempunion.Chanels.Add(chanel);
                    Connections.Add(tempunion);
                }

                if (selectedunion.Chanels.Count == 0)
                {
                    Connections.Remove(selectedunion);
                }


                ConnectionList.ItemsSource = null;
                ConnectionList.ItemsSource = Connections;

                var selectedindex = ConnectionList.SelectedIndex;

                SelectedUnion.ItemsSource = null;
                if (selectedindex != -1)
                {
                    SelectedUnion.ItemsSource = Connections.ElementAt(selectedindex).Chanels;
                }
                else
                {
                    var lastindex = Connections.Count - 1;
                    SelectedUnion.ItemsSource = Connections.ElementAt(lastindex).Chanels;
                }
            }
        }

        private void SelectedUnion_OnSorting(object sender, DataGridSortingEventArgs e)
        {
            var args = e;
            var column = args.Column.SortMemberPath;

            if (column == "ConnectionName" || column == "ChanelName")
            {
                args.Handled = true;
                var selectedunion = (ConnectionUnion)ConnectionList.SelectedItem;

                if (selectedunion != null) 
                {
                    if (column == "ConnectionName") 
                    {
                        selectedunion.Chanels = selectedunion.Chanels.OrderBy(x => x.ConnectionName, new AlphanumComparatorFast()).ToList();
                    }
                    else
                    {
                        selectedunion.Chanels = selectedunion.Chanels.OrderBy(x => x.ChanelName, new AlphanumComparatorFast()).ToList();
                    }

                    SelectedUnion.ItemsSource = null;
                    SelectedUnion.ItemsSource = Connections.ElementAt(ConnectionList.SelectedIndex).Chanels;
                }
            }
        }

        private void SelectedUnion_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var args = e;
            var key = args.Key;

            if (SelectedUnion.ItemsSource != null && SelectedUnion.SelectedItem != null) 
            {
                var selectedindex = ConnectionList.SelectedIndex;
                var selectedchanelineIndex = SelectedUnion.SelectedIndex;
                var selectedchanel = (Chanel) SelectedUnion.SelectedItem;
                var maxindex = Connections.ElementAt(ConnectionList.SelectedIndex).Chanels.Count - 1;

                if (key == Key.Up && selectedchanelineIndex != 0)
                {
                    Connections.ElementAt(selectedindex).Chanels.Remove(selectedchanel);
                    Connections.ElementAt(selectedindex).Chanels.Insert(selectedchanelineIndex - 1, selectedchanel);

                    SelectedUnion.ItemsSource = null;
                    SelectedUnion.ItemsSource = Connections.ElementAt(ConnectionList.SelectedIndex).Chanels;

                    SelectedUnion.SelectedItem = SelectedUnion.Items[selectedchanelineIndex];
                    SelectedUnion.ScrollIntoView(SelectedUnion.Items[selectedchanelineIndex]);
                    DataGridRow dgrow = (DataGridRow)SelectedUnion.ItemContainerGenerator.ContainerFromItem(SelectedUnion.Items[selectedchanelineIndex]);
                    dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                }
                if (key == Key.Down && selectedchanelineIndex != maxindex)
                {
                    Connections.ElementAt(selectedindex).Chanels.Remove(selectedchanel);
                    Connections.ElementAt(selectedindex).Chanels.Insert(selectedchanelineIndex + 1, selectedchanel);

                    SelectedUnion.ItemsSource = null;
                    SelectedUnion.ItemsSource = Connections.ElementAt(ConnectionList.SelectedIndex).Chanels;

                    SelectedUnion.SelectedItem = SelectedUnion.Items[selectedchanelineIndex];
                    SelectedUnion.ScrollIntoView(SelectedUnion.Items[selectedchanelineIndex]);
                    DataGridRow dgrow = (DataGridRow)SelectedUnion.ItemContainerGenerator.ContainerFromItem(SelectedUnion.Items[selectedchanelineIndex]);
                    dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            _startupWindow.Show();
            _codeclosing = true;
            Close();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (Connections.Count != 0) 
            {
                var chanelConfigurationWindow =
                    new ChanelConfigurationWindow(Connections, this)
                    {
                        WindowStartupLocation = WindowStartupLocation.Manual,
                        Left = Left,
                        Top = Top,
                        Width = ActualWidth,
                        Height = ActualHeight
                    };
                chanelConfigurationWindow.Show();
                Hide();
            }
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_codeclosing)
            {
                Application.Current.Shutdown();
            }
        }

        private void ConnectionList_OnSorting(object sender, DataGridSortingEventArgs e)
        {
            var args = e;
            var column = args.Column.SortMemberPath;

            if (column == "DisplayName")
            {
                ConnectionList.ItemsSource = null;
                Connections = Connections.OrderBy(x => x.DisplayName, new AlphanumComparatorFast()).ToList();
                ConnectionList.ItemsSource = Connections;
            }
            args.Handled = true;
        }
    }

    public enum PcbTypes
    {
        EttNew,
        EttOld,
        F2kNew,
        F2kOld,
        Unknown
    }
}
