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

        public EttOutputWindow(List<ConnectionUnion> unions, Window parentwindow)
        {
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions;

            //var result = new EttOutputCommon(Unions);
            var result = new EttOutputOlegStyle(Unions);
            var doc = new TextDocument();


            doc.Text = result.Container;
            doc.FileName = result.Name;
            InitializeComponent();

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
            throw new System.NotImplementedException();
        }
    }
}
