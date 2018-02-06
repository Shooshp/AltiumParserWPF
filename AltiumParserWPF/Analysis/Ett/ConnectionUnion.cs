using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett
{
    [DebuggerDisplay("Name:{Name}, Count:{Chanels.Count}")]
    public class ConnectionUnion
    {
        public string Name;
        public List<Chanel> Chanels { get; set; }
        private ConnectionType type;
        private Direction direction;
        private InitialState initialstate;

        public ConnectionType ConnectionType
        {
            get { return type; }
            set { type = value; }
        }

        public Direction Direction
        {
            get { return direction; }
            set
            {
                direction = value;

                if (direction == Direction.In)
                {
                    initialstate = InitialState.HiZ;
                }
                else
                {
                    initialstate = InitialState.Na;
                }
            }
        }

        public InitialState InitialState
        {
            get { return initialstate; }
            set
            {
                var state = value;
                if (direction != Direction.Na)
                {
                    if (direction == Direction.In)
                    {
                        initialstate = InitialState.HiZ;
                    }
                    else
                    {
                        if (direction == Direction.Bidir)
                        {
                            initialstate = state;
                        }
                        else
                        {
                            if (state == InitialState.HiZ)
                            {
                                initialstate = InitialState.Na;
                            }
                            else
                            {
                                initialstate = state;
                            }
                        }
                    }
                }
                else
                {
                    initialstate = InitialState.Na;
                }
            }
        }

        public string DisplayName
        {
            get
            {
                if (Chanels.Count == 1)
                {
                    return Name;
                }
                else
                {
                    var line = Name + "[" + Chanels.Count + "]";
                    return line;
                }
            }
        }

        public ConnectionUnion(string name)
        {
            Name = name;
            Chanels = new List<Chanel>();
            direction = Direction.Na;
            initialstate = InitialState.Na;
        }

        public void Add(Chanel chanel)
        {
            Chanels.Add(chanel);
        }

        public override string ToString()
        {
            var line = "";

            if (Chanels.Count > 1)
            {
                line += "const u8 " + Name.ToUpper() + "[" + Chanels.Count + "] = \n{\n \t";
                var counter = 1;
                foreach (var chanel in Chanels)
                {
                    line += chanel.ChanelName.Replace("CH ", "");
                    if (counter != Chanels.Count)
                    {
                        line += ", ";
                        if (counter % 10 == 0)
                        {
                            line += "\n \t";
                        }
                    }
                    else
                    {
                        line += "\n};\n";
                    }

                    counter++;
                }
            }
            else
            {
                line = "typedef Ch" + Chanels.ElementAt(0).ChanelName.Replace("CH ", "") + " " + Name.ToUpper() + ";\n";
            }

            return line;
        }

        public string GetDataType()
        {
            var vartype = "";

            if (Chanels.Count < 9)
            {
                vartype = "u8";
            }
            else
            {
                if (Chanels.Count < 17)
                {
                    vartype = "u16";
                }
                else
                {
                    vartype = "u32";
                }
            }

            return vartype;
        }
    }

    public enum ConnectionType
    {
        [Description("Array")]
        Array,
        [Description("Bus")]
        Bus,
        [Description("Global")]
        Global
    }

    public enum Direction
    {
        [Description("In")]
        In,
        [Description("Out")]
        Out,
        [Description("Bidir")]
        Bidir,
        [Description("Unknown")]
        Na
    }

    public enum InitialState
    {
        [Description("Low")]
        Low,
        [Description("High")]
        High,
        [Description("HiZ")]
        HiZ,
        [Description("Unknown")]
        Na
    }
}
