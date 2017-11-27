using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Chat_To_Alliance : Message
    {
        internal override short Type => 14315;

        public Chat_To_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal string Message;

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {
            var Level = this.Device.GameMode.Level;
            if (Level.Player.InAlliance)
            {
                if (!string.IsNullOrWhiteSpace(this.Message))
                {
                    if (this.Message.Length <= 128)
                    {
                        this.Message = Resources.Regex.Replace(this.Message, " ");

                        if (this.Message.StartsWith(" "))
                        {
                            this.Message = this.Message.Remove(0, 1);
                        }

                        if (this.Message.Length > 0)
                        {
                            Level.Player.Alliance.Streams.AddEntry(
                                new ChatStreamEntry(this.Device.GameMode.Level.Player.AllianceMember)
                                {
                                    Message = this.Message
                                });
                        }
                    }
                }
            }
        }
    }
}
