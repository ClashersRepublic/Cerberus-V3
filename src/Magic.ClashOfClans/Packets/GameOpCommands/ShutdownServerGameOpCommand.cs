using Magic.Core;
using Magic.Core.Network;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class ShutdownServerGameOpCommand : GameOpCommand
    {
        string[] m_vArgs;

        public ShutdownServerGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(4);
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= GetRequiredAccountPrivileges())
            {
                foreach (var onlinePlayer in ResourcesManager.OnlinePlayers)
                {
                    var p = new ShutdownStartedMessage(onlinePlayer.Client);
                    p.SetCode(5);
                    p.Send();
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
