using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    internal class Send_Global_Chat : Message
    {
        internal override short Type => 14715;

        public Send_Global_Chat(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal string Message;

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (this.Device.Chat != null)
            {
                //if (DateTime.UtcNow != this.Device.LastGlobalChatEntry)
                {
                    if (!string.IsNullOrEmpty(this.Message))
                    {
                        if (this.Message.StartsWith(Factory.Delimiter.ToString()))
                        {
                            Debug Debug = Factory.CreateDebug(this.Message, this.Device, out string CommandName);

                            new Global_Chat_Line(this.Device, this.Device.GameMode.Level.Player) { Message = this.Message}.Send();

                            if (Debug != null)
                            {
                                Debug.Decode();
                                Debug.Process();
                            }
                            else
                                this.SendChatMessage($"Unknown command '{CommandName}'. Type '/help' for more information.");

                            return;
                        }

                        if (this.Message.Length <= 128)
                        {
                            this.Message = Resources.Regex.Replace(this.Message, " ");

                            if (this.Message.Length > 0)
                            {
                                this.Device.Chat.AddEntry(this.Device, this.Message);
                            }
                        }
                    }
                }
                
                this.Device.LastGlobalChatEntry = DateTime.Now;
            }
        }
    }
}