using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.StreamEntries;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Commands.Client
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
