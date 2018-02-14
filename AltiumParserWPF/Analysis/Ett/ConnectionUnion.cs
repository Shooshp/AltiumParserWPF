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
        private ConnectionType _type;
        private Direction _direction;
        private InitialState _initialstate;

        public ConnectionType ConnectionType
        {
            get { return _type; }
            set
            {
                if (Chanels.Count == 1)
                {
                    _type = ConnectionType.Global;
                }
                else
                {
                    _type = value;
                }
            }
        }

        public Direction Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;

                if (_direction == Direction.In)
                {
                    _initialstate = InitialState.HiZ;
                }
                else
                {
                    _initialstate = InitialState.Na;
                }
            }
        }

        public InitialState InitialState
        {
            get { return _initialstate; }
            set
            {
                var state = value;
                if (_direction != Direction.Na)
                {
                    if (_direction == Direction.In)
                    {
                        _initialstate = InitialState.HiZ;
                    }
                    else
                    {
                        if (_direction == Direction.Bidir)
                        {
                            _initialstate = state;
                        }
                        else
                        {
                            if (state == InitialState.HiZ)
                            {
                                _initialstate = InitialState.Na;
                            }
                            else
                            {
                                _initialstate = state;
                            }
                        }
                    }
                }
                else
                {
                    _initialstate = InitialState.Na;
                }
            }
        }

        public string DisplayName
        {
            get
            {
                if (Chanels.Count == 1)
                {
                    return Name.Replace("\\","");
                }
                else
                {
                    var line = Name.Replace("\\", "") + "[" + Chanels.Count + "]";
                    return line;
                }
            }
            set
            {
                if (value.Contains("["))
                {
                    Name = value.Split('[')[0];
                }
                else
                {
                    Name = value;
                }
            }
        }

        public ConnectionUnion(string name)
        {
            Name = name;
            Chanels = new List<Chanel>();
            _direction = Direction.Na;
            _initialstate = InitialState.Na;
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
            string vartype;

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
