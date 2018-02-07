using System.Collections.Generic;
using System.Linq;
using AltiumParserWPF.Analysis.Ett.CodeMaker.Templates;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class EttFunctions
    {
        private readonly ConnectionUnion _union;

        public readonly List<string> InitializationCode;
        public readonly List<string> WorkingRutines;

        private CodeTemplateFunction _getBusFunction;
        private CodeTemplateFunction _setBusFunction;

        private CodeTemplateFunction _getPinFromArrayFunction;
        private CodeTemplateFunction _setPinInArrayFunction;
        private CodeTemplateFunction _clearPinArrayFunction;

        private string _singleChanelInit;


        public EttFunctions(ConnectionUnion union)
        {
            _union = union;

            InitializationCode = new List<string>();
            WorkingRutines = new List<string>();

            switch (_union.ConnectionType)
            {
                case ConnectionType.Bus:

                    switch (_union.Direction)
                    {
                        case Direction.In:
                            CreateGetBusFunction();
                            WorkingRutines.AddRange(_getBusFunction.Code);
                            break;

                        case Direction.Out:
                            CreateSetBusFunction();
                            WorkingRutines.AddRange(_setBusFunction.Code);
                            break;

                        case Direction.Bidir:
                            CreateGetBusFunction();
                            CreateSetBusFunction();
                            WorkingRutines.AddRange(_getBusFunction.Code);
                            WorkingRutines.AddRange(_setBusFunction.Code);
                            break;
                    }

                    InitBus();
                    break;

                case ConnectionType.Array:
                    
                    switch (_union.Direction)
                    {
                        case Direction.In:
                            CreateGetPinFromArrayFunction();
                            WorkingRutines.AddRange(_getPinFromArrayFunction.Code);
                            break;

                        case Direction.Out:
                            CreateSetPinInArrayFunction();
                            CreateClearPinArrayFunction();
                            WorkingRutines.AddRange(_setPinInArrayFunction.Code);
                            WorkingRutines.AddRange(_clearPinArrayFunction.Code);
                            break;

                        case Direction.Bidir:
                            CreateGetPinFromArrayFunction();
                            CreateSetPinInArrayFunction();
                            CreateClearPinArrayFunction();
                            WorkingRutines.AddRange(_getPinFromArrayFunction.Code);
                            WorkingRutines.AddRange(_setPinInArrayFunction.Code);
                            WorkingRutines.AddRange(_clearPinArrayFunction.Code);
                            break;
                    }
                    InitArray();
                    break;

                case ConnectionType.Global:
                    CreateSingleChanel();
                    InitSingleChanel();
                    WorkingRutines.Add(_singleChanelInit);
                    break;
            }
        }

        private void CreateGetBusFunction()
        {
            var tempcodeget = new List<string> {_union.GetDataType() + " LOCAL_VALUE=0;"};

            var functionbody = new List<string>
            {
                "ChannelControl::dir(" + _union.Name + "[DATA_BIT], 1);",
                "LOCAL_VALUE = LOCAL_VALUE | (ChannelControl::read(" + _union.Name + "[DATA_BIT]) << DATA_BIT);"
            };

            tempcodeget.AddRange(new CodeTemplateFor("DATA_BIT", _union.Chanels.Count.ToString(), functionbody).Code);
            tempcodeget.Add("return LOCAL_VALUE;");

            var getname = _union.GetDataType() + " GET_BUS_" + _union.Name + "(void)";

            _getBusFunction = new CodeTemplateFunction(getname, tempcodeget);
        }

        private void CreateSetBusFunction()
        {
            var tempcodeset = new List<string>();

            var functionbody = new List<string>
            {
                "ChannelControl::dir(" + _union.Name + "[DATA_BIT], 0);",
                "ChannelControl::write(" + _union.Name + "[DATA_BIT],((DAT>>DATA_BIT)&0x1));"
            };

            tempcodeset.AddRange(new CodeTemplateFor("DATA_BIT", _union.Chanels.Count.ToString(), functionbody).Code);

            var setname = "void SET_BUS_" + _union.Name + "(" + _union.GetDataType() + " DAT)";
            _setBusFunction = new CodeTemplateFunction(setname, tempcodeset);
        }

        private void CreateSetPinInArrayFunction()
        {
            var tempcodeset = new List<string>();
            var setpinname = "void SET_PIN_" + _union.Name + "(u8 BIT)";
            tempcodeset.Add("ChannelControl::dir(" + _union.Name + "[BIT], 0);");
            tempcodeset.Add("ChannelControl::write(" + _union.Name + "[BIT],1);");

            _setPinInArrayFunction = new CodeTemplateFunction(setpinname, tempcodeset);
        }

        private void CreateClearPinArrayFunction()
        {
            var tempcodeclear = new List<string>();
            var clearpinname = "void CLEAR_PIN_" + _union.Name + "(u8 BIT)";
            tempcodeclear.Add("ChannelControl::dir(" + _union.Name + "[BIT], 0);");
            tempcodeclear.Add("ChannelControl::write(" + _union.Name + "[BIT],0);");

            _clearPinArrayFunction = new CodeTemplateFunction(clearpinname, tempcodeclear);
        }

        private void CreateGetPinFromArrayFunction()
        {
            var tempcodeget = new List<string>();
            var getpinname = "u8 GET_PIN_" + _union.Name + "(u8 BIT)";
            tempcodeget.Add("ChannelControl::dir(" + _union.Name + "[BIT], 1);");
            tempcodeget.Add("return ChannelControl::read(" + _union.Name + "[BIT]);");

            _getPinFromArrayFunction = new CodeTemplateFunction(getpinname, tempcodeget);
        }

        private void CreateSingleChanel()
        {
            var chanel = _union.Chanels.ElementAt(0);
            var tempname = "";

            if (chanel.ChanelName.Contains("CH"))
            {
                tempname = "Ch" + chanel.ChanelName.Replace("CH", "").Replace(" ", "");
            }

            if (chanel.ChanelName.Contains("J1_") || chanel.ChanelName.Contains("J2_"))
            {
                tempname = "C" + chanel.ChanelName.Replace(" ", "");
            }

            _singleChanelInit = "typedef " + tempname + " " + chanel.ConnectionName.ToUpper() + ";";
        }

        private void InitSingleChanel()
        {
            var chanel = _union.Chanels.ElementAt(0);

            var initcode = new List<string>();

            if (_union.InitialState == InitialState.HiZ)
            {
                initcode.Add(chanel.ConnectionName.ToUpper() + "::SetDirRead();");
                initcode.Add(chanel.ConnectionName.ToUpper() + "::Read();");
            }
            else
            {
                initcode.Add(chanel.ConnectionName.ToUpper() + "::SetDirWrite();");
                if (_union.InitialState == InitialState.High)
                {
                    initcode.Add(chanel.ConnectionName.ToUpper() + "::Set();");
                }
                else
                {
                    initcode.Add(chanel.ConnectionName.ToUpper() + "::Clear();");
                }
            }
            InitializationCode.AddRange(initcode);
        }

        private void InitBus()
        {
            string initialcode;

            if (_union.InitialState == InitialState.HiZ)
            {
                initialcode = "GET_BUS_" + _union.Name + "();";
            }
            else
            {
                if (_union.InitialState == InitialState.Low)
                {
                    initialcode = "SET_BUS_" + _union.Name + "(0x0);";
                }
                else
                {
                    var tempvalue = "";

                    switch (_union.GetDataType())
                    {
                        case "u8":
                            tempvalue = "0xFF";
                            break;
                        case "u16":
                            tempvalue = "0xFFFF";
                            break;
                        case "u32":
                            tempvalue = "0xFFFFFFFF";
                            break;
                    }

                    initialcode = "SET_BUS_" + _union.Name + "("+ tempvalue +");";
                }
            }
            InitializationCode.Add(initialcode);
        }

        private void InitArray()
        {
            var initcode = new List<string>();

            if (_union.InitialState == InitialState.HiZ)
            {
                initcode.Add("GET_PIN_" + _union.Name + "(u8 BIT);");
            }
            else
            {
                if (_union.InitialState == InitialState.Low)
                {
                    initcode.Add("CLEAR_PIN_" + _union.Name + "(u8 BIT);");
                }
                else
                {
                    initcode.Add("SET_PIN_" + _union.Name + "(u8 BIT);");
                }
            }

            InitializationCode.AddRange(new CodeTemplateFor("BIT", _union.Chanels.Count.ToString(), initcode).Code);
        }
    }
}
