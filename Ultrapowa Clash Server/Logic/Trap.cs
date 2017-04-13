using UCS.Files.Logic;

namespace UCS.Logic
{
    internal class Trap : ConstructionItem
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new TriggerComponent());
        }

        public override int ClassId => 4;

        public TrapData GetTrapData() => (TrapData)GetData();
    }
}
