using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Move_Building : Command
    {

        internal override int Type => 501;

        public Move_Building(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int X;
        internal int Y;

        internal int Id;

        internal override void Decode()
        {
            this.X = Reader.ReadInt32();
            this.Y = Reader.ReadInt32();
            this.Id = Reader.ReadInt32();

            ExecuteSubTick = Reader.ReadInt32();
        }

        internal override void Execute()
        {
            GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.Id);

            if (GameObject != null)
            {
                if (GameObject.Type != 3)
                {
                    GameObject.SetPositionXY(this.X, this.Y);
                }
            }
        }
    }
}
