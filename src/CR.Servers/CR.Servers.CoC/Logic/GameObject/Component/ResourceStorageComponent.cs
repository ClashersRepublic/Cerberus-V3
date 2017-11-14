using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Servers.CoC.Logic
{
    internal class ResourceStorageComponent : Component
    {
        internal int[] MaxArray;
        internal override int Type => 6;

        public ResourceStorageComponent(GameObject GameObject) : base(GameObject)
        {
            // ResourceStorageComponent.
        }

        internal int GetMax(int ResourceID)
        {
            if (ResourceID > this.MaxArray.Length)
            {
                //Logging.Info(this.GetType(), "Index is out of range : " + ResourceID);
                return 0;
            }

            return this.MaxArray[ResourceID];
        }

        internal void SetMaxArray(int[] MaxArray)
        {
            this.MaxArray = MaxArray;
        }
    }
}