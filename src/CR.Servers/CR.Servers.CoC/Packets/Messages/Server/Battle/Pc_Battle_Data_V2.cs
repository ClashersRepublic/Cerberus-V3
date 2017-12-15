using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Pc_Battle_Data_V2 : Message
    {
        internal override short Type => 25023;

        public Pc_Battle_Data_V2(Device Device) : base(Device)
        {
            this.Device.State = State.IN_1VS1_BATTLE;
        }

        internal Level Enemy;
        internal override void Encode()
        {
            this.Device.GameMode.Level.Player.Encode(this.Data);
            this.Data.AddLong(this.Enemy.Player.UserId);
                      
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddCompressed(this.Enemy.BattleV2().ToString());
            this.Data.AddCompressed(Game_Events.Events_Json);
            this.Data.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");

            this.Data.AddLong(this.Enemy.Player.UserId);
            this.Data.AddHexa("592FA598".Replace(" ",""));

        }
    }
}
