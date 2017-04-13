using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.GameOpCommands
{
    internal class MaxBaseGameOpCommand : GameOpCommand
    {
        public MaxBaseGameOpCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                var p = level.GetPlayerAvatar();
                using (StreamReader streamReader = new StreamReader("Gamefiles/level/PVP/Base55.json"))
                {
                    string end = streamReader.ReadToEnd();
                    ResourcesManager.SetGameObject(level, end);
                    new OutOfSyncMessage(level.GetClient()).Send();
                }
            }
            else
                SendCommandFailedMessage(level.GetClient());
        }
    }
}
