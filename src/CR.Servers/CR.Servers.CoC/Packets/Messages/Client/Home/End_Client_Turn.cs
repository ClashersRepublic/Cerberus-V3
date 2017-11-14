using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    internal class End_Client_Turn : Message
    {
        internal int SubTick;
        internal int Checksum;
        internal int CommandCount;

        internal List<Command> Commands;

        internal override short Type => 14102;

        public End_Client_Turn(Device Device, Reader Reader) : base(Device, Reader)
        {
            // End_Client_Turn.
        }


        internal override void Decode()
        {
            this.SubTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.CommandCount = this.Reader.ReadInt32();

            if (this.CommandCount <= 512)
            {
                if (this.CommandCount > 0)
                {
                    this.Commands = new List<Command>(this.CommandCount);

                    for (int i = 0; i < this.CommandCount; i++)
                    {
                        var CommandID = Reader.ReadInt32();
                        if (Factory.Commands.ContainsKey(CommandID))
                        {
                            var Command = Factory.CreateCommand(CommandID, Device, Reader);

                            if (Command != null)
                            {
                                Command.Decode();

                                this.Commands.Add(Command);
                            }
                            else
                            {
                                this.CommandCount = this.Commands.Count;
                                break;
                            }
                        }
                        else
                        {
                            this.CommandCount = this.Commands.Count;
                            //Dowhatitsupposedtodo
                            Logging.Error(this.GetType(), "Command is not handled! (" + CommandID + ")");
                            break;
                        }
                    }
                }
            }
            else
                Logging.Error(this.GetType(), "Command count is too high! (" + this.CommandCount + ")");

        }

        internal override void Process()
        {
            if (this.CommandCount > 0)
            {
                do
                {

                    //TODO: Tick stuff
                    Command Command = this.Commands[0];

                    Command.Execute();

                    this.Commands.Remove(Command);
                    continue;
                } while (this.Commands.Count > 0);
            }
        }
    }
}
