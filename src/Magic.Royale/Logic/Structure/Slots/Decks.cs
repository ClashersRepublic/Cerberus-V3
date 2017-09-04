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
    }
}
