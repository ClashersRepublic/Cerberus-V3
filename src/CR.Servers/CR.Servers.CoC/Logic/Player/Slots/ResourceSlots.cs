using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic.Enums;

namespace CR.Servers.CoC.Logic
{
    internal class ResourceSlots : DataSlots
    {
        internal ResourceSlots(int Capacity = 10) : base(Capacity)
        {
            // ResourceSlots.
        }

        internal void Initialize()
        {
            this.Set(3000001, Globals.StartingGold);
            this.Set(3000002, Globals.StartingElixir);

            this.Set(3000007, Globals.StartingGold2);
            this.Set(3000008, Globals.StartingElixir2);
        }

        internal void Set(Resource Resource, int Count)
        {
            Set(3000000 + (int)Resource, Count);
        }

        internal void Plus(int Global, int Count)
        {
            var i = FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count += Count;
            else
                Add(new Item(Global, Count));
        }

        internal void Plus(Resource Resource, int Count)
        {
            Plus(3000000 + (int)Resource, Count);
        }

        internal bool Minus(int Global, int Count)
        {
            var i = FindIndex(R => R.Data == Global);

            if (i > -1)
                if (this[i].Count >= Count)
                {
                    this[i].Count -= Count;
                    return true;
                }

            return false;
        }

        internal void Minus(Resource _Resource, int _Value)
        {
            var Index = FindIndex(T => T.Data == 3000000 + (int)_Resource);

            if (Index > -1)
                this[Index].Count -= _Value;
        }
    }
}