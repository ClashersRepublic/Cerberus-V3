using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Mode;
using CR.Servers.CoC.Packets;
using CR.Servers.CoC.Packets.Cryptography;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions;
using CR.Servers.Extensions.Binary;
using CR.Servers.Library;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Logic
{
    public class Device : IDisposable
    {
        internal Chat.Chat Chat;
        internal Token Token;
        internal Socket Socket;
        internal DeviceInfo Info;
        internal GameMode GameMode;
        internal DateTime LastGlobalChatEntry;
        internal DateTime LastKeepAlive;
        internal DateTime LastMessage;
        internal DateTime Session;
        internal readonly Keep_Alive_Ok KeepAliveOk;

        internal Account Account;

        internal IEncrypter ReceiveDecrypter;
        internal IEncrypter SendEncrypter;
        // internal PepperState PepperState;

        internal State State = State.DISCONNECTED;

        internal bool Disposed;

        internal uint Seed;

        internal int Checksum
        {
            get
            {
                int Checksum = 0;

                Checksum += this.GameMode.Time.SubTick; 
                Checksum += this.GameMode.Time.TotalSecs; 

                if (this.GameMode.Level.Player != null)
                {
                    Checksum += this.GameMode.Level.Player.Checksum;

                    if (this.GameMode.Level.GameObjectManager != null)
                    {
                        Checksum += this.GameMode.Level.GameObjectManager.Checksum;
                    }
                }

                // Visitor

                Checksum += Checksum;

                return Checksum;
            }
        }

        internal long TimeSinceLastKeepAliveMs => (long)DateTime.UtcNow.Subtract(this.LastKeepAlive).TotalMilliseconds;


        internal Device(Socket Socket)
        {
            this.Socket = Socket;
            this.GameMode = new GameMode(this);
            this.KeepAliveOk = new Keep_Alive_Ok(this);
            this.LastKeepAlive = DateTime.UtcNow;
        }

        internal Device(Socket Socket, Token Token) : this(Socket)
        {
            this.Token = Token;
        }

        internal bool Connected
        {
            get
            {
                if (this.Socket.Connected)
                {
                    try
                    {
                        if (!this.Socket.Poll(1000, SelectMode.SelectRead) || this.Socket.Available != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        internal string OS => this.Info.Android ? "Android" : "iOS";
        internal void Process(byte[] Buffer)
        {
            if (this.State != State.DISCONNECTED)
            {
                if (Buffer.Length >= 7 && Buffer.Length <= Constants.ReceiveBuffer)
                {
                    using (Reader Reader = new Reader(Buffer))
                    {
                        short Identifier = Reader.ReadInt16();
                        int Length = Reader.ReadInt24();
                        short Version = Reader.ReadInt16();

                        if (Buffer.Length - 7 >= Length)
                        {
                            if (this.ReceiveDecrypter == null)
                            {
                                this.InitializeEncrypter(Identifier);
                            }

                            byte[] Packet = this.ReceiveDecrypter.Decrypt(Reader.ReadBytes(Length));

                            Message Message = Factory.CreateMessage(Identifier, this, new Reader(Packet));

                            if (Message != null)
                            {
                                Message.Length = Length;
                                Message.Version = Version;

                                //Message.Reader = new Reader(Packet);

                                Logging.Info(this.GetType(),
                                    "Packet " + ConsolePad.Padding(Message.GetType().Name) + " received from " +
                                    this.Socket.RemoteEndPoint + ".");

                                try
                                {
                                    Message.Decode();
                                    Message.Process();
                                }
                                catch (Exception Exception)
                                {
                                    Logging.Error(this.GetType(), Exception.GetType().Name + " when handling the following message : ID " + Identifier + ", Length " + Length + ", Version " + Version + ".");
                                    Logging.Error(Exception.GetType(), Exception.Message + " [" +  (this.GameMode?.Level?.Player != null ? this.GameMode.Level.Player.HighID + ":" + this.GameMode.Level.Player.LowID  : "-:-") + ']' + Environment.NewLine + Exception.StackTrace);
                                }
                            }
                            else
                            {
                                File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\Logs\\" + $"Unknown Message ({Identifier}) - UserId ({(this.GameMode?.Level?.Player != null ? this.GameMode.Level.Player.HighID + "-" + this.GameMode.Level.Player.LowID : "-")}) - {DateTime.Now:yy_MM_dd__hh_mm_ss}}}.bin", Packet);

                            }

                            if (!this.Token.Aborting)
                            {
                                this.Token.Packet.RemoveRange(0, Length + 7);

                                if (Buffer.Length - 7 - Length >= 7)
                                {
                                    this.Process(Reader.ReadBytes(Buffer.Length - 7 - Length));
                                }
                            }
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "The received buffer length is inferior the header length.");
                        }
                    }
                }
                else
                {
                    Resources.Gateway.Disconnect(this.Token.Args);
                }
            }
            else
            {
                if (this.Connected)
                {
                    Resources.Gateway.Disconnect(this.Token.Args);
                }
            }
        }
        internal void InitializeEncrypter(int FirstMessageType)
        {
            //if (FirstMessageType == 10100)
            {
                //Pepper
            }
            //else if (FirstMessageType == 10101)
            {
                this.ReceiveDecrypter = new RC4Encrypter(Factory.RC4Key, "nonce");
                this.SendEncrypter = new RC4Encrypter(Factory.RC4Key, "nonce");
            }
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Disposed = true;
                this.State = State.DISCONNECTED;

                this.Chat?.Quit(this);

                if (this.Account != null)
                {
                    if (this.Account.Player != null)
                    {
                        Resources.Accounts.SavePlayer(this.Account.Player);

                        if (this.GameMode?.Level != null && this.Account.Player.BattleIdV2 > 0)
                        {
                            Resources.BattlesV2.Dequeue(this.GameMode.Level);
                        }

                        this.Account.Player.Alliance?.DecrementTotalConnected();
                    }
                    if (this.Account.Home != null)
                    {
                        Resources.Accounts.SaveHome(this.Account.Home);
                    }
                }

                if (this.GameMode?.CommandManager != null)
                {
                    foreach (Command Command in this.GameMode.CommandManager.ServerCommands.Values.ToArray())
                    {
                        Command.Execute();
                    }
                }

                this.Token = null;

                this.Socket.Dispose();
            }
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

            internal void ShowValues()
            {
                foreach (FieldInfo Field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (Field != null)
                    {
                        Logging.Info(this.GetType(), ConsolePad.Padding(Field.Name) + " : " + ConsolePad.Padding(!string.IsNullOrEmpty(Field.Name) ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)") : "(null)", 40));
                    }
                }
            }
        }
    }
}
