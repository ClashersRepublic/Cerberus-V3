using System.Collections.Generic;
using System.Linq;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Core.Consoles.Colorful;

namespace CR.Servers.CoC.Logic.Chat
{
    internal class Chat
    {
        internal List<Device> Devices;

        internal object Gate = new object();

        public Chat()
        {
            this.Devices = new List<Device>(50);
        }

        internal bool TryAdd(Device Device)
        {
            lock (this.Gate)
            {
                if (this.Devices.Count < 50)
                {
                    this.Devices.Add(Device);

                    return true;
                }
            }

            return false;
        }
        internal void Quit(Device Device)
        {
            lock (this.Gate)
            {
                this.Devices.Remove(Device);
            }
        }

        internal void AddEntry(Device Device, string Message)
        {
            Device[] Devices = this.Devices.ToArray();

            if (Devices.Contains(Device))
            {
                if (!string.IsNullOrEmpty(Message))
                {
                    foreach (Device Device2 in Devices.SkipWhile(T=> T == Device))
                    {
                        Console.WriteLine(Device2 == Device);
                        if (Device2.Connected)
                        {
                            new Global_Chat_Line(Device2, Device.GameMode.Level.Player){Message = Message}.Send();
                        }
                        else
                            this.Quit(Device2);
                    }

                    /*if (Device.Connected)
                    {
                        new Global_Chat_Line(Device, Device.GameMode.Level.Player) { Message = Message, Name = "You"}.Send();
                    }
                    else
                        this.Quit(Device);*/
                }
            }
        }
    }
}
