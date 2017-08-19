using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Unlock_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;

        public Unlock_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        public override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        public override void Process()
        {
            var ca = this.Device.Player.Avatar;

            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId, this.Device.Player.Avatar.Variables.IsBuilderVillage);

            var b = (Construction_Item)go;

            var bd = (Buildings)b.GetConstructionItemData;

            if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel).GetGlobalId(), bd.GetBuildCost(b.GetUpgradeLevel)))
            { 
#if DEBUG
               var name = go.Data.Row.Name;
               Logger.SayInfo($"Building: Unlocking {name} with ID {BuildingId}");
#endif
                if (bd.IsAllianceCastle())
                {
                    var a = (Building)go;
                    var al = a.GetBuildingData;

                    ca.Castle_Level++;
                    ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                    ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                }
                if (name == "Hero Altar Warmachine")
                {

                    if (b.GetHeroBaseComponent(true) != null)
                    {
                        Buildings data = (Buildings)b.Data;
                        Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                        this.Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                        this.Device.Player.Avatar.SetHeroHealth(hd, 0);
                        this.Device.Player.Avatar.SetHeroState(hd, 3);
                    }
                }

                var rd = bd.GetBuildResource(b.GetUpgradeLevel);
                ca.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(b.GetUpgradeLevel));
                b.Locked = false;
            }
        }
    }
}