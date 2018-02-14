using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
                    RecentFiles.Add(path);
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
                AddToRecent(path);
                var index = path.IndexOf(".SchDoc", StringComparison.Ordinal);
                FilePathTextBox.Text = path.Substring(0, index + 7);
            }
        }

        private List<string> GetRecentFiles()
        {
            var appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\recent.txt";
            var templist = new List<string>();
            using (var filestream = File.Open(appPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(appPath))
                {
                    var templine = "";

                    while ((templine = reader.ReadLine()) != null) 
                    {
                        templist.Add(templine);
                    }
                }
                filestream.Close();
            }

            return templist;
        }

        private void AddToRecent(string file)
        {
            RecentFiles.Add(file);

            if (RecentFiles.Count > 5) 
            {
                RecentFiles.RemoveRange(5, RecentFiles.Count);
            }

            var appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\recent.txt";

            using (var filestream = File.Open(appPath, FileMode.Open, FileAccess.Write))
            {
                using (var writer = new StreamWriter(appPath))
                {
                    foreach (var fileline in RecentFiles)
                    {
                        writer.WriteLine(fileline);
                    }
                }
                filestream.Close();
            }
        }
    }
}
