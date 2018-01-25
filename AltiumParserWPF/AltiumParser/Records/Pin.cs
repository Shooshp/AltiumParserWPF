using System;
using System.Collections.Generic;
using System.Globalization;
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

        public float Lenght;

        public Dot PinStartDot;

        public Pin(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            var templenght = PinLength + "." + PinLength_Frac;
            Lenght = float.Parse(templenght, CultureInfo.InvariantCulture.NumberFormat);

            PinStartDot = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
            Connection = GetEnd();
        }

        private Dot GetEnd()
        {
            var rotation = PinConglomerate & 0x3;
            var tempx = PinStartDot.X;
            var tempy = PinStartDot.Y;

            switch (rotation)
            {
                case 0:
                    // right
                    tempx += Lenght;
                break;

                case 1: 
                    // Up
                    tempy += Lenght;
                break;

                case 2:
                    //Left
                    tempx -= Lenght;
                break;

                case 3:
                    // Down
                    tempy -= Lenght;
                break;
            }

            var dot = new Dot(tempx, tempy);

            return dot;
        }
    }
}
