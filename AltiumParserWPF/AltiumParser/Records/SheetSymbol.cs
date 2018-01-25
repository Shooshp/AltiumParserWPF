using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.AltiumParser.Records
{
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

        public Designator Designator;
        public List<SheetEntry> SheetEntriesList;

        public SheetSymbol(string record, int id)
        {
            IsConnectable = false;
            Id = id;

            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);
        }

        public void CombineProperties(List<SheetEntry> sheetEntries, List<Designator> designators)
        {
            SheetEntriesList = new List<SheetEntry>();

            foreach (var sheetEntry in sheetEntries)
            {
                if (sheetEntry.OwnerIndex == Id)
                {
                    SheetEntriesList.Add(sheetEntry);
                }
            }

            foreach (var designator in designators)
            {
                if (designator.OwnerIndex == Id)
                {
                    Designator = designator;
                }
            }
        }
    }
}
