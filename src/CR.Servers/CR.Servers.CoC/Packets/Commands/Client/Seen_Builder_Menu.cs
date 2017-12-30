using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Seen_Builder_Menu : Command
    {
        internal override int Type => 604;

        public Seen_Builder_Menu(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int State;

        internal override void Decode()
        {
            this.State = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            if (this.State == 0)
            {
                this.Device.GameMode.Level.Player.Variables.Set(Variable.SeenBuilderMenu, 1);
            }
        }
    }
}
