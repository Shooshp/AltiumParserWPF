using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett.OutputConverters
{
    public class EttOutputOlegStyle : EttOutputTemplate
    {
        public EttOutputOlegStyle(List<ConnectionUnion> unions)
        {
            Unions = unions;
            Name = "OlegStyleOOP.cpp";
            var result = new List<string>();

            foreach (var union in Unions)
            {
                result.Add(OlegObjectCreator(union));
                result.Add("");
            }

            CombineList(result);
        }

        private string OlegObjectCreator(ConnectionUnion union)
        {
            string templine = null;

            switch (union.ConnectionType)
            {
                case ConnectionType.Global:

                    templine = "pin " + union.Name + "(" + union.Chanels.ElementAt(0).ChanelName.Replace("CH","").Replace(" ", "") + ",";

                    if (union.Direction == Direction.Out)
                    {
                        templine += "OUT, ";
                        if (union.InitialState == InitialState.Low)
                        {
                            templine += "LO";
                        }
                        else
                        {
                            templine += "HI";
                        }
                    }
                    else
                    {
                        templine += "INP, LO";
                    }

                    templine += ");";

                    break;

                case ConnectionType.Array:
                    templine = "pins " + union.Name + "(";

                    if (union.Direction == Direction.Out)
                    {
                        templine += "OUT, ";
                        if (union.InitialState == InitialState.Low)
                        {
                            templine += "LO, ";
                        }
                        else
                        {
                            templine += "HI, ";
                        }
                    }
                    else
                    {
                        templine += "INP, LO, ";
                    }

                    templine += union.Chanels.Count + ",\n\t";

                    var counter = 1;
                    foreach (var chanel in union.Chanels)
                    {
                        templine += chanel.ChanelName.Replace("CH ", "");

                        if (counter != union.Chanels.Count)
                        {
                            templine += ", ";
                            if (counter % 10 == 0)
                            {
                                templine += "\n \t";
                            }
                        }
                        else
                        {
                            templine += "\n);\n";
                        }

                        counter++;
                    }

                    break;

                case ConnectionType.Bus:
                    templine = "port " + union.Name + "(";

                    if (union.Direction == Direction.Out)
                    {
                        templine += "OUT, ";
                        if (union.InitialState == InitialState.Low)
                        {
                            templine += "LO, ";
                        }
                        else
                        {
                            templine += "HI, ";
                        }
                    }
                    else
                    {
                        templine += "INP, LO, ";
                    }

                    templine += union.Chanels.Count + ",\n\t";

                    var counter2 = 1;
                    foreach (var chanel in union.Chanels)
                    {
                        templine += chanel.ChanelName.Replace("CH ", "");

                        if (counter2 != union.Chanels.Count)
                        {
                            templine += ", ";
                            if (counter2 % 10 == 0)
                            {
                                templine += "\n \t";
                            }
                        }
                        else
                        {
                            templine += "\n);\n";
                        }

                        counter2++;
                    }
                    break;
            }

            return templine;
        }
    }
}
