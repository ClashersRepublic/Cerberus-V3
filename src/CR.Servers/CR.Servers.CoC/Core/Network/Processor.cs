using System;
using System.Collections.Concurrent;
using System.Threading;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets;

namespace CR.Servers.CoC.Core.Network
{
    internal class Processor
    {
        private int _incomingIndex;
        private int _outgoingIndex;

        private IncomingProcessorThread[] _incomingThreads;
        private OutgoingProcessorThread[] _outgoingThreads;

        internal Processor()
        {
            /*
            _incomingIndex = -1;
            _outgoingIndex = -1;
            */

            _incomingThreads = new IncomingProcessorThread[Environment.ProcessorCount];
            _outgoingThreads = new OutgoingProcessorThread[Environment.ProcessorCount];

            /* Initialize the processor threads. */
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                _incomingThreads[i] = new IncomingProcessorThread();
                _outgoingThreads[i] = new OutgoingProcessorThread();
            }

            /* Start the processor threads. */
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                _incomingThreads[i].Start();
                _outgoingThreads[i].Start();
            }
        }

        public IncomingProcessorThread[] IncomingThreads => _incomingThreads;
        public OutgoingProcessorThread[] OutgoingThreads => _outgoingThreads;

        /* Enqueues a message we're sending on a processing thread. */
        public void EnqueueOutgoing(Message message)
        {
            /*
            Interlocked.CompareExchange(ref _outgoingIndex, -1, _outgoingThreads.Length - 1);
            var index = Interlocked.Increment(ref _outgoingIndex);
            */

            lock (_outgoingThreads)
            {
                var processor = _outgoingThreads[_outgoingIndex];
                processor._queue.Enqueue(message);

                if (++_outgoingIndex > _outgoingThreads.Length - 1)
                    Interlocked.Exchange(ref _outgoingIndex, 0);
            }
        }

        /* Enqueues a message we've received on a processing thread. */
        public void EnqueueIncoming(Message message)
        {
            /*
            Interlocked.CompareExchange(ref _outgoingIndex, -1, _outgoingThreads.Length - 1);
            var index = Interlocked.Increment(ref _incomingIndex);
            */

            lock (_incomingThreads)
            {
                var processor = _incomingThreads[_incomingIndex];
                processor._queue.Enqueue(message);

                if (++_incomingIndex > _incomingThreads.Length - 1)
                    Interlocked.Exchange(ref _incomingIndex, 0);
            }
        }

        /* Represents a thread which processes incoming messages. */
        public class IncomingProcessorThread
        {
            private readonly Thread _thread;
            internal readonly ConcurrentQueue<Message> _queue;

            public IncomingProcessorThread()
            {
                _thread = new Thread(Process);
                _queue = new ConcurrentQueue<Message>();
            }

            public int Count => _queue.Count;

            public void Start()
            {
                _thread.Start();
            }

            private void Process()
            {
                try
                {
                    while (true)
                    {
                        try
                        {
                            Message message;
                            while (_queue.TryDequeue(out message))
                            {
                                try
                                { message.Decode(); }
                                catch (Exception ex)
                                { Logging.Error(message.GetType(), "Exception while decoding message: " + ex); }

                                try
                                { message.Process(); }
                                catch (Exception ex)
                                { Logging.Error(message.GetType(), "Exception while processing message: " + ex); }

                                message.Timer.Stop();
                            }

                            Thread.Sleep(1);
                        }
                        catch (Exception ex)
                        {
                            Logging.Error(typeof(IncomingProcessorThread), "Exception inside incoming processing queue: " + ex);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    // Space
                }
            }
        }

        /* Represents a thread which processes outgoing messages. */
        public class OutgoingProcessorThread
        {
            private readonly Thread _thread;
            internal readonly ConcurrentQueue<Message> _queue;

            public OutgoingProcessorThread()
            {
                _thread = new Thread(Process);
                _queue = new ConcurrentQueue<Message>();
            }

            public int Count => _queue.Count;

            public void Start()
            {
                _thread.Start();
            }

            private void Process()
            {
                try
                {
                    while (true)
                    {
                        try
                        {
                            Message message;
                            while (_queue.TryDequeue(out message))
                            {
                                Device device = message.Device;

                                try
                                { message.Encode(); }
                                catch (Exception ex)
                                { Logging.Error(message.GetType(), "Exception while encoding message: " + ex); }

                                try
                                { message.Process(); }
                                catch (Exception ex)
                                { Logging.Error(message.GetType(), "Exception while processing incoming message: " + ex); }

                                byte[] messageBytes = message.Data.ToArray();

                                if (device.SendEncrypter != null)
                                    messageBytes = device.SendEncrypter.Encrypt(messageBytes);

                                byte[] packet = new byte[messageBytes.Length + 7];

                                /* Write header. */
                                packet[1] = (byte)message.Type;
                                packet[0] = (byte)(message.Type >> 8);

                                packet[4] = (byte)messageBytes.Length;
                                packet[3] = (byte)(messageBytes.Length >> 8);
                                packet[2] = (byte)(messageBytes.Length >> 16);

                                packet[6] = (byte)message.Version;
                                packet[5] = (byte)(message.Version >> 8);

                                /* Write body. */
                                Array.Copy(messageBytes, 0, packet, 7, messageBytes.Length);

                                Resources.Gateway.Send(packet, message.Device.Token);

                                /*
                                Logging.Info(this.GetType(), "Message " + message.GetType().Name + " sent.");
                                */
                            }

                            Thread.Sleep(1);
                        }
                        catch (Exception ex)
                        {
                            Logging.Error(typeof(OutgoingProcessorThread), "Exception inside outgoing processing queue: " + ex);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    // Space
                }
            }
        }
    }
}