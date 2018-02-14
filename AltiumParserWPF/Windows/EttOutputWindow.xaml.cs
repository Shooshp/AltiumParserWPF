using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AltiumParserWPF.Analysis.CodeEditor;
using AltiumParserWPF.Analysis.Ett;
using AltiumParserWPF.Analysis.Ett.OutputConverters;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace AltiumParserWPF.Windows
{
    public partial class EttOutputWindow 
    {
        public List<ConnectionUnion> Unions;
        private Window _parentWindow;
        private bool _codeclosing;
        public EttOutputTemplate result;

        public EttOutputWindow(List<ConnectionUnion> unions, string type, Window parentwindow)
        {
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions;

            if (type == "Common")
            {
                result = new EttOutputCommon(Unions);
            }

            if (type == "Oleg")
            {
                result = new EttOutputOlegStyle(Unions);
            }


            var doc = new TextDocument
            {
                Text = result.Container,
                FileName = result.Name
            };

            InitializeComponent();
            Title = ApplicationSettings.Name;
            Editor.SyntaxHighlighting = ResourceLoader.LoadHighlightingDefinition("CustomSyntaxDefinitionCpp.xshd");
            Editor.Document = doc;
            Editor.ShowLineNumbers = true;
            var foldingManager = FoldingManager.Install(Editor.TextArea);
            var foldingStrategy = new BraceFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_codeclosing)
            {
                Application.Current.Shutdown();
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show();
            _codeclosing = true;
            Close();
        }
    }
}
