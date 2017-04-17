using Magic.Core;
using Magic.Core.Network;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class MaxRessourcesCommand : GameOpCommand
    {
        public MaxRessourcesCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                var p = level.GetPlayerAvatar();
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"), 999999999);
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"), 999999999);
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("DarkElixir"), 999999999);
                p.SetDiamonds(999999999);
                new OwnHomeDataMessage(level.GetClient(), level).Send();
            }
            else
                SendCommandFailedMessage(level.GetClient());
        }
    }
}
