using System;
using System.Collections.Generic;
using System.Reflection;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network
{
    internal class Message
    {
        // NOTE: Maybe make disposable.


        public Message()
        {
            // Space
        }

        public Message(Device device)
        {
            Device = device;
            Reader = null;
            Data = new List<byte>();
        }

        public Message(Device device, Reader reader)
        {
            Device = device;
            Reader = reader;
            Data = null;
        }

        public Device Device { get; set; }
        public ushort Identifier { get; set; }
        public ushort Version { get; set; }

        public Reader Reader;
        public List<byte> Data;
        public int Length;

        public virtual void Decode()
        {
            // Space
        }

        public virtual void Encode()
        {
            // Space
        }

        public virtual void Process()
        {
            // Space
        }

        public void Decrypt()
        {
            try
            {
                if (Constants.IsRc4)
                {
                    var buffer = Reader.ReadBytes(Length);
                    if (Identifier != 10100)
                        Device.Decrypt(buffer);

                    Reader = new Reader(buffer);
                    Length = buffer.Length;
                }
            }
            catch
            {
                Device.State = State.DISCONNECTED;
                throw;
            }
        }

        public void Encrypt()
        {
            try
            {
                if (Constants.IsRc4)
                    if (this.Device.State > State.SESSION_OK)
                        Device.Encrypt(Data);
                Length = Data.Count;
            }
            catch (Exception)
            {
                Device.State = State.DISCONNECTED;
                throw;
            }
        }

        public byte[] ToBytes()
        {
            var encodedMessage = new List<byte>(7 + Length);
            encodedMessage.AddUShort(Identifier);
            encodedMessage.AddUInt24((uint)Length);
            encodedMessage.AddUShort(Version);

            if (Data == null)
            {
                Logger.Error("Data was null when getting raw data of message.");
                throw new ArgumentNullException();
            }

            encodedMessage.AddRange(Data);
            return encodedMessage.ToArray();
        }

        internal void Debug()
        {
            Console.WriteLine(GetType().Name + " : " +
                              BitConverter.ToString(
                                  Reader.ReadBytes(
                                      (int) (Reader.BaseStream.Length - Reader.BaseStream.Position))));
        }

        internal void SendChatMessage(string message)
        {
            new Global_Chat_Entry(this.Device)
            {
                Message = message,
                Message_Sender = Device.Player.Avatar,
                Bot = true
            }.Send();
        }

        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (var Field in GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                if (Field != null)
                    Logger.SayInfo(Utils.Padding(GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " +
                                   Utils.Padding(
                                       !string.IsNullOrEmpty(Field.Name)
                                           ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)")
                                           : "(null)", 40));
        }
    }
}
