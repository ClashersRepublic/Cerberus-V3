using System;
using Magic.Core;
using Magic.Core.Network;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class BanGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public BanGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(2);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 2)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (l != null)
                        {
                            if (l.GetAccountPrivileges() < level.GetAccountPrivileges())
                            {
                                l.SetAccountStatus(99);
                                l.SetAccountPrivileges(0);
                                if (ResourcesManager.IsPlayerOnline(l))
                                {
                                    new OutOfSyncMessage(l.GetClient()).Send();
                                    
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    catch 
                    {
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}
