﻿namespace CR.Servers.CoC.Logic
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Files.CSV_Helpers;
    using Newtonsoft.Json.Linq;

    internal class GameObject
    {
        internal List<Component> Components;
        internal Data Data;

        internal int Id;
        internal Level Level;
        internal Vector2 Position;

        public GameObject(Data Data, Level Level)
        {
            this.Level = Level;
            this.Data = Data;
            this.Position = new Vector2(0, 0);
            this.Components = new List<Component>(16);
        }

        internal int TileX
        {
            get
            {
                return this.Position.X >> 9;
            }
        }

        internal int TileY
        {
            get
            {
                return this.Position.Y >> 9;
            }
        }

        internal int MidX
        {
            get
            {
                return this.Position.X >> 8;
            }
        }

        internal int MidY
        {
            get
            {
                return this.Position.Y >> 8;
            }
        }

        internal virtual int Checksum
        {
            get
            {
                int Checksum = 0;

                Checksum += this.Type;

                return Checksum;
            }
        }

        internal virtual int HeightInTiles
        {
            get
            {
                return 1;
            }
        }

        internal virtual int WidthInTiles
        {
            get
            {
                return 1;
            }
        }

        internal virtual int VillageType
        {
            get
            {
                return 0;
            }
        }

        internal virtual int Type
        {
            get
            {
                return 0;
            }
        }

        internal void AddComponent(Component Component)
        {
            if (this.Components.Contains(Component))
            {
                Logging.Error(this.GetType(), "AddComponent() : Trying to add a component already added. Type : " + Component.Type + ".");
                return;
            }

            this.Components.Add(Component);

            if (!this.Level.AI)
            {
                this.Level.ComponentManager.AddComponent(Component);
            }
        }

        internal void SetPositionXY(int TileX, int TileY)
        {
            int OldX = this.Position.X >> 9;
            int OldY = this.Position.Y >> 9;

            if (OldX == TileX && OldY == TileX)
            {
                return;
            }

            this.Position.X = TileX << 9;
            this.Position.Y = TileY << 9;

            this.Level.TileMap.GameObjectMoved(this, OldX, OldY);
        }

        internal bool TryGetComponent(int Type, out Component Component)
        {
            Component = this.Components.Find(T => T.Type == Type);
            return Component != null;
        }

        internal virtual void FastForwardTime(int Secs)
        {
            this.Components.ForEach(Component => { Component.FastForwardTime(Secs); });
        }

        internal virtual void Tick()
        {
            this.Components.ForEach(Component => { Component.Tick(); });
        }

        internal virtual void Process()
        {
        }

        internal virtual void Load(JToken Json)
        {
            if (JsonHelper.GetJsonNumber(Json, "x", out int X) && JsonHelper.GetJsonNumber(Json, "y", out int Y))
            {
                this.Position.X = X << 9;
                this.Position.Y = Y << 9;
            }
            else
            {
                Logging.Error(this.GetType(), "An error has been throwed when the load of game object. Position X or Y is null!");
            }

            this.Components.ForEach(Component => { Component.Load(Json); });
        }

        internal virtual void Save(JObject Json)
        {
            Json.Add("x", this.TileX);
            Json.Add("y", this.TileY);

            this.Components.ForEach(Component => { Component.Save(Json); });
        }
    }
}