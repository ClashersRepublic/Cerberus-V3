namespace CR.Servers.CoC.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Extensions;
    using CR.Servers.Extensions.Binary;
    using System.Threading.Tasks;

    public abstract class Message
    {
        private static readonly Task s_completedTask = Task.FromResult<object>(null);

        internal Stopwatch Timer;
        internal List<byte> Data;

        internal Device Device;
        internal int Length;

        internal int Offset;

        internal Reader Reader;
        internal short Version;

        internal Message(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        internal Message(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal abstract short Type { get; }

        internal virtual void Decode()
        {
            //Trace.WriteLine("[*] " + this.GetType().Name + " : " + "Decoding.");
        }

        internal virtual void Encode()
        {
            //Trace.WriteLine("[*] " + this.GetType().Name + " : " + "Encoding.");
        }

        internal virtual void Process()
        {
            //Trace.WriteLine("[*] " + this.GetType().Name + " : " + "Processing.");
        }

        internal virtual Task ProcessAsync()
        {
            return s_completedTask;
        }

        internal virtual void SendChatMessage(string message)
        {
            new GlobalChatLineMessage(this.Device, this.Device.GameMode.Level.Player)
            {
                Message = message,
                Name = "[System] Command Manager",
                ExpLevel = 300,
                League = 22,
                Bot = true
            }.Send();
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

        internal void Send()
        {
            if (Device.Connected)
                Device.EnqueueOutgoingMessage(this);
        }

        internal void Log()
        {
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\Dumps\\" + $"{this.GetType().Name} ({this.Type}) - UserId ({(this.Device.GameMode?.Level?.Player != null ? this.Device.GameMode.Level.Player.HighID + "-" + this.Device.GameMode.Level.Player.LowID : "-")}) - {DateTime.Now:yy_MM_dd__hh_mm_ss}.bin", this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position)));
        }
    }
}