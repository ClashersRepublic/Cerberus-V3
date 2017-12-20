using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Commands.Client.List;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Layout_Building_Position : Command
    {
        internal override int Type => 598;

        public Layout_Building_Position(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
        
        internal List<BuildingToMove> Buildings;

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            var buildingCount = this.Reader.ReadInt32();
            Buildings = new List<BuildingToMove>(buildingCount);

            for (var i = 0; i < buildingCount; i++)
            {
                Buildings.Add(new BuildingToMove
                {
                    X = this.Reader.ReadInt32(),
                    Y = this.Reader.ReadInt32(),
                    Id = this.Reader.ReadInt32()
                });
            }
            base.Decode();
        }
    }
}
