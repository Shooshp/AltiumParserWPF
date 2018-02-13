using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace AltiumParserWPF
{
    public partial class StartupWindow 
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog {Filter = "PCB files (*.SchDoc)|*.SchDoc|All files (*.*)|*.*" };
            if (openFileDialog.ShowDialog() == true)
                FilePathTextBox.Text = openFileDialog.FileName;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            var path = FilePathTextBox.Text;
            if (!string.IsNullOrEmpty(path) && path.Contains(".SchDoc"))
            {
                var folder = path.Replace(Path.GetFileName(path), "");
                if (Directory.Exists(folder))
                {
                    var chanelSelectWindow = new Windows.ChanelSelectWindow(path, this);
                    chanelSelectWindow.Show();
                    Hide();
                }
            }
        }

        private void FilePathTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var path = FilePathTextBox.Text;
            if (!string.IsNullOrEmpty(path) && path.Contains(".SchDoc"))
            {
                var index = path.IndexOf(".SchDoc", StringComparison.Ordinal);
                FilePathTextBox.Text = path.Substring(0, index + 7);
            }
        }
    }
}
