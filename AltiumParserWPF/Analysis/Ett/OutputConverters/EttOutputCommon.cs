using System.Collections.Generic;
using AltiumParserWPF.Analysis.Ett.CodeMaker;
using AltiumParserWPF.Analysis.Ett.CodeMaker.Templates;

namespace AltiumParserWPF.Analysis.Ett.OutputConverters
{
    public class EttOutputCommon : EttOutputTemplate
    {
        public List<EttFunctions> Functions;
        public HeaderTemplate Header;

        public EttOutputCommon(List<ConnectionUnion> unions)
        {
            Unions = new List<ConnectionUnion>();
            Functions = new List<EttFunctions>();
            Header = new HeaderTemplate("ProcessFK");
            Unions = unions;

            Name = Header.Name + ".h";

            foreach (var union in Unions)
            {
                Functions.Add(new EttFunctions(union));
            }

            Header.AddBlankLine();
            Header.AddDefine("CHIP_COUNT", 55);
            Header.AddDefine("PORT_COUNT", 112);
            Header.AddBlankLine();
            Header.AddInclude("Pereferial_manager/Control_manager.h");
            Header.AddInclude("I2C_Devices/Iic_pereferial.h");
            Header.AddInclude("GPIO/iopins.h");
            Header.AddInclude("GPIO/pinout.h");
            Header.AddInclude("GPIO/pinlist.h");
            Header.AddInclude("<sleep.h>");
            Header.AddBlankLine();
            Header.AddNameSpace("IO");
            Header.AddNameSpace("MANAGERS");
            Header.AddBlankLine();

            foreach (var union in Unions)
            {
                if (union.ConnectionType != ConnectionType.Global)
                {
                    Header.Code.Add(union.ToString());
                }
            }

            var classcode = new List<string>();
            var initcode = new List<string>();
            var workingcode = new List<string>();
            var errorcode = new List<string>
            {
                "if ( A!=B)",
                "\tif (Control_manager::resultBuff[ GROUPE*CHIP_COUNT + DUT ] < 0xFF)",
                "\t\tControl_manager::resultBuff[ GROUPE*CHIP_COUNT + DUT ]++;"
            };

            var errorRoutine = new CodeTemplateFunction("void ERRORS(u8 A, u8 B, u8 DUT, u8 GROUPE)", errorcode);

            foreach (var function in Functions)
            {
                initcode.AddRange(function.InitializationCode);
                workingcode.AddRange(function.WorkingRutines);
            }

            var initRoutine = new CodeTemplateFunction("void init()", initcode);
            

            classcode.Add("public:");
            classcode.AddRange(initRoutine.Code);
            classcode.Add("static void FK();");
            classcode.Add("private:");
            classcode.AddRange(errorRoutine.Code);

            foreach (var function in Functions)
            {
                classcode.AddRange(function.WorkingRutines);
            }

            var workingclass = new CodeTemplateClass(classcode, "ProcessFK");

            Header.Code.AddRange(workingclass.Code);
            Header.CloseHeader();
            CombineList(Header.Code);
        }
    }
}
