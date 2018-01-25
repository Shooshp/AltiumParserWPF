using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class SheetEntry : Record
    {
        public int OwnerIndex;
        public int OwnerpartId;
        public int DistanceFromTop;
        public int DistanceFromTop_Frac1;
        public int Color;
        public int AreaColor;
        public int TextFontId;
        public string TextStyle;
        public string Name;
        public int Style;
        public string ArrowKind;

        public SheetEntry(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);
        }
    }
}
