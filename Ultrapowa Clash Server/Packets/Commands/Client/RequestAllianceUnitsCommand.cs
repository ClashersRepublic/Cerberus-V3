using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class RequestAllianceUnitsCommand : Command
    {
        public byte FlagHasRequestMessage;
        public string Message;
        public int MessageLength;
        public string Message2;

        public RequestAllianceUnitsCommand(PacketReader br)
        {
            br.ReadUInt32WithEndian();
            FlagHasRequestMessage = br.ReadByte();
            Message = br.ReadString();
            Message2 = br.ReadString();
        }

        public override void Execute(Level level)
        {
        }
    }
}
