namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    using System.Linq;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Alliances;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class Ask_For_Joinable_Alliance_List : Message
    {
        public Ask_For_Joinable_Alliance_List(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14303;

        internal override void Process()
        {
            new Joinable_Alliance_List(this.Device)
            {
                Alliances = Resources.Clans.Values.Where(T =>
                        T.Header.Type != Hiring.CLOSED &&
                        T.Header.NumberOfMembers > 0 &&
                        T.Header.NumberOfMembers < 50 &&
                        T.Header.RequiredScore <= this.Device.GameMode.Level.Player.Score &&
                        T.Header.DuelScore <= this.Device.GameMode.Level.Player.DuelScore)
                    .Take(64).OrderByDescending(T => T.Header.Score).ToArray()
            }.Send();
        }
    }
}