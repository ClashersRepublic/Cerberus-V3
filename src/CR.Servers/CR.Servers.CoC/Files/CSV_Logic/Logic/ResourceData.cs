using System;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Files.CSV_Reader;

﻿namespace CR.Servers.CoC.Files.CSV_Logic.Logic
{
    internal class ResourceData : Data
    {
        internal ResourceData  WarRefResourceData;

        public ResourceData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
        }

        public override string Name { get; set; }
        public string TID { get; set; }
        public string SWF { get; set; }
        public string CollectEffect { get; set; }
        public string ResourceIconExportName { get; set; }
        public string StealEffect { get; set; }
        public int StealLimitMid { get; set; }
        public string StealEffectMid { get; set; }
        public int StealLimitBig { get; set; }
        public string StealEffectBig { get; set; }
        public bool PremiumCurrency { get; set; }
        public string HudInstanceName { get; set; }
        public string CapFullTID { get; set; }
        public int TextRed { get; set; }
        public int TextGreen { get; set; }
        public int TextBlue { get; set; }
        public string WarRefResource { get; set; }
        public string BundleIconExportName { get; set; }
        public int VillageType { get; set; }

        internal override void Process()
        {
            this.WarRefResourceData = (ResourceData) CSV.Tables.Get(Gamefile.Resources).GetData(this.WarRefResource);
        }
    }
}
