using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.Logic.AvatarStreamEntry;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.GameOpCommands
{
    internal class ServerStatusGameOpCommand   : GameOpCommand
    {
        readonly string[] m_vArgs;
        private readonly PerformanceCounter _cpuCounter;

        public ServerStatusGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
            var curProcess = Process.GetCurrentProcess();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    _cpuCounter.NextValue(); //Always 0
                    var avatar = level.GetPlayerAvatar();
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.GetId());
                    mail.SetSenderAvatarId(avatar.GetId());
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(2);
                    mail.SetAllianceId(0);
                    mail.SetAllianceBadgeData(1526735450);
                    mail.SetAllianceName("UCS Server Information");
					mail.SetMessage(@"Online Players: " + ResourcesManager.GetOnlinePlayers().Count +
						"\nIn Memory Players: " + ResourcesManager.GetInMemoryLevels().Count +
						"\nConnected Players: " + ResourcesManager.GetConnectedClients().Count +
						//"\nUCS Ram: " + (Process.GetCurrentProcess().WorkingSet64 / 1048576) + "MB/" + //Unknown yet how to get properly
						"\nServer Ram: " + PerformanceInfo.GetPhysicalAvailableMemoryInMiB() + "MB/" + Performances.GetTotalMemory() + "MB" +
						"\nServer CPU " + _cpuCounter.NextValue() + "%"  //Match Taskmanager
						);

                    mail.SetSenderLevel(avatar.GetAvatarLevel());
                    mail.SetSenderLeagueId(avatar.GetLeagueId());

                    var p = new AvatarStreamEntryMessage(level.GetClient());
                    p.SetAvatarStreamEntry(mail);
                    p.Send();
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}
