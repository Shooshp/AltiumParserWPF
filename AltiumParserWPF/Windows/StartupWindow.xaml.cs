using System.Windows;
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
                var chanelSelectWindow = new Windows.ChanelSelectWindow(path, this);
                chanelSelectWindow.Show();
                Hide();
            }
        }
    }
}
