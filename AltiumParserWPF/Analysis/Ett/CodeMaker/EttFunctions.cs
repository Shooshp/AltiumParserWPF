using System.Collections.Generic;
using System.Linq;

namespace AltiumParserWPF.Analysis.Ett.CodeMaker
{
    public class EttFunctions
    {
        public ConnectionUnion Union;

        public List<string> InitializationCode;
        public List<string> WorkingRutines;

        public CodeTemplateFunction GetBusFunction;
        public CodeTemplateFunction SetBusFunction;

        public CodeTemplateFunction GetPinFromArrayFunction;
        public CodeTemplateFunction SetPinInArrayFunction;
        public CodeTemplateFunction ClearPinArrayFunction;

        public string SingleChanelInit;


        public EttFunctions(ConnectionUnion union)
        {
            Union = union;

            InitializationCode = new List<string>();
            WorkingRutines = new List<string>();

            switch (Union.ConnectionType)
            {
                case ConnectionType.Bus:

                    switch (Union.Direction)
                    {
                        case Direction.In:
                            CreateGetBusFunction();
                            WorkingRutines.AddRange(GetBusFunction.Code);
                            break;

                        case Direction.Out:
                            CreateSetBusFunction();
                            WorkingRutines.AddRange(SetBusFunction.Code);
                            break;

                        case Direction.Bidir:
                            CreateGetBusFunction();
                            CreateSetBusFunction();
                            WorkingRutines.AddRange(GetBusFunction.Code);
                            WorkingRutines.AddRange(SetBusFunction.Code);
                            break;
                    }

                    break;

                case ConnectionType.Array:
                    
                    switch (Union.Direction)
                    {
                        case Direction.In:
                            CreateGetPinFromArrayFunction();
                            WorkingRutines.AddRange(GetPinFromArrayFunction.Code);
                            break;

                        case Direction.Out:
                            CreateSetPinInArrayFunction();
                            CreateClearPinArrayFunction();
                            WorkingRutines.AddRange(SetPinInArrayFunction.Code);
                            WorkingRutines.AddRange(ClearPinArrayFunction.Code);
                            break;

                        case Direction.Bidir:
                            CreateGetPinFromArrayFunction();
                            CreateSetPinInArrayFunction();
                            CreateClearPinArrayFunction();
                            WorkingRutines.AddRange(GetPinFromArrayFunction.Code);
                            WorkingRutines.AddRange(SetPinInArrayFunction.Code);
                            WorkingRutines.AddRange(ClearPinArrayFunction.Code);
                            break;
                    }
                    break;

                case ConnectionType.Global:
                    CreateSingleChanelInit();
                    WorkingRutines.Add(SingleChanelInit);
                    break;
            }
        }

        private void CreateGetBusFunction()
        {
            var tempcodeget = new List<string>();
            tempcodeget.Add(Union.GetDataType() + " LOCAL_VALUE=0;");

            var functionbody = new List<string>();

            functionbody.Add("ChannelControl::dir(" + Union.Name + "[DATA_BIT], 1);");
            functionbody.Add("LOCAL_VALUE = LOCAL_VALUE | (ChannelControl::read(" + Union.Name + "[DATA_BIT]) << DATA_BIT);");

            tempcodeget.AddRange(new CodeTemplateFor("DATA_BIT", Union.Chanels.Count.ToString(), functionbody).Code);
            tempcodeget.Add("return LOCAL_VALUE;");

            var getname = Union.GetDataType() + " GET_" + Union.Name + "(void)";

            GetBusFunction = new CodeTemplateFunction(getname, tempcodeget);
        }

        private void CreateSetBusFunction()
        {
            var tempcodeset = new List<string>();

            var functionbody  = new List<string>();

            functionbody.Add("ChannelControl::dir(" + Union.Name + "[DATA_BIT], 0);");
            functionbody.Add("ChannelControl::write(" + Union.Name + "[DATA_BIT],((DAT>>DATA_BIT)&0x1));");

            tempcodeset.AddRange(new CodeTemplateFor("DATA_BIT", Union.Chanels.Count.ToString(), functionbody).Code);

            var setname = "void SET_" + Union.Name + "(" + Union.GetDataType() + " DAT)";
            SetBusFunction = new CodeTemplateFunction(setname, tempcodeset);
        }

        private void CreateSetPinInArrayFunction()
        {
            var tempcodeset = new List<string>();
            var setpinname = "void SET_" + Union.Name + "(u8 BIT)";
            tempcodeset.Add("ChannelControl::dir(" + Union.Name + "[BIT], 0);");
            tempcodeset.Add("ChannelControl::write(" + Union.Name + "[BIT],1);");

            SetPinInArrayFunction = new CodeTemplateFunction(setpinname, tempcodeset);
        }

        private void CreateClearPinArrayFunction()
        {
            var tempcodeclear = new List<string>();
            var clearpinname = "void CLEAR_" + Union.Name + "(u8 BIT)";
            tempcodeclear.Add("ChannelControl::dir(" + Union.Name + "[BIT], 0);");
            tempcodeclear.Add("ChannelControl::write(" + Union.Name + "[BIT],0);");

            ClearPinArrayFunction = new CodeTemplateFunction(clearpinname, tempcodeclear);
        }

        private void CreateGetPinFromArrayFunction()
        {
            var tempcodeget = new List<string>();
            var getpinname = "u8 GET_" + Union.Name + "(u8 BIT)";
            tempcodeget.Add("ChannelControl::dir(" + Union.Name + "[BIT], 1);");
            tempcodeget.Add("return ChannelControl::read(" + Union.Name + "[BIT]);");

            GetPinFromArrayFunction = new CodeTemplateFunction(getpinname, tempcodeget);
        }

        private void CreateSingleChanelInit()
        {
            var chanel = Union.Chanels.ElementAt(0);
            var tempname = "";

            if (chanel.ChanelName.Contains("CH"))
            {
                tempname = "Ch" + chanel.ChanelName.Replace("CH", "").Replace(" ", "");
            }

            if (chanel.ChanelName.Contains("J1_") || chanel.ChanelName.Contains("J2_"))
            {
                tempname = "C" + chanel.ChanelName.Replace(" ", "");
            }

            SingleChanelInit = "\ttypdef " + tempname + " " + chanel.ConnectionName.ToUpper() + ";";
        }
    }
}
