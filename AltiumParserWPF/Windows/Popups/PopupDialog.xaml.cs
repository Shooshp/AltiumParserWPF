using System.IO;
using System.Reflection;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace AltiumParserWPF.Windows.Popups
{
    public partial class PopupDialog : ModernDialog
    {
        public PopupDialog(string title, string value)
        {
            InitializeComponent();
            Title = Path.GetFileName(Assembly.GetEntryAssembly().GetName().Name);
            TextBlock.Text = title;
            ResponseTextBox.Text = value;
            this.OkButton.Visibility = Visibility.Visible;
        }

        public string ResponseText
        {
            get => ResponseTextBox.Text;
            set => ResponseTextBox.Text = value;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
