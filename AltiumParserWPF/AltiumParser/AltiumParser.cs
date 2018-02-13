using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AltiumParserWPF.AltiumParser.Records;
using OpenMcdf;

namespace AltiumParserWPF.AltiumParser
{
    public class AltiumParser
    {
        public string FilePath;
        public string[] Records;
        public int RecordCount;
        public List<Component> Components;
        public List<Designator> Designators;
        public List<Pin> Pins;
        public List<Wire> Wires;
        public List<Net> Nets;
        public List<Junction> Junctions;
        public List<PowerPort> PowerPorts;
        public List<Port> Ports;
        public List<SheetSymbol> SheetSymbols;
        public List<SheetEntry> SheetEntries;
        public List<SheetName> SheetsNames;
        public List<SheetFile> SheetFiles;
        public List<Parameter> Parameters;

        public List<BomEntry> BuildOfMaterials;

        public List<SubPart> SubParts;

        public AltiumParser(string filepath)
        {
            FilePath = filepath;

            var recordCountPattern = new Regex(@"(WEIGHT=(?<Weight>\d+)?)");
            
            var file = new CompoundFile(FilePath);

            var mainStream = file.RootStorage.GetStream("FileHeader");
            //var storageStream = file.RootStorage.GetStream("Storage");
           // var additionalStream = file.RootStorage.GetStream("Additional");

            var fileHeader = Encoding.Default.GetString(mainStream.GetData());
            //var additional = Encoding.Default.GetString(additionalStream.GetData());
            //var storage = Encoding.Default.GetString(storageStream.GetData());

            file.Close();

            Records = fileHeader.Split(new[] { "RECORD=" }, StringSplitOptions.None);

            var match = recordCountPattern.Match(Records[0]);
            if (match.Success)
            {
                RecordCount = Convert.ToInt32(match.Groups["Weight"].Value);
            }

            Components = new List<Component>();
            Designators = new List<Designator>();
            Pins = new List<Pin>();
            Wires = new List<Wire>();
            Nets = new List<Net>();
            Junctions = new List<Junction>();
            PowerPorts = new List<PowerPort>();
            Ports = new List<Port>();
            SheetSymbols = new List<SheetSymbol>();
            SheetEntries = new List<SheetEntry>();
            SheetsNames = new List<SheetName>();
            SheetFiles = new List<SheetFile>();
            Parameters = new List<Parameter>();

            BuildOfMaterials = new List<BomEntry>();
            SubParts = new List<SubPart>();

            ParseRecords();
            BuildComponents();
            GetBom();
            GetSubParts();
            CombineBoms();
        }

        private void ParseRecords()
        {
            var recordTypePattern = new Regex(@"(?<Type>\d+)\|");

            if (RecordCount != 0)
            {
                var counter = -1;

                foreach (var record in Records)
                {
                    var match = recordTypePattern.Match(record);

                    if (match.Success)
                    {

                        var type = Convert.ToInt32(match.Groups["Type"].Value);

                        switch (type)
                        {
                            case 1:
                                Components.Add(new Component(record, counter));
                                break;

                            case 2:
                                Pins.Add(new Pin(record));
                                break;

                            case 3:
                                //TODO: Parse Symbol
                                break;

                            case 4:
                                //TODO: Parse Label
                                break;

                            case 5:
                                //TODO: Parse Bezier
                                break;

                            case 6:
                                //TODO: Parse Polyline
                                break;

                            case 7:
                                //TODO: Parse Polygon
                                break;

                            case 8:
                                //TODO: Parse Ellipse
                                break;

                            case 9:
                                //TODO: Parse Piechart
                                break;

                            case 10:
                                //TODO: Parse Round rectangle
                                break;

                            case 11:
                                //TODO: Parse Elliptical arc
                                break;

                            case 12:
                                //TODO: Parse Arc
                                break;

                            case 13:
                                //TODO: Parse Line
                                break;

                            case 14:
                                //TODO: Parse Rectangle
                                break;

                            case 15:
                                SheetSymbols.Add(new SheetSymbol(record, counter));
                                break;

                            case 16:
                                SheetEntries.Add(new SheetEntry(record));
                                break;

                            case 17:
                                PowerPorts.Add(new PowerPort(record));
                                break;

                            case 18:
                                Ports.Add(new Port(record));
                                break;

                            case 22:
                                //TODO: Parse No ERC
                                break;

                            case 25:
                                Nets.Add(new Net(record));
                                break;

                            case 26:
                                //TODO: Parse Bus
                                break;

                            case 27:
                                Wires.Add(new Wire(record));
                                break;

                            case 28:
                                //TODO: Parse Text frame
                                break;

                            case 29:
                                Junctions.Add(new Junction(record));
                                break;

                            case 30:
                                //TODO: Parse Image
                                break;

                            case 31:
                                //TODO: Parse Sheet
                                break;

                            case 32:
                                SheetsNames.Add(new SheetName(record));
                                break;

                            case 33:
                                SheetFiles.Add(new SheetFile(record));
                                break;

                            case 34:
                                Designators.Add(new Designator(record));
                                break;

                            case 37:
                                //TODO: Parse Bus entry
                                break;

                            case 39:
                                //TODO: Parse Template
                                break;

                            case 41:
                                Parameters.Add(new Parameter(record));
                                break;

                            case 43:
                                //TODO: Parse Warning sign
                                break;

                            case 44:
                                //TODO: Parse Implementation list
                                break;

                            case 45:
                                //TODO: Parse Implementation
                                break;

                        }
                    }
                    counter++;
                }
            }
        }

