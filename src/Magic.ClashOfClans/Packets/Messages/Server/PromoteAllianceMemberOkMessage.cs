using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Client;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class PromoteAllianceMemberOkMessage : Message
    {
        internal long Id;
        internal int Role;

        public PromoteAllianceMemberOkMessage(PacketProcessing.Client client)
            : base(client)
        {
            MessageType = 24306;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt64(Id);
            pack.AddInt32(Role);
            Encrypt(pack.ToArray());
        }
        public void SetID(long id)
        {
            Id = id;
        }

        public void SetRole(int role)
        {
            Role = role;
        }
    }
}