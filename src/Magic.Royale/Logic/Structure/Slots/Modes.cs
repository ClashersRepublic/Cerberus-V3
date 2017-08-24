using System.Collections.Generic;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Modes : List<Slot>
    {
        internal Modes()
        {
            // Modes.
        }

        internal Modes(bool Initialize)
        {
            if (Initialize)
                this.Initialize();
        }

        internal bool IsAttackingOwnBase => Get(Mode.ATTACK_OWN_BASE) == 1;

        internal int Get(int Gl_ID)
        {
            var i = FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Mode Variables)
        {
            return Get((int) Variables);
        }

        internal void Set(int Global, int Count)
        {
            var i = FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count = Count;
            else
                Add(new Slot(Global, Count));
        }

        internal void Set(Mode Variables, int Count)
        {
            Set((int) Variables, Count);
        }

        internal void Minus(Mode _Resource, int _Value)
        {
            var Index = FindIndex(T => T.Data == (int) _Resource);

            if (Index > -1)
                this[Index].Count -= _Value;
        }

        internal void Initialize()
        {
            Set(Mode.ATTACK_OWN_BASE, 0);
        }
    }
}