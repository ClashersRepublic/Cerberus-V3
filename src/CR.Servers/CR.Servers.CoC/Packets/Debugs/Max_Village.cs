namespace CR.Servers.CoC.Packets.Debugs
{
    using System.Text;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class Max_Village : Debug
    {
        internal StringBuilder Help;

        internal int VillageID;

        public Max_Village(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
        }

        internal override Rank RequiredRank => Rank.Elite;

        internal override void Process()
        {
        }
    }
}