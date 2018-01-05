namespace CR.Servers.CoC.Packets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Commands.Client.Battle;
    using CR.Servers.Extensions;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Extensions.List;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(CommandConverter))]
    public class Command
    {
        internal Device Device;
        internal int ExecuteSubTick = -1;

        internal Reader Reader;

        public Command()
        {
        }

        public Command(Device Device)
        {
            this.Device = Device;
        }

        public Command(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal virtual int Type => 0;
        internal virtual bool IsServerCommand => false;

        internal virtual void Decode()
        {
            this.ExecuteSubTick = this.Reader.ReadInt32();
        }

        internal virtual void Encode(List<byte> Data)
        {
            Data.AddInt(this.ExecuteSubTick);
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
            Logging.Info(this.GetType(), BitConverter.ToString(this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
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

        internal void Log()
        {
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\Dumps\\" + $"{this.GetType().Name} ({this.Type}) - {DateTime.Now:yy_MM_dd__hh_mm_ss}.bin", this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position)));
        }
    }

    internal class CommandConverter : JsonConverter
    {
        public override bool CanWrite => true;

        public override bool CanConvert(Type Type)
        {
            return Type.BaseType == typeof(Command) || Type == typeof(Command);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject Token = JObject.Load(reader);

            if (JsonHelper.GetJsonNumber(Token, "ct", out int Type))
            {
                Command Entry;

                switch (Type)
                {
                    case 700:
                        Entry = new Place_Attacker();
                        break;
                    default:
                        Entry = new Command();
                        break;
                }

                Entry.Load(Token);

                return Entry;
            }
            Logging.Error(this.GetType(), "ReadJson() - JsonObject doesn't contains 'ct' key.");

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Command Command = (Command) value;

            if (Command != null)
            {
                Command.Save().WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}