using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class TrainUnitCommand : Command
    {
        public int Count;
        public int UnitType;
        public int Tick;

        public TrainUnitCommand(PacketReader br)
        {
            br.ReadInt32();
            int num1 = (int)br.ReadUInt32();
            UnitType = br.ReadInt32();
            Count = br.ReadInt32();
            int num2 = (int)br.ReadUInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            if (UnitType.ToString().StartsWith("400"))
            {
                CombatItemData _TroopData = (CombatItemData)CSVManager.DataTables.GetDataById(UnitType);
                List<DataSlot> units = level.GetPlayerAvatar().GetUnits();
                ResourceData trainingResource = _TroopData.GetTrainingResource();
                if (_TroopData == null)
                    return;
                DataSlot dataSlot1 = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _TroopData.GetGlobalID()));
                if (dataSlot1 != null)
                {
                    dataSlot1.Value = dataSlot1.Value + Count;
                }
                else
                {
                    DataSlot dataSlot2 = new DataSlot((Data)_TroopData, Count);
                    units.Add(dataSlot2);
                }
                avatar.SetResourceCount(trainingResource, avatar.GetResourceCount(trainingResource) - _TroopData.GetTrainingCost(avatar.GetUnitUpgradeLevel(_TroopData)));
            }
            else
            {
                if (!this.UnitType.ToString().StartsWith("260"))
                    return;
                SpellData _SpellData = (SpellData)CSVManager.DataTables.GetDataById(UnitType);
                List<DataSlot> spells = level.GetPlayerAvatar().GetSpells();
                ResourceData trainingResource = _SpellData.GetTrainingResource();
                if (_SpellData == null)
                    return;
                DataSlot dataSlot1 = spells.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _SpellData.GetGlobalID()));
                if (dataSlot1 != null)
                {
                    dataSlot1.Value = dataSlot1.Value + Count;
                }
                else
                {
                    DataSlot dataSlot2 = new DataSlot((Data)_SpellData, Count);
                    spells.Add(dataSlot2);
                }
                avatar.SetResourceCount(trainingResource, avatar.GetResourceCount(trainingResource) - _SpellData.GetTrainingCost(avatar.GetUnitUpgradeLevel((CombatItemData)_SpellData)));
            }
        }
    }
}
