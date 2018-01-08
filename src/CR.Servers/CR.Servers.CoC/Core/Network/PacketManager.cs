namespace CR.Servers.CoC.Core.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets;

    internal class PacketManager
    {
        internal ConcurrentQueue<Message> SendMessageQueue;
        internal ConcurrentQueue<Message> ReceiveMessageQueue;

        internal Thread ReceiveThread;
        internal Thread SendThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PacketManager"/> class.
        /// </summary>
        internal PacketManager()
        {
            this.SendMessageQueue = new ConcurrentQueue<Message>();
            this.ReceiveMessageQueue = new ConcurrentQueue<Message>();

            this.ReceiveThread = new Thread(this.HandleMessageTask);
            this.SendThread = new Thread(this.SendMessageTask);

            this.ReceiveThread.Start();
            this.SendThread.Start();
        }

        /// <summary>
        ///     Task for the handle message thread.
        /// </summary>
        private void HandleMessageTask()
        {
            while (true)
            {
                if (this.ReceiveMessageQueue.TryDequeue(out Message message))
                {
                    this.HandleMessage(message);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        private void HandleMessage(Message message)
        {
            Device device = message.Device;

            if (device.Connected)
            {
                try
                {
                    message.Decode();
                    message.Process();

                    Logging.Info(this.GetType(), "Message " + message.GetType().Name + " received.");
                }
                catch (Exception exception)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the handle of message type " + message.Type + ". trace: " + exception);
                }
            }
        }

        /// <summary>
        ///     Task for the send message thread.
        /// </summary>
        private void SendMessageTask()
        {
            while (true)
            {
                if (this.SendMessageQueue.TryDequeue(out Message message))
                {
                    this.SendMessage(message);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        ///     Sends the specified message.
        /// </summary>
        private void SendMessage(Message message)
        {
            Device device = message.Device;

            if (device.Connected)
            {
                try
                {
                    message.Encode();

                    byte[] messageBytes = message.Data.ToArray();

                    if (device.SendEncrypter != null)
                    {
                        messageBytes = device.SendEncrypter.Encrypt(messageBytes);
                    }

                    message.Process();

                    byte[] packet = new byte[messageBytes.Length + 7];

                    packet[1] = (byte) (message.Type);
                    packet[0] = (byte) (message.Type >> 8);

                    packet[4] = (byte) (messageBytes.Length);
                    packet[3] = (byte) (messageBytes.Length >> 8);
                    packet[2] = (byte) (messageBytes.Length >> 16);

                    packet[6] = (byte) (message.Version);
                    packet[5] = (byte) (message.Version >> 8);

                    Array.Copy(messageBytes, 0, packet, 7, messageBytes.Length);
                    Resources.Gateway.Send(packet, message.Device.Token.Socket);

                    Logging.Info(this.GetType(), "Message " + message.GetType().Name + " sent.");
                }
                catch (Exception exception)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the handle of message type " + message.Type + ". trace: " + exception);
                }
            }
        }
    }
}