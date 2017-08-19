using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Change_Village_Mode : Command
    {
        internal int Tick;

        public Change_Village_Mode(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Device.Player.Avatar.Variables.Set(Variable.VillageToGoTo, Reader.ReadInt32());
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
#if DEBUG
            Logger.SayInfo(
                $"Village Manager : Changing mode to {(Village_Mode) Device.Player.Avatar.Variables.Get(Variable.VillageToGoTo)}");
#endif
        }
    }
}
