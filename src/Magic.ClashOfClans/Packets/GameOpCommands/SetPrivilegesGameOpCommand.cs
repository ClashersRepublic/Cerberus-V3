using System;
using Magic.Core;
using Magic.Logic;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class SetPrivilegesGameOpCommand : GameOpCommand
    {
        #region Private Fields

        readonly string[] m_vArgs;

        #endregion Private Fields

        #region Public Constructors

        public SetPrivilegesGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(4);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 3)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var accountPrivileges = Convert.ToByte(m_vArgs[2]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (accountPrivileges < level.GetAccountPrivileges())
                        {
                            if (l != null)
                            {
                                l.SetAccountPrivileges(accountPrivileges);
                            }
                            else
                            {
                                //Debugger.WriteLine("SetPrivileges failed: id " + id + " not found");
                            }
                        }
                        else
                        {
                            //Debugger.WriteLine("SetPrivileges failed: target privileges too high");
                        }
                    }
                    catch 
                    {
                        ////Debugger.WriteLine("SetPrivileges failed with error: " + ex);
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }

        #endregion Public Methods
    }
}