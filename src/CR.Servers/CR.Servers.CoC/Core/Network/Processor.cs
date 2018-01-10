namespace CR.Servers.CoC.Core.Network
{
    using CR.Servers.CoC.Packets;

    internal static class Processor
    {
        internal static void Send(this Message message)
        {
            if (message.Device.Connected)
            {
                Resources.PacketManager.SendMessageQueue.Enqueue(message);
            }
        }
    }
}