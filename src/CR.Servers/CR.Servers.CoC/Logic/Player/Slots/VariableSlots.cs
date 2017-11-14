using CR.Servers.CoC.Logic.Enums;

namespace CR.Servers.CoC.Logic
{
    internal class VariableSlots : DataSlots
    {
        internal VariableSlots(int Capacity = 20) : base(Capacity)
        {
            // ResourceSlots.
        }

        internal void Initialize()
        {
            this.Set((int)Variable.VillageToGoTo, 0);
        }
    }
}
