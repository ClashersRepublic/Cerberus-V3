using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Servers.CoC.Logic
{
    internal class ModSlot : DataSlots
    {        
        public ModSlot() : base()
        {

        }

        internal bool SelfAttack
        {
            get => this.GetCountByGlobalId(0) == 1;
            set => this.Set(0, value ? 1 : 0);
        }

        internal bool AIAttack;
        internal Level AILevel;

        internal void Initialize()
        {
            this.Set(0, 0);
        }

    }
}
