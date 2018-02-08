using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltiumParserWPF.Analysis.Ett
{
    public class OldEttBoard : PCB
    {
        public int DutCount;
        public List<DUT> Duts;

        private List<Parity> Mapping;

        public OldEttBoard(AltiumParser.AltiumParser parser)
        {
            Board = parser;
            DutCount = GetDutNumber();
            Chanels = new List<Chanel>();
            MakeMap();
            GetActiveChanels();
        }

        private int GetDutNumber()
        {
            Duts = new List<DUT>();

            var temp = new List<DUT>();
            foreach (var sheet in Board.SheetSymbols)
            {
                if (sheet.SheetName.Text.ToUpper().Contains("DUT"))
                {
                    temp.Add(new DUT(sheet));
                }
            }
            Duts = temp.OrderBy(x => x.Name, new AlphanumComparatorFast()).ToList();

            return Duts.Count;
        }

        private void GetActiveChanels()
        {
            foreach (var component in Board.Components)
            {
                if (component.DesignItemId.Contains("CONN_DIN_48") || component.DesignItemId.Contains("CONN_DIN_96"))
                {
                    foreach (var pin in component.PinList)
                    {
                        var tempname = "";
                        if (component.DesignItemId.Contains("CONN_DIN_48"))
                        {
                            tempname = "J2_" + pin.Name;
                        }
                        if (component.DesignItemId.Contains("CONN_DIN_96"))
                        {
                            tempname = "J1_" + pin.Name;
                        }

                        if (Mapping.Exists(x => x.Socket == tempname))
                        {
                            if (pin.ConnectedNets.Count != 0)
                            {
                                Chanels.Add(new Chanel(tempname, pin.ConnectedNets.ElementAt(0).Text.ToUpper()));
                            }
                            else
                            {
                                if (pin.ConnectedPorts.Count != 0)
                                {
                                    Chanels.Add(new Chanel(tempname, pin.ConnectedPorts.ElementAt(0).Name.ToUpper()));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MakeMap()
        {
            Mapping = new List<Parity>();

            Mapping.Add(new Parity("J1_67", "CH 1"));
            Mapping.Add(new Parity("J1_36", "CH 2"));
            Mapping.Add(new Parity("J1_71", "CH 3"));
            Mapping.Add(new Parity("J1_72", "CH 4"));
            Mapping.Add(new Parity("J1_6", "CH 5"));
            Mapping.Add(new Parity("J1_7", "CH 6"));
            Mapping.Add(new Parity("J1_8", "CH 7"));
            Mapping.Add(new Parity("J1_9", "CH 8"));
            Mapping.Add(new Parity("J1_55", "CH 9"));
            Mapping.Add(new Parity("J1_88", "CH 10"));
            Mapping.Add(new Parity("J1_89", "CH 11"));
            Mapping.Add(new Parity("J1_59", "CH 12"));
            Mapping.Add(new Parity("J1_93", "CH 13"));
            Mapping.Add(new Parity("J1_95", "CH 14"));
            Mapping.Add(new Parity("J1_34", "CH 15"));
            Mapping.Add(new Parity("J1_98", "CH 16"));
            Mapping.Add(new Parity("J1_24", "CH 17"));
            Mapping.Add(new Parity("J1_57", "CH 18"));
            Mapping.Add(new Parity("J1_90", "CH 19"));
            Mapping.Add(new Parity("J1_91", "CH 20"));
            Mapping.Add(new Parity("J1_29", "CH 21"));
            Mapping.Add(new Parity("J1_63", "CH 22"));
            Mapping.Add(new Parity("J1_33", "CH 23"));
            Mapping.Add(new Parity("J1_97", "CH 24"));
            Mapping.Add(new Parity("J1_44", "CH 25"));
            Mapping.Add(new Parity("J1_13", "CH 26"));
            Mapping.Add(new Parity("J1_48", "CH 27"));
            Mapping.Add(new Parity("J1_16", "CH 28"));
            Mapping.Add(new Parity("J1_51", "CH 29"));
            Mapping.Add(new Parity("J1_19", "CH 30"));
            Mapping.Add(new Parity("J1_86", "CH 31"));
            Mapping.Add(new Parity("J1_22", "CH 32"));
            Mapping.Add(new Parity("J1_68", "CH 33"));
            Mapping.Add(new Parity("J1_70", "CH 34"));
            Mapping.Add(new Parity("J1_4", "CH 35"));
            Mapping.Add(new Parity("J1_5", "CH 36"));
            Mapping.Add(new Parity("J1_73", "CH 37"));
            Mapping.Add(new Parity("J1_74", "CH 38"));
            Mapping.Add(new Parity("J1_75", "CH 39"));
            Mapping.Add(new Parity("J1_78", "CH 40"));
            Mapping.Add(new Parity("J1_77", "CH 41"));
            Mapping.Add(new Parity("J1_46", "CH 42"));
            Mapping.Add(new Parity("J1_81", "CH 43"));
            Mapping.Add(new Parity("J1_49", "CH 44"));
            Mapping.Add(new Parity("J1_83", "CH 45"));
            Mapping.Add(new Parity("J1_84", "CH 46"));
            Mapping.Add(new Parity("J1_53", "CH 47"));
            Mapping.Add(new Parity("J1_54", "CH 48"));
            Mapping.Add(new Parity("J1_87", "CH 49"));
            Mapping.Add(new Parity("J1_25", "CH 50"));
            Mapping.Add(new Parity("J1_58", "CH 51"));
            Mapping.Add(new Parity("J1_28", "CH 52"));
            Mapping.Add(new Parity("J1_60", "CH 53"));
            Mapping.Add(new Parity("J1_31", "CH 54"));
            Mapping.Add(new Parity("J1_64", "CH 55"));
            Mapping.Add(new Parity("J1_96", "CH 56"));
            Mapping.Add(new Parity("J1_69", "CH 57"));
            Mapping.Add(new Parity("J1_37", "CH 58"));
            Mapping.Add(new Parity("J1_38", "CH 59"));
            Mapping.Add(new Parity("J1_39", "CH 60"));
            Mapping.Add(new Parity("J1_40", "CH 61"));
            Mapping.Add(new Parity("J1_41", "CH 62"));
            Mapping.Add(new Parity("J1_42", "CH 63"));
            Mapping.Add(new Parity("J1_14", "CH 64"));
            Mapping.Add(new Parity("J1_76", "CH 65"));
            Mapping.Add(new Parity("J1_45", "CH 66"));
            Mapping.Add(new Parity("J1_80", "CH 67"));
            Mapping.Add(new Parity("J1_15", "CH 68"));
            Mapping.Add(new Parity("J1_50", "CH 69"));
            Mapping.Add(new Parity("J1_18", "CH 70"));
            Mapping.Add(new Parity("J1_52", "CH 71"));
            Mapping.Add(new Parity("J1_23", "CH 72"));
            Mapping.Add(new Parity("J1_43", "CH 73"));
            Mapping.Add(new Parity("J1_79", "CH 74"));
            Mapping.Add(new Parity("J1_47", "CH 75"));
            Mapping.Add(new Parity("J1_82", "CH 76"));
            Mapping.Add(new Parity("J1_17", "CH 77"));
            Mapping.Add(new Parity("J1_85", "CH 78"));
            Mapping.Add(new Parity("J1_20", "CH 79"));
            Mapping.Add(new Parity("J1_21", "CH 80"));
            Mapping.Add(new Parity("J1_56", "CH 81"));
            Mapping.Add(new Parity("J1_26", "CH 82"));
            Mapping.Add(new Parity("J1_27", "CH 83"));
            Mapping.Add(new Parity("J1_92", "CH 84"));
            Mapping.Add(new Parity("J1_61", "CH 85"));
            Mapping.Add(new Parity("J1_32", "CH 86"));
            Mapping.Add(new Parity("J1_65", "CH 87"));
            Mapping.Add(new Parity("J1_66", "CH 88"));
            Mapping.Add(new Parity("J2_35", "CH 89"));
            Mapping.Add(new Parity("J2_36", "CH 90"));
            Mapping.Add(new Parity("J2_22", "CH 91"));
            Mapping.Add(new Parity("J2_23", "CH 92"));
            Mapping.Add(new Parity("J2_25", "CH 93"));
            Mapping.Add(new Parity("J2_42", "CH 94"));
            Mapping.Add(new Parity("J2_28", "CH 95"));
            Mapping.Add(new Parity("J2_45", "CH 96"));
            Mapping.Add(new Parity("J2_19", "CH 97"));
            Mapping.Add(new Parity("J2_37", "CH 98"));
            Mapping.Add(new Parity("J2_38", "CH 99"));
            Mapping.Add(new Parity("J2_24", "CH 100"));
            Mapping.Add(new Parity("J2_41", "CH 101"));
            Mapping.Add(new Parity("J2_27", "CH 102"));
            Mapping.Add(new Parity("J2_44", "CH 103"));
            Mapping.Add(new Parity("J2_30", "CH 104"));
            Mapping.Add(new Parity("J2_20", "CH 105"));
            Mapping.Add(new Parity("J2_21", "CH 106"));
            Mapping.Add(new Parity("J2_39", "CH 107"));
            Mapping.Add(new Parity("J2_40", "CH 108"));
            Mapping.Add(new Parity("J2_26", "CH 109"));
            Mapping.Add(new Parity("J2_43", "CH 110"));
            Mapping.Add(new Parity("J2_29", "CH 111"));
            Mapping.Add(new Parity("J2_46", "CH 112"));
        }
    }

    class Parity
    {
        public string Socket;
        public string Chanel;

        internal Parity(string socket, string chanel)
        {
            Socket = socket;
            Chanel = chanel;
        }
    }

    
}
