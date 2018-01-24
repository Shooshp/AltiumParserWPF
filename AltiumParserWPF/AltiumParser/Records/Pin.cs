using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
    public class Pin : Record
    {
        public int OwnerIndex;
        public int OwnerpartId;
        public int FormalType;
        public int Electrical;
        public int PinConglomerate;
        public int PinLength;
        public int PinLength_Frac;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public string Name;
        public int Designator;
        public int PinName_PositionConglomerate;
        public int Name_CustomPosition_Margin;
        public int Name_CustomPosition_Margin_Frac;
        public int Name_CustomFontId;
        public int PinDesignator_PositionConglomerate;
        public int Designator_CustomFontId;

        public Line PinLine;

        public Pin(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            PinLine = new Line(Location_X, Location_X_Frac, Location_Y, Location_X_Frac);
        }
    }
}
