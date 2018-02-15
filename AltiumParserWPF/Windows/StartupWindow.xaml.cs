using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;

namespace AltiumParserWPF
{
    public partial class StartupWindow
    {
        public List<string> RecentFiles;
        public StartupWindow()
        {
            RecentFiles = GetRecentFiles();

            InitializeComponent();
            Title = ApplicationSettings.Name;
            RecentFilesList.ItemsSource = RecentFiles;
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
                    AddToRecent(path);
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

        private List<string> GetRecentFiles()
        {
            var appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\recent.txt";
            var templist = new List<string>();
            if (File.Exists(appPath)) 
            {
                templist = File.ReadAllLines(appPath).ToList();
            }
            else
            {
                File.Create(appPath);
            }

            return templist;
        }

        private void AddToRecent(string file)
        {
            if (!RecentFiles.Exists(x=>x == file)) 
            {
                RecentFiles.Insert(0, file);
            }
            else
            {
                RecentFiles.RemoveAll(x=>x == file);
                RecentFiles.Insert(0,file);
            }

            if (RecentFiles.Count > 5) 
            {
                RecentFiles.RemoveRange(5, RecentFiles.Count - 5);
            }

            var appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\recent.txt";

            File.WriteAllLines(appPath, RecentFiles);
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            var args = e;
            var sen = sender;
            var link = (Hyperlink)args.OriginalSource;
            FilePathTextBox.Text = link.DataContext.ToString();
            args.Handled = true;
        }
    }
}
