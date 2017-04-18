using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Files.Logic;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.Packets.GameOpCommands
{
    internal class MaxBaseGameOpCommand : GameOpCommand
    {
        public static readonly string s_maxBase = File.ReadAllText("contents/max_home.json");

        public MaxBaseGameOpCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            level.SetHome(s_maxBase);
            new OwnHomeDataMessage(level.Client, level).Send();
        }
    }
}
