using System;
using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.AltiumParser.Records;

namespace AltiumParserWPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var filename = @"F:\test\test.SchDoc";

            var small = "DIN41612R.РОЗ.УГЛ.48";
            var big = "DIN41612R.РОЗ.УГЛ.96";

            var parser = new AltiumParser.AltiumParser(filename);

            foreach (var sheetEntry in parser.SheetEntries)
            {
                Console.WriteLine(sheetEntry.Name + " X " + sheetEntry.Connection.X + " Y " + sheetEntry.Connection.Y);
            }
        }
    }
}
