using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Swap_GameObject : Command
    {
        internal override int Type => 577;

        public Swap_GameObject(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int Id1;
        internal int Id2;

        internal override void Decode()
        {
            this.Id1 = Reader.ReadInt32();
            this.Id2 = Reader.ReadInt32();

            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            var GameObject1 = Level.GameObjectManager.Filter.GetGameObjectById(this.Id1);
            var GameObject2 = Level.GameObjectManager.Filter.GetGameObjectById(this.Id2);

            if (GameObject1 != null && GameObject2 != null)
            {
                if ((GameObject1.Type == 0 || GameObject1.Type == 4 || GameObject1.Type == 6) && (GameObject2.Type == 0 || GameObject2.Type == 4 || GameObject2.Type == 6))
                {
                    int X = GameObject1.TileX;
                    int Y = GameObject1.TileY;

                    GameObject1.SetPositionXY(GameObject2.TileX, GameObject2.TileY);
                    GameObject2.SetPositionXY(X, Y);
                }
            }
        }
    }
}