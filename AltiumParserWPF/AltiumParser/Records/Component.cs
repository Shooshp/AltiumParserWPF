﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AltiumParserWPF.AltiumParser.Records
{
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
        public Designator Designator;

        public Component(string record, int id)
        {
            IsConnectable = false;
            Id = id;
            TrimRecord(record);
            ExtractParameters();
            AllocateValues(this);
        }

        public void CombineProperties(List<Pin> pins, List<Designator> designators)
        {
            PinList = new List<Pin>();

            foreach (var pin in pins)
            {
                if (pin.OwnerIndex == Id)
                {
                    PinList.Add(pin);
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
