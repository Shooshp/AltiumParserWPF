using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AltiumParserWPF.Analysis;
using AltiumParserWPF.Analysis.CodeEditor;
using AltiumParserWPF.Analysis.Ett;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace AltiumParserWPF.Windows
{

    public partial class F2KOutputWindow
    {
        public List<ConnectionUnion> Unions;
        private Window _parentWindow;
        private bool _codeclosing;
        public string Result;

        public F2KOutputWindow(List<ConnectionUnion> unions, Window parentwindow, string pcbinfo)
        {
            _codeclosing = false;
            _parentWindow = parentwindow;
            Unions = unions.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();
            Result = ConvertToResult(Unions);

            var doc = new TextDocument
            {
                Text = Result,
                FileName = pcbinfo
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

        private string ConvertToResult(List<ConnectionUnion> unions)
        {
            var tempresult = "";

            foreach (var union in unions)
            {
                tempresult += "\t" + union.Name.Split('(')[0].Replace("\\","") + " = " + union.Chanels.First().ChanelName.Replace("CH ","") + "\n"; 
            }

            return tempresult;
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
