using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Diamonds_Added : ServerCommand
    {
        internal override int Type => 7;

        public Diamonds_Added(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        public Diamonds_Added(Device Device) : base(Device)
        {

        }

        internal int Count;
        internal bool AlliangeGift;

        //internal BillingPackageData BillingPackageData;
        //internal GemBundleData GemBundleData;

        internal override void Decode()
        {
            this.AlliangeGift = Reader.ReadBoolean();

            this.Count = Reader.ReadInt32();

            Reader.ReadInt32();
            Reader.ReadInt32();
            /*this.BillingPackageData = Reader.ReadData<BillingPackageData>();
            this.GemBundleData = Reader.ReadData<GemBundleData>();*/

            Reader.ReadBoolean();

            Reader.ReadString(); //TransactionID

            base.Decode();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddBool(this.AlliangeGift);

            Data.AddInt(this.Count);
            Data.AddInt(0);
            Data.AddInt(0);
            /*this.Data.AddData(this.BillingPackageData);
            this.Data.AddData(this.GemBundleData);*/
            Data.AddBool(this.AlliangeGift);

            Data.AddString(null); // TransactionID

            base.Encode(Data);
        }

        internal override void Execute()
        {
            ShowValues();
            this.Device.GameMode.Level.Player.AddDiamonds(this.Count);
        }

    }
}
