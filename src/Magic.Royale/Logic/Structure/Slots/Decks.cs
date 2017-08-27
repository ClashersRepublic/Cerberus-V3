using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Magic.Royale.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Decks : List<Deck>
    {
        public const int DECK_COUNT = 5;
        [JsonIgnore] public Avatar Player;

        [JsonIgnore]  public bool Initialized;

        public Decks()
        {
            
        }

        public Decks(Avatar Player)
        {
            this.Player = Player;
        }
    }
}
