using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;

namespace CR.Servers.CoC.Logic
{
    internal class Trap : GameObject
    {


        private int UpgradeLevel;
        internal Timer ConstructionTimer;
        internal TrapData TrapData => (TrapData)this.Data;

        internal override int HeightInTiles => this.TrapData.Height;

        internal override int WidthInTiles => this.TrapData.Width;

        internal override int Type => 0;

        internal override int VillageType => this.TrapData.VillageType;

        internal int RemainingConstructionTime => ConstructionTimer?.GetRemainingSeconds(this.Level.Time) ?? 0;

        internal bool Constructing => this.ConstructionTimer != null;

        internal bool UpgradeAvailable
        {
            get
            {
                if (!this.Constructing)
                {
                    var Data = this.TrapData;

                    if (Data.MaxLevel > this.UpgradeLevel)
                    {
                        if (this.Level.Player.Village2)
                        {
                            return this.Level.GameObjectManager.TownHall2.GetUpgradeLevel() + 1 >= Data.TownHallLevel[this.UpgradeLevel + 1];
                        }
                        return this.Level.GameObjectManager.TownHall.GetUpgradeLevel() + 1 >=  Data.TownHallLevel[this.UpgradeLevel + 1];
                    }
                }

                return false;
            }
        }

        internal int GetUpgradeLevel() => this.UpgradeLevel;

        public Trap(Data Data, Level Level) : base(Data, Level)
        {
        }
    }
}
