using System;
using System.Text;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Debugs.Elite
{
    internal class Fast_Forward : Debug
    {
        internal override Rank RequiredRank => Rank.Elite;

        public Fast_Forward(Device Device, params string[] Parameters) : base(Device, Parameters)
        {

        }
        
        internal StringBuilder Help;

        internal override void Process()
        {
            if (Parameters.Length >= 1)
            {
                if (int.TryParse(this.Parameters[0], out int Time))
                {
                    if (Time > 0)
                    {
                        this.Device.GameMode.Level.FastForwardTime(Time);

                        if (this.Device.Connected)
                            new Own_Home_Data(this.Device).Send();


                        this.SendChatMessage("Command successfuly processed! Total elapsed time " + TimeSpan.FromSeconds(Time));
                    }
                }
                else
                {
                    this.Help = new StringBuilder();
                    this.Help.AppendLine("Time exceed the limit!");
                    this.SendChatMessage(Help.ToString());
                }
                //Time bust be at least 1
            }
        }
    }
}
