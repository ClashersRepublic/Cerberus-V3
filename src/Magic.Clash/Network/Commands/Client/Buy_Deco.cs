using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        internal int[] XY;

        public Buy_Deco(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            this.XY = new[] { this.Reader.ReadInt32(), this.Reader.ReadInt32() };
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        public override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var bd = (Decos)CSV.Tables.Get(Gamefile.Decos).GetDataWithID(BuildingId);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Deco(bd, this.Device.Player);

                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalId(), bd.GetBuildCost()))
                {
                    ca.Resources.Minus(bd.GetGlobalId(), bd.GetBuildCost());
                    b.SetPosition(XY[0], XY[1]);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
            else
            {
                var b = new Builder_Deco(bd, this.Device.Player);
                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalId(), bd.GetBuildCost()))
                {
                    ca.Resources.Minus(bd.GetGlobalId(), bd.GetBuildCost());
                    b.SetPosition(XY[0], XY[1]);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
        }
    }
}