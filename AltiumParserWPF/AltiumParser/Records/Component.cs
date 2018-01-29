using System.Collections.Generic;
using System.Diagnostics;

namespace AltiumParserWPF.AltiumParser.Records
{
    [DebuggerDisplay("Name:{Libreference}, Part:{CurrentPartId},  {Designator.Text} ")]
    public class Component : Record
    {
        public string Libreference;
        public string ComponentDescription;
        public int PartCount;
        public int DisplayModeCount;
        public int IndexInSheet;
        public int OwnerpartId;
        public int Location_X;
        public int Location_X_Frac;
        public int Location_Y;
        public int Location_Y_Frac;
        public int CurrentPartId;
        public string LibraryPath;
        public string SourceLibraryName;
        public string DataBaseTableName;
        public string TargetFileName;
        public string UniqueId;
        public int AreaColor;
        public int Color;
        public string PartIdLocked;
        public string DesignItemId;
        public int AllPinCount;

        public List<Pin> PinList;
        public List<Parameter> AdditionalParameters;
        public Designator Designator;

        public Component(string record, int id)
        {
            IsConnectable = false;
            Id = id;
            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);

            Connection = new Dot(Location_X, Location_X_Frac, Location_Y, Location_Y_Frac);
        }

        public void Init(AltiumParser parser)
        {
            PinList = new List<Pin>();
            AdditionalParameters = new List<Parameter>();

            foreach (var pin in parser.Pins)
            {
                if (pin.OwnerIndex == Id && pin.OwnerpartId == CurrentPartId)
                {
                    PinList.Add(pin);
                }
            }


            foreach (var designator in parser.Designators)
            {
                if (designator.OwnerIndex == Id)
                {
                    Designator = designator;
                }
            }

            foreach (var parameter in parser.Parameters)
            {
                if (parameter.OwnerIndex == Id)
                {
                    AdditionalParameters.Add(parameter);
                }
            }

            foreach (var pin in PinList)
            {
                pin.CheckWires(parser);
                pin.CheckNets(parser);
                pin.CheckPorts(parser);
                pin.CheckPowerPorts(parser); 
            }
        }
    }
}
