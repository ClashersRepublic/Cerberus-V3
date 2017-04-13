using System;
using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class BuyShieldCommand : Command
    {
        public int ShieldId;
        public int Tick;

        public BuyShieldCommand(PacketReader br)
        {
            ShieldId = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            int time = avatar.GetShieldTime + Convert.ToInt32(TimeSpan.FromHours((double)((ShieldData)CSVManager.DataTables.GetDataById(ShieldId)).TimeH).TotalSeconds);
            avatar.SetShieldTime(time);
            int diamonds = ((ShieldData) CSVManager.DataTables.GetDataById(ShieldId)).Diamonds;
            avatar.UseDiamonds(diamonds);
        }
    }
}
