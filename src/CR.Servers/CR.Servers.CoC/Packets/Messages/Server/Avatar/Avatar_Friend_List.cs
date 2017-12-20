using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Friend_List : Message
    {
        internal override short Type => 20105;

        public Avatar_Friend_List(Device Device) : base(Device)
        {
            
        }

        internal List<Player> Friends;

        internal override void Encode()
        {
           // this.Data.AddHexa("000000000000000300000042013180860100000042013180860000000431384352ffffffffffffffffffffffff000000000000004900000008000006ce000001c40000000300010c49010000002900312823690015550000000e42524156452050415348544f4f4e0000000100000008000000002300d148cf010000002300d148cf0000000a41726a6f206a656765670000000f313431353038373836333233373334ffffffffffffffff0000000000000058000000080000065c0000034f000000040235cad1000000000b00cd7dec010000000b00cd7dec000000054c49504f4effffffffffffffffffffffff00003685000000920000000f00000c54000009b3000000020000004601000000590016933a640019500000000d467269656e642773204b696e67000000020000000700");
            this.Data.AddInt(0);
            this.Data.AddInt(this.Friends.Count);

            foreach (var Friend in this.Friends)
            {
                this.Data.AddLong(1);
                this.Data.AddBool(true);
                this.Data.AddLong(1);
                this.Data.AddString(Friend.Name);
                this.Data.AddString(null);
                this.Data.AddString(null);
                this.Data.AddString(null);
                this.Data.AddInt(0); //Protection Time
                this.Data.AddInt(Friend.ExpLevel);
                this.Data.AddInt(Friend.League);
                this.Data.AddInt(Friend.Score);
                this.Data.AddInt(Friend.DuelScore);
                this.Data.AddInt(4); //2 = Request Send, 3 = Want to be friend, 4 = friend 
                this.Data.AddInt(0);

                if (Friend.InAlliance)
                {
                    this.Data.AddBool(true);
                    this.Data.AddLong(Friend.AllianceId);
                    this.Data.AddInt(Friend.Alliance.Header.Badge);
                    this.Data.AddString(Friend.Alliance.Header.Name);
                    this.Data.AddInt((int)Friend.AllianceMember.Role);
                    this.Data.AddInt(Friend.Alliance.Header.ExpLevel);
                }
                this.Data.AddBool(false);
                //this.Data.AddHexa("ffffffffffffffffffffffff000000000000004900000008000006ce000001c40000000300010c27010000002900312823690015550000000e42524156452050415348544f4f4e000000010000000800");
            }
        }

        internal override void Process()
        {
        }
    }
}
