using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Windows
{
    public partial class ChanelConfigurationWindow
    {
        public List<ConnectionUnion> Unions;
        private Window _parentWindow;
        private bool _codeclosing;


        public ChanelConfigurationWindow(List<ConnectionUnion> unions, Window parentwindow)
        {
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions;

            foreach (var union in Unions)
            {
                if (union.Chanels.Count == 1)
                {
                    union.ConnectionType = ConnectionType.Global;
                }
            }
            
            InitializeComponent();

            ConnectionConfiguration.ItemsSource = Unions;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show();
            _codeclosing = true;
            Close();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            var error = false;

            foreach (var union in Unions)
            {
                if (union.Direction == Direction.Na || union.InitialState == InitialState.Na)
                {
                    error = true;
                }
            }

            if (!error)
            {
                var ettoutputwindow =
                    new EttOutputWindow(Unions, OutputType.Text, this)
                    {
                        WindowStartupLocation = WindowStartupLocation.Manual,
                        Left = Left,
                        Top = Top,
                        Width = ActualWidth,
                        Height = ActualHeight
                    };
                ettoutputwindow.Show();
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

        private void OnTypeChange(object sender, SelectionChangedEventArgs e)
        {
            var selectedunion = (ConnectionUnion)ConnectionConfiguration.SelectedItem;
            if (selectedunion!=null)
            {
                var args = (ComboBox)e.Source;
                var newtype = (ConnectionType)args.SelectedItem;
                var currenttype = selectedunion.ConnectionType;

                if (newtype != currenttype)
                {
                    selectedunion.ConnectionType = newtype;
                    ConnectionConfiguration.ItemsSource = null;
                    ConnectionConfiguration.ItemsSource = Unions;
                }
            }
        }

        private void OnDirectionChange(object sender, SelectionChangedEventArgs e)
        {
            var selectedunion = (ConnectionUnion)ConnectionConfiguration.SelectedItem;
            if (selectedunion != null)
            {
                var args = (ComboBox)e.Source;
                var newDirection = (Direction)args.SelectedItem;
                var currentdirection = selectedunion.Direction;

                if (newDirection != currentdirection)
                {
                    selectedunion.Direction = newDirection;
                    ConnectionConfiguration.ItemsSource = null;
                    ConnectionConfiguration.ItemsSource = Unions;
                }
            }
        }

        private void OnStateChange(object sender, SelectionChangedEventArgs e)
        {
            var selectedunion = (ConnectionUnion)ConnectionConfiguration.SelectedItem;
            if (selectedunion!=null)
            {
                var args = (ComboBox)e.Source;
                var newInitialState = (InitialState)args.SelectedItem;
                var currentInitialState = selectedunion.InitialState;

                if (newInitialState != currentInitialState)
                {
                    selectedunion.InitialState = newInitialState;
                    ConnectionConfiguration.ItemsSource = null;
                    ConnectionConfiguration.ItemsSource = Unions;
                }
            }
        }
    } 
}
