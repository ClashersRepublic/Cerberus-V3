using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class CancelShieldCommand : Command
    {
        public int Tick;

        public CancelShieldCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            level.GetPlayerAvatar().SetShieldTime(0);
        }
    }      
}
