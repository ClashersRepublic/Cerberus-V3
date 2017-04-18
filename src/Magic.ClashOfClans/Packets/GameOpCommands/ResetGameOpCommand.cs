using Magic.Core;
using Magic.Core.Network;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class ResetGameOpCommand : GameOpCommand
    {
        public ResetGameOpCommand(string[] args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            level.SetHome(ObjectManager.m_vHomeDefault);
            new OwnHomeDataMessage(level.Client, level).Send();
        }
    }
}
