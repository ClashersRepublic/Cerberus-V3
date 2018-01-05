namespace CR.Servers.CoC.Logic.Map
{
    using System.Collections.Generic;

    internal class Tile
    {
        internal List<GameObject> GameObjects;

        public Tile()
        {
            this.GameObjects = new List<GameObject>(4);
        }

        internal void AddGameObject(GameObject GameObject)
        {
            if (this.GameObjects.Contains(GameObject))
            {
                return;
            }

            this.GameObjects.Add(GameObject);
        }

        internal bool IsBuildable()
        {
            return this.GameObjects.Count == 0;
        }

        internal bool IsBuildable(GameObject GameObject)
        {
            if (this.GameObjects.Count > 0)
            {
                if (this.GameObjects.Count == 1)
                {
                    return this.GameObjects[0] == GameObject;
                }

                return false;
            }

            return true;
        }

        internal void RemoveGameObject(GameObject GameObject)
        {
            this.GameObjects.Remove(GameObject);
        }
    }
}