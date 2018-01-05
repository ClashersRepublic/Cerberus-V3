using System;
using System.Linq;
using System.Reflection;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic.Mode;
using CR.Servers.CoC.Packets;
using CR.Servers.CoC.Packets.Stream;
using CR.Servers.Extensions;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Logic
{
    public class Device : IDisposable
    {
        internal Account Account;
        internal Chat.Chat Chat;

        internal bool Disposed;
        internal bool UseRC4;

        internal int EncryptionSeed;

        internal GameMode GameMode;
        internal DeviceInfo Info;
        internal DateTime LastKeepAlive;

        internal StreamEncrypter ReceiveEncrypter;
        internal StreamEncrypter SendEncrypter;
  
        internal Token Token;

        internal State State = State.DISCONNECTED;

        internal Device()
        {
            GameMode = new GameMode(this);
            LastKeepAlive = DateTime.UtcNow;
        }
        
        internal int Checksum
        {
            get
            {
                var Checksum = 0;

                Checksum += GameMode.Time.SubTick;
                Checksum += GameMode.Time.TotalSecs;

                if (GameMode.Level.Player != null)
                {
                    Checksum += GameMode.Level.Player.Checksum;

                    if (GameMode.Level.GameObjectManager != null)
                        Checksum += GameMode.Level.GameObjectManager.Checksum;
                }

                // Visitor

                Checksum += Checksum;

                return Checksum;
            }
        }

        internal long TimeSinceLastKeepAliveMs => (long) DateTime.UtcNow.Subtract(LastKeepAlive).TotalMilliseconds;

        internal bool Connected => !Disposed && Token.Connected;

        internal string OS => Info.Android ? "Android" : "iOS";

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                State = State.DISCONNECTED;

                Chat?.Quit(this);

                if (Account != null)
                {
                    if (Account.Player != null)
                    {
                        Resources.Accounts.SavePlayer(Account.Player);

                        if (GameMode?.Level != null && Account.Player.BattleIdV2 > 0)
                        {
                            Resources.BattlesV2.Dequeue(GameMode.Level);
                        }

                        Account.Player.Alliance?.DecrementTotalConnected();
                    }

                    if (Account.Home != null)
                    {
                        Resources.Accounts.SaveHome(Account.Home);
                    }
                }

                if (GameMode?.CommandManager != null)
                {
                    if (GameMode.CommandManager.ServerCommands != null)
                    {
                        foreach (Command Command in GameMode.CommandManager.ServerCommands.Values.ToArray())
                        {
                            Command.Execute();
                        }
                    }
                    else
                    {
                        Logging.Error(GetType(), "CommandManager != null but ServerCommands == null");
                    }
                }
            }
        }

        internal void Process(byte[] buffer)
        {
            if (State != State.DISCONNECTED)
            {
                if (buffer.Length >= 7)
                {
                    var messageType = buffer[1] | (buffer[0] << 8);
                    var messageLength = buffer[4] | (buffer[3] << 8) | (buffer[2] << 16);
                    var messageVersion = buffer[6] | (buffer[5] << 8);

                    if (messageLength < 0x800000)
                    {
                        if (buffer.Length - 7 >= messageLength)
                        {
                            var messageBytes = new byte[messageLength];
                            Array.Copy(buffer, 7, messageBytes, 0, messageLength);

                            if (ReceiveEncrypter != null)
                            {
                                messageBytes = ReceiveEncrypter.Decrypt(messageBytes);
                            }
                            else
                            {
                                if (messageType == 10101)
                                {
                                    UseRC4 = true;
                                    InitRC4Encrypters(Factory.RC4Key, "nonce");
                                    messageBytes = ReceiveEncrypter.Decrypt(messageBytes);
                                }
                            }

                            //Console.WriteLine("Message " + messageType);

                            if (messageBytes != null)
                            {
                                var message = Factory.CreateMessage((short) messageType, this, new Reader(messageBytes));

                                if (message != null)
                                {
                                    message.Version = (short) messageVersion;

                                    try
                                    {
                                        message.Decode();
                                        message.Process();
                                    }
                                    catch (Exception exception)
                                    {
                                        Logging.Error(GetType(),
                                            "An error has been throwed when the handle of message type " + messageType + ", length: " + messageLength + ". trace: " + exception);
                                    }
                                }
                                else
                                {
                                    Logging.Info(GetType(), "Message type " + messageType + " not exist.");
                                }
                            }
                            else
                            {
                                Logging.Error(GetType(), "Unable to decrypt message type " + messageType + ". Encrypter: " + ReceiveEncrypter + ".");
                            }

                            Token.Packet.RemoveRange(0, messageLength + 7);

                            if (buffer.Length - 7 - messageLength >= 7)
                            {
                                var nextPacket = new byte[buffer.Length - 7 - messageLength];
                                Array.Copy(buffer, messageLength + 7, nextPacket, 0, nextPacket.Length);
                                Process(nextPacket);
                            }
                        }
                    }
                    else
                    {
                        Resources.Gateway.Disconnect(this.Token.Args);
                    }
                }
            }
            else
            {
                Resources.Gateway.Disconnect(this.Token.Args);
            }
        }

        internal void InitRC4Encrypters(string key, string nonce)
        {
            this.ReceiveEncrypter = new RC4Encrypter(key, nonce);
            this.SendEncrypter = new RC4Encrypter(key, nonce);
        }

        internal void SetEncrypters(StreamEncrypter rcvEncrypter, StreamEncrypter sndEncrypter)
        {
            ReceiveEncrypter = rcvEncrypter;
            SendEncrypter = sndEncrypter;
        }

        internal struct DeviceInfo
        {
            internal bool Android;
            internal bool Advertising;

            internal string UDID;
            internal string OpenUDID;
            internal string AndroidID;
            internal string DeviceModel;
            internal string ADID;
            internal string OSVersion;
            internal string PreferredLanguage;
            internal string[] ClientVersion;

            internal int Locale;

            /*
            internal void ShowValues()
            {
                foreach (var Field in GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    if (Field != null)
                        Logging.Info(GetType(),
                            ConsolePad.Padding(Field.Name) + " : " +
                            ConsolePad.Padding(!string.IsNullOrEmpty(Field.Name) ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)") : "(null)", 40));
            }
            */
        }
    }
}