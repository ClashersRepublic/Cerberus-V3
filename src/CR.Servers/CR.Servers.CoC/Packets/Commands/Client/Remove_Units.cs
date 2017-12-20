using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Commands.Client.List;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Remove_Units : Command
    {
        internal override int Type => 550;

        public Remove_Units(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal List<UnitToRemove> UnitsToRemove;

        internal override void Decode()
        {
            var UnitCount = this.Reader.ReadInt32();
            this.UnitsToRemove = new List<UnitToRemove>(UnitCount);

            for (int i = 0; i < UnitCount; i++)
            {
                this.Reader.ReadInt32();
                this.UnitsToRemove.Add(new UnitToRemove
                {
                    Type = this.Reader.ReadInt32(),
                    Id = this.Reader.ReadInt32(),
                    Count = this.Reader.ReadInt32(),
                });
                this.Reader.ReadInt32();
            }
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            foreach (var UnitToRemove in UnitsToRemove)
            {
                if (UnitToRemove.Type == 0)
                {
                    var Unit = Level.Player.Units.Find(T => T.Data == UnitToRemove.Id);
                    if (Unit != null)
                    {
                        Unit.Count -= UnitToRemove.Count;
                    }

                }
                else if (UnitToRemove.Type == 1)
                {
                    var Spell = Level.Player.Spells.Find(T => T.Data == UnitToRemove.Id);
                    if (Spell != null)
                    {
                        Spell.Count -= UnitToRemove.Count;
                    }
                }
                else
                    Logging.Error(this.GetType(), "Unable to remove units. Unknown unit type!");
            }
        }
    }
}
