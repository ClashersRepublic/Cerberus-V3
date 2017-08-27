using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Magic.Royale.Core;
using Magic.Royale.Extensions;
using Magic.Royale.Extensions.Binary;
using Magic.Royale.Extensions.List;
using Magic.Royale.Extensions.Sodium;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Network
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

        public virtual void Decrypt()
        {
            if (Device.State >= State.LOGGED)
            {
                Device.SNonce.Increment();

                var buffer = Reader.ReadBytes(Length);
                buffer = Sodium.Decrypt(new byte[16].Concat(buffer).ToArray(), Device.SNonce,
                    Device.PublicKey);

                if (buffer != null)
                {
                    Reader = new Reader(buffer);
                    Length = buffer.Length;
                }
                else
                {
                    Length = 0;
                }
            }
        }

        public virtual void Encrypt()
        {
            if (Device.State >= State.LOGGED)
            {
                Device.RNonce.Increment();

                Data = new List<byte>(
                    Sodium.Encrypt(Data.ToArray(), Device.RNonce, Device.PublicKey).Skip(16).ToArray());
            }

            Length = Data.Count;
        }

        public byte[] ToBytes()
        {
            var encodedMessage = new List<byte>(7 + Length);
            encodedMessage.AddUShort(Identifier);
            encodedMessage.AddUInt24((uint) Length);
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
            /*new Global_Chat_Entry(this.Device)
            {
                Message = message,
                Message_Sender = this.Device.Player.Avatar,
                Bot = true
            }.Send();*/
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
