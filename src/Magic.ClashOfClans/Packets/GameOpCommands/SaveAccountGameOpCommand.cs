using Magic.Core;
using Magic.Core.Network;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class SaveAccountGameOpCommand : GameOpCommand
    {
        public SaveAccountGameOpCommand(string[] args)
        {
            SetRequiredAccountPrivileges(5);
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= GetRequiredAccountPrivileges())
            {
                DatabaseManager.Instance.Save(level);

                var p = new GlobalChatLineMessage(level.Client);
                p.SetChatMessage("Game Successful Saved!");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("CoM Bot");
                p.Send();
            }
            else
            {
                var p = new GlobalChatLineMessage(level.Client);
                p.SetChatMessage("GameOp command failed. Access to Admin GameOP is prohibited.");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("CoM Bot");
                p.Send();
            }
        }
    }
}
