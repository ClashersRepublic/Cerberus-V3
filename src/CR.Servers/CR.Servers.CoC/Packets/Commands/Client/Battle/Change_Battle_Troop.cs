using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Change_Battle_Troop : Command
    {
        internal override int Type => 711;

        public Change_Battle_Troop(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32(); //Old Troop
            this.Reader.ReadInt32(); //New Troop
            base.Decode();
        }

        internal override void Execute()
        {
            //Handle this shit
            base.Execute();
        }
    }
}
