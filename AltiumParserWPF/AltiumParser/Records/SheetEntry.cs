using System.Globalization;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class SheetEntry : Record
    {
        public int OwnerIndex;
        public int OwnerpartId;
        public int Side;
        public int DistanceFromTop;
        public int DistanceFromTop_Frac1;
        public int Color;
        public int AreaColor;
        public int TextFontId;
        public string TextStyle;
        public string Name;
        public int Style;
        public string ArrowKind;
        public float CombinedDistanceFromTop;

        public SheetEntry(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            var temp = DistanceFromTop + "." + DistanceFromTop_Frac1;

            CombinedDistanceFromTop = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat)*10;
        }
    }
}
