using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Upgrade_Unit : Command
    {
        internal int BuidlingID;
        internal int GlobalId;
        internal int Tick;
        internal bool IsSpell;
        internal Characters Troop;
        internal Spells Spell;

        public Upgrade_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuidlingID = Reader.ReadInt32();
            IsSpell = Reader.ReadInt32() == 1;

            GlobalId = Reader.ReadInt32();

            if (IsSpell)
            {
                Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            }
            else
            {
                Troop = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(GlobalId) as Characters;
            }
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;

            if (BuidlingID >= 500000000)
            {
                var go = Device.Player.GameObjectManager.GetGameObjectByID(BuidlingID,
                    Device.Player.Avatar.Variables.IsBuilderVillage);

                if (go != null)
                {
                    var building = (Construction_Item) go;

                    var upgradeComponent = building.GetUnitUpgradeComponent();

                    if (upgradeComponent != null)
                    {

                        var unitLevel = ca.GetUnitUpgradeLevel(IsSpell ? Spell : (Combat_Item) Troop);

                        if (upgradeComponent.CanStartUpgrading(IsSpell ? Spell : (Combat_Item) Troop))
                        {
                            var cost = IsSpell ? Spell.GetUpgradeCost(unitLevel) : Troop.GetUpgradeCost(unitLevel);
                            var upgradeResource = IsSpell ? Spell.GetUpgradeResource() : Troop.GetUpgradeResource();
                            if (ca.HasEnoughResources(upgradeResource.GetGlobalId(), cost))
                            {
#if DEBUG
                                Logger.SayInfo(IsSpell
                                    ? $"Spell : Upgrading {Spell.Row.Name} with ID {GlobalId}"
                                    : $"Unit : Upgrading {Troop.Row.Name} with ID {GlobalId}");
#endif
                                ca.Resources.Minus(upgradeResource.GetGlobalId(), cost);
                                upgradeComponent.StartUpgrading(IsSpell ? Spell : (Combat_Item) Troop);
                            }
                        }
                    }
                    else
                    {
                        ExceptionLogger.Log(new NullReferenceException(),
                            $"Object with id {BuidlingID} from user {Device.Player.Avatar.UserId} is not a Unit_Upgrade_Component at Upgrade Unit");
                    }
                }
                else
                {
                    ExceptionLogger.Log(new NullReferenceException(),
                        $"Object with id {BuidlingID} from user {Device.Player.Avatar.UserId} is null at Upgrade Unit");
                }
            }
            else
            {
                ExceptionLogger.Log(new Exception(),
                    $"Object with id {BuidlingID} from user {Device.Player.Avatar.UserId} is having weird id at Upgrade Unit.Previous command is {Device.Last_Command}");
            }
        }

    }
}

