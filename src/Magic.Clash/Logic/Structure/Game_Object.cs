﻿using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Game_Object
    {
        public const int Width = 48;
        public const int Height = 48;

        public Game_Object(Data data, Level level, bool component = true)
        {
            Level = level;
            Data = data;
            ComponentsActive = component;
            if (component)
            {
                this.Components = new List<Component>();
                for (var i = 0; i < 12; i++)
                {
                    this.Components.Add(new Component());
                }
            }
        }

        internal List<Component> Components;
        internal Data Data { get; }
        internal Level Level { get; }

        internal virtual bool Builder => false;
        internal virtual int ClassId => -1;
        internal virtual int OppositeClassId => -1;

        internal int GlobalId;
        internal int X
        {
            get => _x;
            set
            {
                if (value < 0 || value > Width)
                {
                    if (ClassId != 15 && ClassId != 8)
                        throw new ArgumentOutOfRangeException(nameof(value),
                        "value must be non-negative and Game_Object.Width.");
                }
                _x = value;
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (value < 0 || value > Height)
                {
                    if (ClassId != 15 && ClassId != 8)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "value must be non-negative and Game_Object.Height.");
                }

                _y = value;
            }
        }

        private int _x;

        private int _y;

        internal bool ComponentsActive;

        public int[] GetPosition() => new[] {X, Y};
            
        public void AddComponent(Component component)
        {
            if (Components[component.Type].Type == -1)
            {
                Level.GetComponentManager().AddComponent(component);
                Components[component.Type] = component;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (ComponentsActive)
            {
                if (test || Components[index].IsEnabled)
                    result = Components[index];
            }
            return result;
        }

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();


            if (ComponentsActive)
            {
                foreach (Component c in Components)
                    c.Load(jsonObject);
            }
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);

            if (ComponentsActive)
            {
                foreach (Component c in Components)
                    c.Save(jsonObject);
            }
            return jsonObject;
        }

        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public virtual void Tick()
        {
            if (ComponentsActive)
            {
                foreach (var comp in Components)
                {
                    if (comp.IsEnabled)
                        comp.Tick();
                }
            }
        }
    }
}
