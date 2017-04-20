using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Network;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class KickGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public KickGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 1;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 2)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManagerOld.GetPlayer(id);
                        if (ResourcesManagerOld.IsPlayerOnline(l))
                        {
                            new OutOfSyncMessage(l.Client).Send();
                        }
                        else
                        {
                            Console.WriteLine("Kick failed: id " + id + " not found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Kick failed with error: " + ex);
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
