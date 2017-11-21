using System;
using CR.Servers.CoC.Logic;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets
{
    internal class Debug : IDisposable
    {
        internal Device Device;

        internal string[] Parameters;

        internal int ParameterOffset;

        internal string NextParameter => this.ParameterOffset >= this.Parameters.Length ? this.Parameters[this.ParameterOffset++] : null;

        internal virtual Rank RequiredRank => Rank.Player;


        internal Debug(Device Device, params string[] Parameters)
        {
            this.Device = Device;
            this.Parameters = Parameters;
        }

        internal virtual void Decode()
        {

        }

        internal virtual void Process()
        {
            // Process.
        }

        internal void SendChatMessage(string message)
        {
            /*new Global_Chat_Entry(Device)
            {
                Message = message,
                Message_Sender = Device.Player.Avatar,
                Bot = true
            }.Send();*/
        }

        public void Dispose()
        {
            this.Parameters = null;
        }
    }
}
