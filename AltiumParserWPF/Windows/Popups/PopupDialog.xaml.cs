using System.Windows;

namespace AltiumParserWPF
{
    public partial class PopupDialog : Window
    {
        public PopupDialog(string title, string value)
        {
            InitializeComponent();
            Title.Text = title;
            ResponseTextBox.Text = value;
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
