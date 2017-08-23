using System;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Free_Worker : Command
    {
        internal int Time_Left;
        internal int Unknown1;
        internal bool EmbedCommands;

        public Free_Worker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Time_Left = Reader.ReadInt32();
            Unknown1 = Reader.ReadInt32();
            EmbedCommands = Reader.ReadBoolean();

            if (EmbedCommands)
            {
                Device.Depth++;
                if (Device.Depth >= MaxEmbeddedDepth)
                {
                    new Out_Of_Sync(Device).Send();
                }
            }
            else
            {
                Device.Depth--;
            }
        }

        public override void Process()
        {
            if (Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                if (Device.Player.BuilderWorkerManager.GetFreeWorkers() == 0)
                    return;
                Device.Player.BuilderWorkerManager.FinishTaskOfOneWorker();
            }
            else
            {
                if (Device.Player.VillageWorkerManager.GetFreeWorkers() == 0)
                    return;
                Device.Player.VillageWorkerManager.FinishTaskOfOneWorker();
            }

            Device.Depth = 0;
            if (EmbedCommands)
            {
                var CommandID = Reader.ReadInt32();
                if (CommandFactory.Commands.ContainsKey(CommandID))
                {
                    var Command =
                        Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, Device,
                            CommandID) as Command;

                    if (Command != null)
                    {
#if DEBUG
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Command " + CommandID + " has  been handled.");
                        Console.ResetColor();
#endif
                        Command.Decode();
                        Command.Process();
                    }
                }
            }
        }
    }
}
