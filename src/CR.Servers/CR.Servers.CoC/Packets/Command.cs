using System;
using System.Collections.Generic;
using System.Reflection;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets
{
    internal class Command
    {
        internal int ExecuteSubTick = -1;
        internal virtual int Type => 0;
        internal bool IsServerCommand => false;

        internal Reader Reader;
        internal Device Device;
        

        public Command(Device Device)
        {
            this.Device = Device;
        }

        public Command(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal virtual void Decode()
        {
        }

        internal virtual void Encode(List<byte> Data)
        {
        }

        internal virtual void Execute()
        {
        }
        
        internal virtual void Load(JToken Token)
        {
        }

        internal virtual JObject Save()
        {
            return null;
        }
        
        internal JObject SaveBase()
        {
            return new JObject
            {
                {
                    "t", this.ExecuteSubTick
                }
            };
        }

        internal void ShowBuffer()
        {
            Logging.Info(this.GetType(), BitConverter.ToString(this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
        }

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