using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Change_Village_Mode : Command
    {
        internal override int Type => 591;

        public Change_Village_Mode(Device client, Reader reader) : base(client, reader)
        {
        }

        internal int State;

        internal override void Decode()
        {
            this.State = Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            if (State == 0 || State == 1)
                Level.Player.Variables.Set((int) Variable.VillageToGoTo, State);
#if DEBUG
            Logging.Info(this.GetType(),
                $"Village Manager : Changing mode to {(Village_Mode) Level.Player.Variables.GetCountByGlobalId((int) Variable.VillageToGoTo)}");
#endif
        }

    }

    public enum Village_Mode
    {
        NORMAL_VILLAGE,
        BUILDER_VILLAGE
    }
}