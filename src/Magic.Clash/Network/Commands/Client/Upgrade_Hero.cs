using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Upgrade_Hero : Command
    {
        internal int BuildingId;
        internal uint Tick;

        public Upgrade_Hero(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Tick = Reader.ReadUInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
                Device.Player.Avatar.Variables.IsBuilderVillage);
            var hbc = ((Construction_Item)go)?.GetHeroBaseComponent();
            if (hbc != null)
                if (hbc.CanStartUpgrading)
                {
                    var hd = CSV.Tables.Get(Gamefile.Heroes).GetData(hbc.HeroData.Name) as Heroes;
                    var currentLevel = ca.GetUnitUpgradeLevel(hd);
                    var rd = hd.GetUpgradeResource();
                    var cost = hd.GetUpgradeCost(currentLevel);
                    if (ca.HasEnoughResources(rd.GetGlobalId(), cost))
                        if (Device.Player.Avatar.Variables.IsBuilderVillage
                            ? Device.Player.HasFreeBuilderWorkers
                            : Device.Player.HasFreeVillageWorkers)
                            hbc.StartUpgrading(Device.Player.Avatar.Variables.IsBuilderVillage);
                }
        }
    }
}