        private void BuildComponents()
        {
            foreach (var component in Components)
            {
                component.Init(this);
            }

            foreach (var sheetSymbol in SheetSymbols)
            {
                sheetSymbol.Init(this);
            }
        }

        private void GetBom()
        {
            foreach (var component in Components)
            {
                if (component.CurrentPartId == 1)
                {
                    if (!BuildOfMaterials.Exists(x => x.DeviceType == component.DesignItemId))
                    {
                        BuildOfMaterials.Add(new BomEntry(component.DesignItemId, component.Designator.Text));
                    }
                    else
                    {
                        BuildOfMaterials.Single(x => x.DeviceType == component.DesignItemId).Designators.Add(component.Designator.Text);
                    }
                }
            }
        }

        private void GetSubParts()
        {
            var path = FilePath.Replace(Path.GetFileName(FilePath), "");
            foreach (var sheetSymbol in SheetSymbols)
            {
                var subpartpath = path + sheetSymbol.SheetFile.Text;

                if (subpartpath.Contains(".SchDoc"))
                {
                    if (!SubParts.Exists(x => x.SubParser.FilePath == subpartpath))
                    {
                        SubParts.Add(new SubPart(subpartpath, sheetSymbol.SheetName.Text));
                    }
                    else
                    {
                        SubParts.Single(x => x.SubParser.FilePath == subpartpath).Names.Add(sheetSymbol.SheetName.Text);
                    }
                }
            }
        }

        private void CombineBoms()
        {
            foreach (var subPart in SubParts)
            {
                foreach (var subPartName in subPart.Names)
                {
                    foreach (var bomEntry in subPart.SubParser.BuildOfMaterials)
                    {
                        if (BuildOfMaterials.Exists(x => x.DeviceType == bomEntry.DeviceType))
                        {
                            foreach (var designator in bomEntry.Designators)
                            {
                                BuildOfMaterials.Single(x => x.DeviceType == bomEntry.DeviceType).Designators.Add(subPartName.ToUpper()+"_"+designator);
                            }
                        }
                        else
                        {
                            BuildOfMaterials.Add(new BomEntry(bomEntry.DeviceType, subPartName.ToUpper() + "_" + bomEntry.Designators.ElementAt(0)));

                            foreach (var designator in bomEntry.Designators)
                            {
                                if (!BuildOfMaterials.Single(x => x.DeviceType == bomEntry.DeviceType).Designators.Exists(x=>x.Equals(subPartName.ToUpper() + "_" + designator)))
                                {
                                    BuildOfMaterials.Single(x => x.DeviceType == bomEntry.DeviceType).Designators.Add(subPartName.ToUpper() + "_" + designator);
                                }
                            }
                        }
                    }
                }
            }
        }        
    }
}
