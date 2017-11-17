using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Diamonds_Added : Command
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

            Reader.ReadInt32();

            Reader.ReadString();

            ExecuteSubTick = Reader.ReadInt32();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddBool(this.AlliangeGift);

            Data.AddInt(this.Count);
            Data.AddInt(0);
            Data.AddInt(0);
            /*this.Data.AddData(this.BillingPackageData);
            this.Data.AddData(this.GemBundleData);*/
            Data.AddInt(this.AlliangeGift ? 1 : 0);

            Data.AddString(null); // TransactionID

            Data.AddInt(ExecuteSubTick);
        }

        internal override void Execute()
        {
            this.Device.GameMode.Level.Player.AddDiamonds(this.Count);
        }

    }
}
