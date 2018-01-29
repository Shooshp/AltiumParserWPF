using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("{SheetName.Text}")]
    public class SheetSymbol : Record
    {
        public int IndexInSheet;
        public int OwnerpartId;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int XSize;
        public int XSize_Frac;
        public int YSize;
        public int YSize_Frac;
        public int LineWidth;
        public int AreaColor;
        public string IsSolid;
        public string UniqueId;
        public string SymbolType;

        public SheetName SheetName;
        public SheetFile SheetFile;
        public List<SheetEntry> SheetEntriesList;
        public List<Parameter> AdditionalParameters;
        public Dot TopLeftConer;
        public Dot TopRightConer;
        public float XOffset;

        public SheetSymbol(string record, int id)
        {
            IsConnectable = false;
            Id = id;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            var temp = XSize + "." + XSize_Frac;

            XOffset = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);
            TopLeftConer = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);

            TopRightConer = new Dot(TopLeftConer.X + XOffset, TopLeftConer.Y);
        }

        public void Init(AltiumParser parser)
        {
            SheetEntriesList = new List<SheetEntry>();
            AdditionalParameters = new List<Parameter>();

            foreach (var sheetEntry in parser.SheetEntries)
            {
                if (sheetEntry.OwnerIndex == Id)
                {
                    SheetEntriesList.Add(sheetEntry);
                }
            }

            foreach (var sheetName in parser.SheetsNames)
            {
                if (sheetName.OwnerIndex == Id)
                {
                    SheetName = sheetName;
                }
            }

            foreach (var parameter in parser.Parameters)
            {
                if (parameter.OwnerIndex == Id)
                {
                    AdditionalParameters.Add(parameter);
                }
            }

            foreach (var sheetFile in parser.SheetFiles)
            {
                if (sheetFile.OwnerIndex == Id)
                {
                    SheetFile = sheetFile;
                }
            }

            foreach (var sheetEntry in SheetEntriesList)
            {
                if (sheetEntry.Side == 0)
                {
                    sheetEntry.Connection = new Dot(TopLeftConer.X, TopLeftConer.Y - sheetEntry.CombinedDistanceFromTop);
                }
                else
                {
                    sheetEntry.Connection = new Dot(TopRightConer.X, TopRightConer.Y - sheetEntry.CombinedDistanceFromTop);
                }
            }

            foreach (var sheetEntry in SheetEntriesList)
            {
                sheetEntry.CheckWires(parser);
                sheetEntry.CheckNets(parser);
                sheetEntry.CheckPorts(parser);
                sheetEntry.CheckPowerPorts(parser);
            }
        }
    }
}
