using System;
using System.Collections.Generic;
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

        public AltiumParser(string filepath)
        {
            FilePath = filepath;

            var recordCountPattern = new Regex(@"(WEIGHT=(?<Weight>\d+)?)");
            
            var file = new CompoundFile(FilePath);

            var mainStream = file.RootStorage.GetStream("FileHeader");
            var storageStream = file.RootStorage.GetStream("Storage");
            var additionalStream = file.RootStorage.GetStream("Additional");

            var fileHeader = Encoding.Default.GetString(mainStream.GetData());
            var additional = Encoding.Default.GetString(additionalStream.GetData());
            var storage = Encoding.Default.GetString(storageStream.GetData());

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


            ParseRecords();
            BuildComponents();
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
                                Console.WriteLine(@"Found record type Component " + counter);
                                Components.Add(new Component(record, counter));
                                break;

                            case 2:
                                Console.WriteLine(@"Found record type Pin " + counter);
                                Pins.Add(new Pin(record));
                                break;

                            case 3:
                                Console.WriteLine(@"Found record type Symbol " + counter);
                                //TODO: Parse Symbol
                                break;

                            case 4:
                                Console.WriteLine(@"Found record type Label " + counter);
                                //TODO: Parse Label
                                break;

                            case 5:
                                Console.WriteLine(@"Found record type Bezier " + counter);
                                //TODO: Parse Bezier
                                break;

                            case 6:
                                Console.WriteLine(@"Found record type Polyline " + counter);
                                //TODO: Parse Polyline
                                break;

                            case 7:
                                Console.WriteLine(@"Found record type Polygon " + counter);
                                //TODO: Parse Polygon
                                break;

                            case 8:
                                Console.WriteLine(@"Found record type Ellipse " + counter);
                                //TODO: Parse Ellipse
                                break;

                            case 9:
                                Console.WriteLine(@"Found record type Piechart " + counter);
                                //TODO: Parse Piechart
                                break;

                            case 10:
                                Console.WriteLine(@"Found record type Round rectangle " + counter);
                                //TODO: Parse Round rectangle
                                break;

                            case 11:
                                Console.WriteLine(@"Found record type Elliptical arc " + counter);
                                //TODO: Parse Elliptical arc
                                break;

                            case 12:
                                Console.WriteLine(@"Found record type Arc " + counter);
                                //TODO: Parse Arc
                                break;

                            case 13:
                                Console.WriteLine(@"Found record type Line " + counter);
                                //TODO: Parse Line
                                break;

                            case 14:
                                Console.WriteLine(@"Found record type Rectangle " + counter);
                                //TODO: Parse Rectangle
                                break;

                            case 15:
                                Console.WriteLine(@"Found record type Sheet symbol " + counter);
                                SheetSymbols.Add(new SheetSymbol(record, counter));
                                break;

                            case 16:
                                Console.WriteLine(@"Found record type Sheet entry " + counter);
                                SheetEntries.Add(new SheetEntry(record));
                                break;

                            case 17:
                                Console.WriteLine(@"Found record type Power port " + counter);
                                PowerPorts.Add(new PowerPort(record));
                                break;

                            case 18:
                                Console.WriteLine(@"Found record type Port " + counter);
                                Ports.Add(new Port(record));
                                break;

                            case 22:
                                Console.WriteLine(@"Found record type No ERC " + counter);
                                //TODO: Parse No ERC
                                break;

                            case 25:
                                Console.WriteLine(@"Found record type Net label " + counter);
                                Nets.Add(new Net(record));
                                break;

                            case 26:
                                Console.WriteLine(@"Found record type Bus " + counter);
                                //TODO: Parse Bus
                                break;

                            case 27:
                                Console.WriteLine(@"Found record type Wire " + counter);
                                Wires.Add(new Wire(record));
                                break;

                            case 28:
                                Console.WriteLine(@"Found record type Text frame " + counter);
                                //TODO: Parse Text frame
                                break;

                            case 29:
                                Console.WriteLine(@"Found record type Junction " + counter);
                                Junctions.Add(new Junction(record));
                                break;

                            case 30:
                                Console.WriteLine(@"Found record type Image " + counter);
                                //TODO: Parse Image
                                break;

                            case 31:
                                Console.WriteLine(@"Found record type Sheet " + counter);
                                //TODO: Parse Sheet
                                break;

                            case 32:
                                Console.WriteLine(@"Found record type Sheet name" + counter);
                                SheetsNames.Add(new SheetName(record));
                                break;

                            case 33:
                                Console.WriteLine(@"Found record type Sheet file name " + counter);
                                SheetFiles.Add(new SheetFile(record));
                                break;

                            case 34:
                                Console.WriteLine(@"Found record type Designator " + counter);
                                Designators.Add(new Designator(record));
                                //TODO: Parse Designator
                                break;

                            case 37:
                                Console.WriteLine(@"Found record type Bus entry " + counter);
                                //TODO: Parse Bus entry
                                break;

                            case 39:
                                Console.WriteLine(@"Found record type Template " + counter);
                                //TODO: Parse Template
                                break;

                            case 41:
                                Console.WriteLine(@"Found record type Parameter " + counter);
                                Parameters.Add(new Parameter(record));
                                break;

                            case 43:
                                Console.WriteLine(@"Found record type Warning sign " + counter);
                                //TODO: Parse Warning sign
                                break;

                            case 44:
                                Console.WriteLine(@"Found record type Implementation list " + counter);
                                //TODO: Parse Implementation list
                                break;

                            case 45:
                                Console.WriteLine(@"Found record type Implementation " + counter);
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
                component.CombineProperties(Pins, Designators, Parameters);
            }

            foreach (var sheetSymbol in SheetSymbols)
            {
                sheetSymbol.CombineProperties(SheetEntries, SheetsNames, SheetFiles, Parameters);
            }

            foreach (var entry in SheetEntries)
            {
                entry.ChekWires(this);
                entry.CheckNets(this);
            }
        }
    }
}
