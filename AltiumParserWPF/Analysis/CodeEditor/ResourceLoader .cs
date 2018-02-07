﻿using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace AltiumParserWPF.Analysis.CodeEditor
{
    public static class ResourceLoader
    {
        public static IHighlightingDefinition LoadHighlightingDefinition(
            string resourceName)
        {
            var type = typeof(ResourceLoader);
            var fullName = type.Namespace + "." + resourceName;
            using (var stream = type.Assembly.GetManifestResourceStream(fullName))
            using (var reader = new XmlTextReader(stream))
                return HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
    }
}
