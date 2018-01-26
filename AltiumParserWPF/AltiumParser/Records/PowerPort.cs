namespace AltiumParserWPF.AltiumParser.Records
{
    public class PowerPort : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int Style;
        public string ShowNetName;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int Orientation;
        public int Color;
        public int FontId;
        public string Text;
        public string UniqueId;


        public PowerPort(string record)
        {
            IsConnectable = true;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            Connection = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
        }
    }
}
