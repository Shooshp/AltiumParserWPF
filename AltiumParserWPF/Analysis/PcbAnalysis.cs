namespace AltiumParserWPF.Analysis
{
    public static class PcbAnalysis
    {
        static string newettSmall = "DIN41612R.РОЗ.УГЛ.48";
        static string newettBig = "DIN41612R.РОЗ.УГЛ.96";

        private static string oldettSmall = "CONN_DIN_48";
        private static string oldettBig = "CONN_DIN_96";

        public static PcbTypes GetPsbType(AltiumParser.AltiumParser parser)
        {
            var type = @"unknow";

            if (parser.BuildOfMaterials.Exists(x=>x.DeviceType.Contains(newettSmall)) 
                && parser.BuildOfMaterials.Exists(x => x.DeviceType.Contains(newettBig)))
            {
                return PcbTypes.EttNew;
            }

            if (parser.BuildOfMaterials.Exists(x => x.DeviceType.Contains(oldettSmall))
                && parser.BuildOfMaterials.Exists(x => x.DeviceType.Contains(oldettBig)))
            {
                return PcbTypes.EttOld;
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
