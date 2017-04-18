using System;
using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class SetActiveVillageLayoutCommand : Command
    {
        private int Layout;
        public SetActiveVillageLayoutCommand(PacketReader br)
        {
            Layout = br.ReadInt32();
        }
        public override void Execute(Level level)
        {
            level.Avatar.SetActiveLayout(Layout);
        }
    }
}
