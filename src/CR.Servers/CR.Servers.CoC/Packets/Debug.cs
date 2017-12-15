using System;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets
{
    internal class Debug : IDisposable
    {
        internal Device Device;

        internal Alliance Alliance;

        internal string[] Parameters;

        internal int ParameterOffset;

        internal string NextParameter => this.ParameterOffset >= this.Parameters.Length ? this.Parameters[this.ParameterOffset++] : null;

        internal virtual Rank RequiredRank => Rank.Player;


        internal Debug(Device Device, params string[] Parameters)
        {
            this.Device = Device;
            this.Parameters = Parameters;
        }

        internal virtual void Process()
        {
            // Process.
        }

        internal void SendChatMessage(string message)
        {
            new Global_Chat_Line(this.Device,this.Device.GameMode.Level.Player)
            {
                Message = message,
                Name = "[System] Command Manager",
                ExpLevel = 300,
                League = 22,
                Bot = true
            }.Send();
        }

        public void Dispose()
        {
            this.Parameters = null;
        }
    }
}
