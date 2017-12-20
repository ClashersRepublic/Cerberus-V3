using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Request_Join_Alliance : Message
    {
        internal override short Type => 14317;

        public Request_Join_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int HighId;
        internal int LowId;
        internal string Message;

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {
            var Player = this.Device.GameMode.Level.Player;
            var Alliance = Resources.Clans.Get(this.HighId, this.LowId);

            if (Alliance != null)
            {
                Alliance.Streams.AddEntry(new JoinRequestStreamEntry()
                {
                    SenderHighId = Player.HighID,
                    SenderLowId = Player.LowID,
                    SenderLeague = Player.League,
                    SenderName = Player.Name,
                    SenderLevel = Player.ExpLevel,
                    SenderRole = 0,
                });
            }
            else 
                Logging.Error(this.GetType(), "Unable to request to join the alliance. The alliance is null");
        }
    }
}
