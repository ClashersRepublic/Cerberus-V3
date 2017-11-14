using CR.Servers.CoC.Extensions.Game;

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
    }
}