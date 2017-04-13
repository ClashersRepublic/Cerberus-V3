using System;
using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class CastSpellCommand : Command
    {
        public SpellData Spell;
        public uint Unknown1;
        public int X;
        public int Y;

        public CastSpellCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            Spell = (SpellData) br.ReadDataReference();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            DataSlot dataSlot = level.GetPlayerAvatar().GetSpells().Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == this.Spell.GetGlobalID()));
            if (dataSlot == null)
              return;
            dataSlot.Value = dataSlot.Value - 1;
        }
    }
}
