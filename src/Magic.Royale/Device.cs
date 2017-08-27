#define Info
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using Magic.Royale.Core;
using Magic.Royale.Core.Crypto;
using Magic.Royale.Core.Settings;
using Magic.Royale.Extensions;
using Magic.Royale.Extensions.Binary;
using Magic.Royale.Logic;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Network;
using Magic.Royale.Network.Messages.Server;

namespace Magic.Royale
{
    internal partial class Device
    {
        internal readonly KeepAliveOkMessage _keepAliveOk;

        public Device(Socket socket)
        {
            Socket = socket;
            _socketHandle = socket.Handle.ToInt64();
            DataStream = new List<byte>();
            _keepAliveOk = new KeepAliveOkMessage(this);
            DataStream = new List<byte>(Constants.BufferSize);

            State = State.DISCONNECTED;

            IncomingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, IncomingPacketsKey, Key._RC4_EndecryptKey.Length);

            OutgoingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, OutgoingPacketsKey, Key._RC4_EndecryptKey.Length);

            LastKeepAlive = DateTime.Now;
            NextKeepAlive = LastKeepAlive.AddSeconds(30);
        }

        private readonly long _socketHandle;

        public State State { get; set; }

        public List<byte> DataStream { get; }

        public Socket Socket { get; }

        public Avatar Player { get; set; }

        public long GetSocketHandle()
        {
            return _socketHandle;
        }
        internal int Depth { get; set; }
        internal int Last_Command { get; set; }

        internal int Major { get; set; }
        internal int Minor { get; set; }
        internal int Revision { get; set; }

        internal uint ClientSeed { get; set; }
        internal string AndroidID { get; set; }
        internal string OpenUDID { get; set; }
        internal string Model { get; set; }
        internal string OSVersion { get; set; }
        internal string MACAddress { get; set; }
        internal string AdvertiseID { get; set; }
        internal string VendorID { get; set; }
        internal string IPAddress { get; set; }
        internal bool Android { get; set; }
        internal bool Advertising { get; set; }

        public byte[] IncomingPacketsKey { get; set; }
        public byte[] OutgoingPacketsKey { get; set; }

        public DateTime NextKeepAlive { get; set; }
        public DateTime LastKeepAlive { get; set; }

        public void Process()
        {
            const int HEADER_LEN = 7;

            var result = false;
            if (DataStream.Count >= 5)
                using (Reader Reader = new Reader(DataStream.ToArray()))
                {
                    ushort Identifier = Reader.ReadUInt16();
                    int Length = Reader.ReadInt24();
                    ushort Version = Reader.ReadUInt16();

                    if (DataStream.Count - HEADER_LEN >= Length)
                    {
                        var message = MessageFactory.Read(this, Reader, Identifier);
                        if (message != null)
                        {
                            message.Identifier = Identifier; // Just in case they don't do it in the constructor.
                            message.Length = Length;
                            message.Version = Version;

#if Info
                            Logger.SayInfo("[MESSAGE] " + message.Device.Socket.RemoteEndPoint + " --> " + message.GetType().Name + " [" + message.Identifier + "]");
#endif
                            try
                            {
                                message.Decrypt();
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogger.Log(ex, $"Unable to decrypt message with ID: {Identifier}");
                            }

                            try
                            {
                                message.Decode();
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogger.Log(ex, $"Unable to decode message with ID: {Identifier}");
                            }

                            try { message.Process(); }
                            catch (Exception ex)
                            {
                                ExceptionLogger.Log(ex, $"Exception while processing incoming message {message.GetType()}");
                            }
                        }
                        else
                        {
                            Logger.Say("Unhandled message " + Identifier);

                            // Make sure we don't break the stream.
                            SNonce.Increment();
                        }

                        // Clean up. 
                        DataStream.RemoveRange(0, HEADER_LEN + Length);
                        if (DataStream.Count >= 7)
                        {
                            this.Process();
                        }
                    }
                }
        }
    }
}
