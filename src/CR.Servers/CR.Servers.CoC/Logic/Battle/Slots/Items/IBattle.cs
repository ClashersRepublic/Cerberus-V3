namespace CR.Servers.CoC.Logic.Battle.Slots.Items
{
    using CR.Servers.CoC.Packets;
    using Newtonsoft.Json.Linq;

    public interface IBattle
    {
        double BattleTick { get; set; }

        int EndTick { get; set; }

        Battle_Command Commands { get; set; }

        JObject Save();

        void Add(Command Command);
    }
}