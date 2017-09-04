using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Buy_Deco : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal int X;
        internal int Y;

        public Buy_Deco(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            X = Reader.ReadInt32();
            Y = Reader.ReadInt32();
            BuildingId = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;
            var bd = (Decos) CSV.Tables.Get(Gamefile.Decos).GetDataWithID(BuildingId);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Deco(bd, Device.Player);

                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalId(), bd.GetBuildCost()))
                {
                    ca.Resources.Minus(bd.GetGlobalId(), bd.GetBuildCost());
                    b.SetPosition(X, Y);
                    Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
            else
            {
                var b = new Builder_Deco(bd, Device.Player);
                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalId(), bd.GetBuildCost()))
                {
                    ca.Resources.Minus(bd.GetGlobalId(), bd.GetBuildCost());
                    b.SetPosition(X, Y);
                    Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
        }
    }
}