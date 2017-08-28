using System;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Add_Global_Message : Message
    {
        internal string Message = string.Empty;

        public Add_Global_Message(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            Message = Reader.ReadString();
        }

        public override void Process()
        {
            if (Message.StartsWith(DebugFactory.Delimiter))
            {
                var command = Message.Remove(0, 1);
                var commands = command.Split(' ');
                if (commands.Length > 0)
                {
                    var commandName = commands[0];
                    if (DebugFactory.Debugs.ContainsKey(commandName))
                    {
                        var args = commands.Skip(1).ToArray();
                        var Debug = Activator.CreateInstance(DebugFactory.Debugs[commandName], Device, args) as Debug;

                        if (Debug != null)
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"Debug Command {commandName} has  been handled.");
                            Console.ResetColor();
#endif

                            try
                            {
                                Debug.Process();
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogger.Log(ex,
                                    $"Exception while processing incoming debug request {Debug.GetType()}");
                            }
                        }
                    }
                    else
                    {
                        SendChatMessage($"Unknown command '{commandName}'. Type '/help' for more information.");
                    }
                }
            }
            else
            {
                var onlinePlayers = ResourcesManager.OnlinePlayers;
                //Parallel.ForEach(onlinePlayers, t =>
                    foreach (var t in onlinePlayers.ToArray())
                {
                    if (t != null)
                    {
                        new Global_Chat_Entry(t.Device)
                        {
                            Message = Message,
                            Message_Sender = Device.Player.Avatar,
                            Regex = false,
                            Sender = Device == t.Device
                        }.Send();
                    }
                }//);
            }
        }
    }
}
