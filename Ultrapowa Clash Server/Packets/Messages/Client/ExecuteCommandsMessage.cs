using System;
using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14102
    internal class ExecuteCommandsMessage : Message
    {
        public ExecuteCommandsMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public uint Checksum;
        public byte[] NestedCommands;
        public uint NumberOfCommands;
        public uint Subtick;

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                Subtick = br.ReadUInt32WithEndian();
                Checksum = br.ReadUInt32WithEndian();
                NumberOfCommands = br.ReadUInt32WithEndian();

                if (NumberOfCommands > 0 && NumberOfCommands < 120)
                    NestedCommands = br.ReadBytes(GetLength() - 12);
            }
        }

        public override void Process(Level level)
        {
            level.Tick();

            if (NumberOfCommands > 0 && NumberOfCommands < 120)
            {
                using (var br = new PacketReader(new MemoryStream(NestedCommands)))
                {
                    for (var i = 0; i < NumberOfCommands; i++)
                    {
                        var cmd = (Command)CommandFactory.Read(br, 0);
                        if (cmd != null)
                        {
                            try { cmd.Execute(level); }
                            catch (Exception ex)
                            {
                                Logger.Error($"Exception while processing command {cmd.GetType()}: " + ex);
                            }
                        }
                        else break;
                    }
                }
            }
        }
    }
}
