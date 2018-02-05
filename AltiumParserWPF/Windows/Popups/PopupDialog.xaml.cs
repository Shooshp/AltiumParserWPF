using System.Windows;

namespace AltiumParserWPF
{
    public partial class PopupDialog : Window
    {
        public PopupDialog(string title)
        {
            InitializeComponent();
            Title.Text = title;
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
