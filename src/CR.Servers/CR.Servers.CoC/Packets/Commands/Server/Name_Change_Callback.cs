using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Name_Change_Callback : ServerCommand
    {


        internal override int Type => 3;

        public Name_Change_Callback(Device Client) : base(Client)
        {
        }

        public Name_Change_Callback(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal string AvatarName;
        internal int ChangeNameCount;

        internal override void Decode()
        {
            this.AvatarName = Reader.ReadString();
            this.ChangeNameCount = Reader.ReadInt32();
            base.Decode();

        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddString(this.AvatarName);
            Data.AddInt(this.ChangeNameCount);
            base.Encode(Data);
        }

        internal override void Execute()
        {
            ShowValues();
            var Level = this.Device.GameMode.Level;
            if (this.AvatarName != null)
            {
                Level.Player.Name = this.AvatarName;
                Level.Player.ChangeNameCount = this.ChangeNameCount;

                if (Level.Player.NameSetByUser)
                {
                    Level.Player.ChangeNameCount++;
                }
                else
                    Level.Player.NameSetByUser = true;
            }
            
        }
    }
}
