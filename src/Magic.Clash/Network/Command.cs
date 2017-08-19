using System;
using System.Collections.Generic;
using System.Reflection;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network
{
    internal class Command
    {
        internal const int MaxEmbeddedDepth = 10;

        internal int Identifier;

        internal Reader Reader;
        internal Device Device;

        internal List<byte> Data;

        public Command(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        public Command(Reader Reader, Device Device, int Identifier)
        {
            this.Identifier = Identifier;
            this.Device = Device;
            this.Reader = Reader;
        }

        public virtual void Decode()
        {
            // Decode.
        }

        public virtual void Encode()
        {
            // Encode.
        }

        public virtual void Process()
        {
            // Process.
        }

        internal void Debug()
        {
            Console.WriteLine(Utils.Padding(this.GetType().Name, 15) + " : " + BitConverter.ToString(this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
        }


        internal void ShowValues()
        {
            //Console.WriteLine(Environment.NewLine);

            foreach (FieldInfo Field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Field != null)
                {
                    Logger.SayInfo(Utils.Padding(this.GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " + Utils.Padding(!string.IsNullOrEmpty(Field.Name) ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)") : "(null)", 40));
                }
            }
        }
    }
}
