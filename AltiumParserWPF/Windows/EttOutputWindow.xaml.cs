using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AltiumParserWPF.Analysis.Ett;

namespace AltiumParserWPF.Windows
{
    public partial class EttOutputWindow : Window
    {
        public List<ConnectionUnion> Unions;
        private Window _parentWindow;
        private bool _codeclosing;

        public EttOutputWindow(List<ConnectionUnion> unions, Window parentwindow)
        {
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions;

            var result = new EttPostProcessor(Unions);

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_codeclosing)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
