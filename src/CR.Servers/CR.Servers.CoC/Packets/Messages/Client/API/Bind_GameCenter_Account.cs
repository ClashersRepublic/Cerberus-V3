using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.API
{
    internal class Bind_GameCenter_Account : Message
    {
        internal override short Type => 14212;

        public Bind_GameCenter_Account(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal bool Force;
        internal string GameCenterId;
        internal string Url;
        internal string BundleId;

        internal byte[] Signature;
        internal byte[] Salt;
        internal byte[] Timestamp;

        internal override void Decode()
        {
            this.Force = this.Reader.ReadBoolean();

            this.GameCenterId = this.Reader.ReadString();
            this.Url = this.Reader.ReadString();
            this.BundleId = this.Reader.ReadString();

            this.Signature = this.Reader.ReadArray();
            this.Salt = this.Reader.ReadArray();
            this.Timestamp = this.Reader.ReadArray();
        }

        internal override void Process()
        {
            /*Player Bounded = await Resources.Players.FindPlayer(T => !(T.HighID == this.Device.GameMode.Level.Player.HighID && T.LowID == this.Device.GameMode.Level.Player.LowID) && T.GameCenterID == this.GameCenterId);

            if (Bounded != null)
            {
                new GameCenter_Account_Already_Bound_Message(this.Device, Bounded).Send();
                return;
            }
            
            Mongo.Players.UpdateOne(T => T.HighId == this.Device.GameMode.Level.Player.HighID && T.LowId == this.Device.GameMode.Level.Player.LowID, Builders<Players>.Update.Set(T => T.GameCenterId, this.GameCenterId));
      
            new GameCenter_Account_Bound_Message(this.Device, 1).Send();*/
        }
    }
}
