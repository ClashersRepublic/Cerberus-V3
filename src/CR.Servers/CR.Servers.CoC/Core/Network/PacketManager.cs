namespace CR.Servers.CoC.Core.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets;

    internal class PacketManager
    {
        internal ConcurrentQueue<Message> ReceiveMessageQueue;

        internal Thread ReceiveThread;
        internal ConcurrentQueue<Message> SendMessageQueue;
        internal Thread SendThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PacketManager" /> class.
        /// </summary>
        internal PacketManager()
        {
            this.SendMessageQueue = new ConcurrentQueue<Message>();
            this.ReceiveMessageQueue = new ConcurrentQueue<Message>();

            this.ReceiveThread = new Thread(this.HandleMessageTask)
            {
                Priority = ThreadPriority.AboveNormal
            };

            this.SendThread = new Thread(this.SendMessageTask)
            {
                Priority = ThreadPriority.AboveNormal
            };

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
                var message = (Message)null;
                while (this.ReceiveMessageQueue.TryDequeue(out message))
                {
                    this.HandleMessage(message);
                }

                Thread.Sleep(1);
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

                    if (message.Timer.ElapsedMilliseconds >= TimeSpan.FromSeconds(0.5).TotalMilliseconds)
                    {
                        Console.WriteLine("Message " + message.GetType().Name + $" received ({message.Timer.ElapsedMilliseconds}).");
                    }
                    //Logging.Info(this.GetType(), "Message " + message.GetType().Name + $" received ({message.Timer.ElapsedMilliseconds}).");
                    message.Timer.Stop();
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
                var message = (Message)null;
                while (SendMessageQueue.TryDequeue(out message))               
                {
                    this.SendMessage(message);
                }

                Thread.Sleep(1);
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

                    byte[] packet = new byte[messageBytes.Length + 7];

                    packet[1] = (byte) message.Type;
                    packet[0] = (byte) (message.Type >> 8);

                    packet[4] = (byte) messageBytes.Length;
                    packet[3] = (byte) (messageBytes.Length >> 8);
                    packet[2] = (byte) (messageBytes.Length >> 16);

                    packet[6] = (byte) message.Version;
                    packet[5] = (byte) (message.Version >> 8);

                    Array.Copy(messageBytes, 0, packet, 7, messageBytes.Length);
                    Resources.Gateway.Send(packet, message.Device.Token);

                    message.Process();

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