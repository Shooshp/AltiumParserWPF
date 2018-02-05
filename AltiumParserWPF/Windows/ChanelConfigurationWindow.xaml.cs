using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Windows
{
    public partial class ChanelConfigurationWindow : Window
    {
        public List<ConnectionUnion> Unions;
        private Window _parentWindow;
        private bool _codeclosing;
        public List<string> ChanelTypes;

        public ChanelConfigurationWindow(List<ConnectionUnion> unions, Window parentwindow)
        {
            ChanelTypes = new List<string>();
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions;
            
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
            throw new NotImplementedException();
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

            var args = (ComboBox)e.Source;
            var newtype = (ConnectionType)args.SelectedItem;
            var currenttype =  selectedunion.ConnectionType;

            if (newtype != currenttype)
            {
                selectedunion.ConnectionType = newtype;
                ConnectionConfiguration.ItemsSource = null;
                ConnectionConfiguration.ItemsSource = Unions;
            }
        }

        private void OnDirectionChange(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnStateChange(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    } 
}
