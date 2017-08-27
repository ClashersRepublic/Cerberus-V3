using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network
{
    internal class Debug
    {
        internal Device Device;
        internal string[] Parameters;

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
            new Global_Chat_Entry(Device)
            {
                Message = message,
                Message_Sender = Device.Player.Avatar,
                Bot = true
            }.Send();
        }
    }
}