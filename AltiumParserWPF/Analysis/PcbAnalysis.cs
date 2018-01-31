using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AltiumParserWPF
{
    public static class PcbAnalysis
    {
        static string small = "DIN41612R.РОЗ.УГЛ.48";
        static string big = "DIN41612R.РОЗ.УГЛ.96";

        public static PcbTypes GetPsbType(AltiumParser.AltiumParser parser)
        {
            var type = @"unknow";

            if (parser.BuildOfMaterials.Exists(x=>x.DeviceType.Contains(small)) 
                && parser.BuildOfMaterials.Exists(x => x.DeviceType.Contains(big)))
            {
                return PcbTypes.EttNew;
            }

            return PcbTypes.Unknown;
        }

        public enum PcbTypes
        {
            EttNew,
            EttOld,
            F2kNew,
            F2kOld,
            Unknown
        }
    }
}
