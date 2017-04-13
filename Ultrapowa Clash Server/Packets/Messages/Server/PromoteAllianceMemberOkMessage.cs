using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Client;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class PromoteAllianceMemberOkMessage : Message
    {
        internal long Id;
        internal int Role;

        public PromoteAllianceMemberOkMessage(PacketProcessing.Client client)
            : base(client)
        {
            SetMessageType(24306);
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