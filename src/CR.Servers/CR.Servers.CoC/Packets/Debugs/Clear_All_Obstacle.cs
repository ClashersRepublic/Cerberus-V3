using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Debugs
{
    internal class Clear_All_Obstacle : Debug
    {
        internal override Rank RequiredRank => Rank.Player;

        public Clear_All_Obstacle(Device Device, params string[] Parameters) : base(Device, Parameters)
        {

        }

        internal override void Process()
        {
            var GameObjectManager = this.Device.GameMode.Level.GameObjectManager;
            GameObjectManager.GameObjects[3][0].Clear();
            GameObjectManager.GameObjects[3][1].Clear();
            GameObjectManager.ObstaclesIndex[0] = 0;
            GameObjectManager.ObstaclesIndex[1] = 0;

            if (this.Device.Connected)
            {
                new Own_Home_Data(this.Device).Send();
            }
        }
    }
}
