using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Free_Worker : Command
    {
        internal override int Type => 521;

        public Free_Worker(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int TimeLeft;
        internal int VillageWorker;
        internal bool EmbedCommand;
        internal Command Command;

        internal override void Decode()
        {
            this.TimeLeft = this.Reader.ReadInt32();
            this.VillageWorker = this.Reader.ReadInt32();
            this.EmbedCommand = this.Reader.ReadBoolean();

            if (this.EmbedCommand)
            {
                var CommandID = this.Reader.ReadInt32();

                if (Factory.Commands.ContainsKey(CommandID))
                {
                    Command = Factory.CreateCommand(CommandID, this.Device, this.Reader);

                    if (Command != null)
                    {
                        Command.Decode();
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Embded Command is not handled! (" + CommandID + ")");
                    this.Reader.BaseStream.Position = this.Reader.BaseStream.Position - (4 + 1 + 4 + 4);
                    this.Log();
                }
            }
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            if (this.VillageWorker == 0)
            {
                if (Level.WorkerManager.FreeWorkers == 0)
                {
                    Level.WorkerManager.FinishTaskOfOneWorker();
                }
                else
                {
                    Logging.Error(this.GetType(), "Free worker called even when there is free worker!");
                }

            }
            else
            {
                if (Level.WorkerManagerV2.FreeWorkers == 0)
                {
                    Level.WorkerManagerV2.FinishTaskOfOneWorker();
                }
                else
                {
                    Logging.Error(this.GetType(), "Free worker called even when there is free worker!");
                }
            }
                
            if (this.Command != null)
            {
                try
                {
                    this.Command.Execute();
                    Logging.Error(this.GetType(), "Embded Command is handled! (" + Command.Type + ")");
                }
                catch (Exception Exception)
                {
                    Logging.Error(this.GetType(),
                        "An error has been throwed when the process of command type " + this.Command.Type + ". " +
                        Exception);
                }
            }
        }
    }
}
