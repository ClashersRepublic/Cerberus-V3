namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Battle;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;
    using Newtonsoft.Json.Linq;

    internal class Surrender_Attack : Command
    {
        public Surrender_Attack(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override int Type => 703;

        internal override void Execute()
        {
            Level Level = this.Device.GameMode.Level;
            if (Level.GameObjectManager.Map == 0)
            {
            }
            else
            {
                if (this.Device.State == State.IN_1VS1_BATTLE)
                {
                    Battle_V2 Battle = Resources.BattlesV2.GetPlayer(Level.Player.BattleIdV2, Level.Player.UserId);
                    Battle.Add(this);
                    Level.BattleManager.BattleCommandManager.StoreCommands(this);
                }
            }
        }


        internal override void Load(JToken Token)
        {
            JObject Command = (JObject) Token["c"];

            if (Command != null)
            {
                JsonHelper.GetJsonNumber(Command, "t", out this.ExecuteSubTick);
            }
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"ct", this.Type},
                {
                    "c", new JObject
                    {
                        {"t", this.ExecuteSubTick}
                    }
                }
            };

            return Json;
        }
    }
}