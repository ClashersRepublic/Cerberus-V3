using Magic.Logic;
using Magic.Network.Messages.Components;

namespace Magic.Network.Messages.Server
{
    /// <summary>
    /// Description of OwnHomeDataMessage.
    /// </summary>
    public class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        public OwnHomeDataMessage()
        {
            // Space
        }

        public long UserId;

        public int Unknown1;
        public int Unknown2;

        //public int Unknown3;
        //public int Unknown4;
        //public int Timestamp;
        public Timer UnknownTimer1;

        public int Unknown5;
        public int Unknown6;

        public int[] BattleDeck;
        public int[] Deck;
        public int[] Deck2;

        public byte Unknown7;

        public CardMessageComponent[] BattleCards;
        public CardMessageComponent[] UnusedCards;

        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;

        public string ChallengeName;

        public int Unknown16;
        public ChallengeMessageComponent[] Challenges;

        public int Unknown17;
        public int Unknown18;

        public string UnknownJsonData;

        public int Unknown20;
        public int Unknown21;
        public int Unknown22;
        public ChestMessageComponent[] Chests;

        //public int FreeChestRemainingSeconds;
        //public int FreeChestTotalSeconds;
        //public int FreeChestNextDate;
        public Timer FreeChestTimer;

        //public int Unknown23;
        //public int Unknown24;
        //public int Unknown25;
        public Timer UnknownTimer2;

        public int Unknown26;
        public ChestMessageComponent[] FreeChests;

        public int Unknown27;
        public int Unknown28;
        public int Unknown29;

        public int CrownCount;
        public int CrownChestUnlocked;

        //public int CrownChestNextSeconds;
        //public int CrownChestTotalTime;
        //public int CrownChestTimestamp;
        public Timer CrownChestTimer;

        //public int CrownChestSomethingSeconds;
        //public int CrownChestTotalTime2;
        //public int CrownChestTimestamp2;
        public Timer CrownChestTimer2;

        public int Unknown30;

        //public int Unknown31;
        //public int Unknown32;
        //public int Unknown33;
        public Timer UnknownTimer3;

        public int Unknown53;
        public int Unknown54;
        public int Unknown55;

        public int ExpLevel;
        public int ArenaType;
        public int ArenaId;

        public int Unknown34;

        public int DayOfWeek;
        public int DayOfWeek2;

        //public int ShopNewCardTimeSeconds;
        //public int ShopNewCardTimeSeconds2;
        //public int ShopNewCardTimestamp;
        public Timer ShopTimer;

        public int Unknown35;
        public ShopItemMessageComponent[] ShopItems;

        //public int Unknown36;
        //public int Unknown37;
        //public int Unknown38;
        public Timer UnknownTimer4;

        //public int Unknown39;
        //public int Unknown40;
        //public int Unknown41;
        public Timer UnknownTimer5;

        //public int Unknown42;
        //public int Unknown43;
        //public int Unknown44;
        public Timer UnknownTimer6;

        public int Unknown45;
        public int Unknown46;
        public int Unknown47;

        public int Unknown48;
        public int[] UnknownCharacterArray;

        public int Unknown49;
        public int Unknown50;
        public int Unknown51;
        public byte Unknown52;

        public int Unknown56;
        public UnknownMessageComponent[] UnknownComponent1;

        public int Unknown57;
        public int Unknown58;

        public AvatarMessageComponent Avatar;
        private int Unknown59;
        private int Unknown60;
        private int Unknown61;
        private int Unknown62;
        private int Unknown63;
        private int Unknown64;
        private int Unknown65;
        private int Unknown66;
        private int Unknown67;
        private int Unknown68;
        private int Unknown69;
        private int Unknown70;
        private int Unknown71;
        private int Unknown72;
        private int Unknown73;
        private int Unknown74;
        private int Unknown75;

        /// <summary>
        /// Gets the ID of the <see cref="OwnHomeDataMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24101; } }

        /// <summary>
        /// Reads the <see cref="OwnHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();

            Unknown1 = reader.ReadRRInt32(); // 32
            Unknown2 = reader.ReadRRInt32(); // 4

            //Unknown3 = reader.ReadRRInt32(); // 1516340
            //Unknown4 = reader.ReadRRInt32(); // 1526040
            //Timestamp = reader.ReadRRInt32();

            UnknownTimer1.ReadMessageComponent(reader);

            Unknown5 = reader.ReadRRInt32(); // 0
            Unknown6 = reader.ReadRRInt32(); // 3

            var count1 = reader.ReadRRInt32();
            BattleDeck = new int[count1]; // Not too sure about functionality.
            for (int i = 0; i < count1; i++)
                BattleDeck[i] = reader.ReadRRInt32();

            var count2 = reader.ReadRRInt32();
            Deck = new int[count1]; // Not too sure about functionality.
            for (int i = 0; i < count2; i++)
                Deck[i] = reader.ReadRRInt32();

            var count3 = reader.ReadRRInt32();
            Deck2 = new int[count1]; // Not too sure about functionality.
            for (int i = 0; i < count3; i++)
                Deck2[i] = reader.ReadRRInt32();

            Unknown7 = reader.ReadByte(); // 255

            //TODO: Switch to DeckMessageComponent.
            // Active cards.
            BattleCards = new CardMessageComponent[8];
            for (int i = 0; i < 8; i++)
            {
                var card = new CardMessageComponent();
                card.ReadMessageComponent(reader);

                BattleCards[i] = card;
            }

            // Inactive cards.
            var count4 = reader.ReadRRInt32();
            UnusedCards = new CardMessageComponent[count4];
            for (int i = 0; i < count4; i++)
            {
                var card = new CardMessageComponent();
                card.ReadMessageComponent(reader);

                UnusedCards[i] = card;
            }

            // Timer?
            // Unknown8 and Unknown9 seems to come in pair
            // could be a long or something.
            Unknown8 = reader.ReadInt32(); // 127

            // This is a timer probs, therefore Unknown8 is incorrect.
            Unknown9 = reader.ReadRRInt32(); // 63
            Unknown10 = reader.ReadRRInt32(); // 1491827316, Some form of timestamp.

            // Could be a boolean.
            Unknown11 = reader.ReadRRInt32(); // 1
            Unknown12 = reader.ReadRRInt32(); // 0
            Unknown13 = reader.ReadRRInt32(); // 0
            Unknown14 = reader.ReadRRInt32(); // 0

            ChallengeName = reader.ReadString(); // Challenge name?

            Unknown16 = reader.ReadRRInt32(); // Array size?
            Challenges = new ChallengeMessageComponent[Unknown16];
            for (int i = 0; i < Unknown16; i++)
            {
                var challenge = new ChallengeMessageComponent();
                challenge.ReadMessageComponent(reader);

                Challenges[i] = challenge;
            }

            // Could be a byte.
            Unknown17 = reader.ReadRRInt32(); // 63

            //TODO: Might need some rework here.
            // Could be an array's size prefix.
            Unknown18 = reader.ReadInt32();
            if (Unknown18 == 0)
            {
                var unknown1_ = reader.ReadRRInt32();
            }
            else if (Unknown18 == 2)
            {
                // Could be 2 shorts.
                var unknown2_ = reader.ReadInt32();
            }

            UnknownJsonData = reader.ReadString(); // JSON: {"ID":"CARD_RELEASE","Params":{"Assassin":"20170324"}}
            Unknown20 = reader.ReadRRInt32(); // 0
            Unknown21 = reader.ReadRRInt32(); // 4, Array Size?

            // Chests
            Unknown22 = reader.ReadRRInt32(); // 1 -- *CRITICAL* Can crash the client.
            if (Unknown22 == 1)
            {
                Chests = new ChestMessageComponent[4];
                for (int i = 0; i < 4; i++)
                {
                    var chest = new ChestMessageComponent();
                    chest.ReadMessageComponent(reader);

                    Chests[i] = chest;

                    // Not too sure about this one but seems consistent.
                    if (chest.Unknown6 == 0)
                        break;
                }
            }

            //FreeChestRemainingSeconds = reader.ReadRRInt32() / 20; // Time remaining for free chest cooldown to end.
            //FreeChestTotalSeconds = reader.ReadRRInt32() / 20; // Total time free chest takes to open.
            //FreeChestNextDate = reader.ReadRRInt32(); // Timestamp, Date of when the next free chest will be available?
            FreeChestTimer.ReadMessageComponent(reader);

            // These could time values as well.
            //Unknown23 = reader.ReadRRInt32(); // 0
            //Unknown24 = reader.ReadRRInt32(); // 0
            //Unknown25 = reader.ReadRRInt32(); // 63

            UnknownTimer2.ReadMessageComponent(reader);

            Unknown26 = reader.ReadRRInt32(); // 0, Could be boolean
            FreeChests = new ChestMessageComponent[Unknown26];
            for (int i = 0; i < Unknown26; i++)
            {
                var chest = new ChestMessageComponent();
                chest.ReadMessageComponent(reader);

                FreeChests[i] = chest;
            }

            // Could be 2 strings, or 8 RRInt32s?
            Unknown27 = reader.ReadInt32(); // 0
            Unknown28 = reader.ReadInt32(); // 0

            Unknown29 = reader.ReadRRInt32(); // 0

            CrownCount = reader.ReadRRInt32(); // Number of crowns the user currently have.
            CrownChestUnlocked = reader.ReadRRInt32(); // Could be boolean.

            //CrownChestNextSeconds = reader.ReadRRInt32() / 20; // Time remaining for next crown chest?
            //CrownChestTotalTime = reader.ReadRRInt32() / 20; // 24h = 86396*2 = smthing
            //CrownChestTimestamp = reader.ReadRRInt32(); // Timestamp of when there will be the next crown chest?
            CrownChestTimer.ReadMessageComponent(reader);

            //CrownChestSomethingSeconds = reader.ReadRRInt32() / 20; // Looks like some time values.
            //// Sometimes same as crownChestTotalTimeSeconds and crownChestTimestamp.
            //CrownChestTotalTime2 = reader.ReadRRInt32() / 20; // 24h = 86396*2 = smthing.
            //CrownChestTimestamp2 = reader.ReadRRInt32(); // Timestamp.
            CrownChestTimer2.ReadMessageComponent(reader);

            Unknown30 = reader.ReadRRInt32(); // 0

            //Unknown31 = reader.ReadRRInt32() / 20; // Some form of time measurement.
            //Unknown32 = reader.ReadRRInt32() / 20; // More time measurement - 1 week. 504000
            //Unknown33 = reader.ReadRRInt32(); // timestamp
            UnknownTimer3.ReadMessageComponent(reader);

            Unknown53 = reader.ReadRRInt32(); // 15
            Unknown54 = reader.ReadInt32(); // 0
            Unknown55 = reader.ReadInt32(); // 2

            ExpLevel = reader.ReadRRInt32(); // Level?
            ArenaType = reader.ReadRRInt32(); // 54 Arena Type?
            ArenaId = reader.ReadRRInt32(); // 01 Arena Id?

            Unknown34 = reader.ReadRRInt32(); // 542354048

            DayOfWeek = reader.ReadRRInt32(); // 1 Day of week?
            DayOfWeek2 = reader.ReadRRInt32(); // 1 Day of week?

            // timeVal & timeVal2 seems same.
            // Card shop's new card timer?
            //ShopNewCardTimeSeconds = reader.ReadRRInt32() / 20; // 711940
            //ShopNewCardTimeSeconds2 = reader.ReadRRInt32() / 20; // 711940
            //ShopNewCardTimestamp = reader.ReadRRInt32(); // Timestamp?
            ShopTimer.ReadMessageComponent(reader);

            // Shop/Offers?
            Unknown35 = reader.ReadRRInt32(); // 6, Array size?
            ShopItems = new ShopItemMessageComponent[Unknown35];
            for (int i = 0; i < Unknown35; i++)
            {
                var item = new ShopItemMessageComponent();
                item.ReadMessageComponent(reader);

                ShopItems[i] = item;
            }

            Unknown35 = reader.ReadRRInt32(); // 0

            // 3x(00 00 7F)
            // Could be time values.
            //Unknown36 = reader.ReadRRInt32(); // 0
            //Unknown37 = reader.ReadRRInt32(); // 0
            //Unknown38 = reader.ReadRRInt32(); // 63
            UnknownTimer4.ReadMessageComponent(reader);

            //Unknown39 = reader.ReadRRInt32(); // 0
            //Unknown40 = reader.ReadRRInt32(); // 0
            //Unknown41 = reader.ReadRRInt32(); // 63
            UnknownTimer5.ReadMessageComponent(reader);

            //Unknown42 = reader.ReadRRInt32(); // 0
            //Unknown43 = reader.ReadRRInt32(); // 0
            //Unknown44 = reader.ReadRRInt32(); // 63
            UnknownTimer6.ReadMessageComponent(reader);

            // Could be 4 RRINT32.
            Unknown45 = reader.ReadInt32(); // 16844288 or 100732416
            // Could be 4 RRINT32.
            Unknown46 = reader.ReadInt32(); // 0

            Unknown47 = reader.ReadRRInt32(); // 0

            // Not too sure about this array stuff.
            // Array size?
            Unknown48 = reader.ReadRRInt32();
            UnknownCharacterArray = new int[Unknown48];
            for (int i = 0; i < Unknown48; i++)
            {
                var type = reader.ReadRRInt32();
                var id = reader.ReadRRInt32();

                UnknownCharacterArray[i] = type * 1000000 + id;
            }

            Unknown49 = reader.ReadRRInt32(); // 0
            Unknown50 = reader.ReadRRInt32(); // 9
            Unknown51 = reader.ReadInt32(); // 0

            Unknown52 = reader.ReadByte(); // 248

            // Could be totally wrong about this.
            Unknown56 = reader.ReadRRInt32(); // 07
            UnknownComponent1 = new UnknownMessageComponent[Unknown56];
            for (int i = 0; i < Unknown56; i++)
            {
                var component = new UnknownMessageComponent();
                component.ReadMessageComponent(reader);

                UnknownComponent1[i] = component;
            }

            Unknown57 = reader.ReadRRInt32(); // 54000010

            Unknown58 = reader.ReadRRInt32(); // 2
            Unknown59 = reader.ReadRRInt32(); // 0
            Unknown60 = reader.ReadRRInt32(); // 342737
            Unknown61 = reader.ReadRRInt32(); // 4
            Unknown62 = reader.ReadRRInt32(); // 24
            Unknown63 = reader.ReadRRInt32(); // -3481
            Unknown64 = reader.ReadRRInt32(); // 8
            Unknown65 = reader.ReadRRInt32(); // 24
            Unknown66 = reader.ReadRRInt32(); // -4245
            Unknown67 = reader.ReadRRInt32(); // 352579400
            Unknown68 = reader.ReadRRInt32(); // 1
            Unknown69 = reader.ReadInt32(); // 0
            Unknown70 = reader.ReadRRInt32(); // 0
            Unknown71 = reader.ReadRRInt32(); // 0

            Avatar = new AvatarMessageComponent();
            Avatar.ReadMessageComponent(reader);

            Unknown72 = reader.ReadInt32(); // 170

            Unknown73 = reader.ReadRRInt32(); // 12022299
            Unknown74 = reader.ReadRRInt32(); // 1491892504
            Unknown75 = reader.ReadRRInt32(); // 1733095
        }

        /// <summary>
        /// Writes the <see cref="OwnHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(UserId);

            writer.Write(Unknown1, true);
            writer.Write(Unknown2, true);

            UnknownTimer1.WriteMessageComponent(writer);

            writer.Write(Unknown5, true);
            writer.Write(Unknown6, true);

            writer.Write(BattleDeck.Length, true);
            for (int i = 0; i < BattleDeck.Length; i++)
                writer.Write(BattleDeck[i], true);

            writer.Write(Deck.Length, true);
            for (int i = 0; i < Deck.Length; i++)
                writer.Write(Deck[i], true);

            writer.Write(Deck2.Length, true);
            for (int i = 0; i < Deck2.Length; i++)
                writer.Write(Deck2[i], true);

            writer.Write(Unknown7);

            for (int i = 0; i < BattleCards.Length; i++)
            {
                var card = BattleCards[i];
                card.WriteMessageComponent(writer);
            }

            writer.Write(UnusedCards.Length, true);
            for (int i = 0; i < UnusedCards.Length; i++)
            {
                var card = UnusedCards[i];
                card.WriteMessageComponent(writer);
            }

            writer.Write(Unknown8);

            writer.Write(Unknown9, true);
            writer.Write(Unknown10, true);

            writer.Write(Unknown11, true);
            writer.Write(Unknown12, true);
            writer.Write(Unknown13, true);
            writer.Write(Unknown14, true);

            writer.Write(ChallengeName);

            writer.Write(Unknown16, true);
            for (int i = 0; i < Unknown16; i++)
            {
                var challenge = Challenges[i];
                challenge.WriteMessageComponent(writer);
            }

            writer.Write(Unknown17, true);

            writer.Write(Unknown18, true);
            if (Unknown18 == 0)
            {
                // Hmm.
            }
            else if (Unknown18 == 2)
            {
                // Hmmm.
            }

            writer.Write(UnknownJsonData);
            writer.Write(Unknown20, true);
            writer.Write(Unknown21, true);

            if (Chests != null)
            {
                writer.Write(1, true);
                for (int i = 0; i < 4; i++)
                {
                    var chest = Chests[i];
                    chest.WriteMessageComponent(writer);
                }
            }
            else
            {
                writer.Write(0, true);
            }

            FreeChestTimer.WriteMessageComponent(writer);

            UnknownTimer2.WriteMessageComponent(writer);

            writer.Write(Unknown26, true);
            for (int i = 0; i < Unknown26; i++)
            {
                var chest = FreeChests[i];
                chest.WriteMessageComponent(writer);
            }

            writer.Write(Unknown27);
            writer.Write(Unknown28);

            writer.Write(Unknown29, true);

            writer.Write(CrownCount, true);
            writer.Write(CrownChestUnlocked, true);

            CrownChestTimer.WriteMessageComponent(writer);
            CrownChestTimer2.WriteMessageComponent(writer);

            writer.Write(Unknown30, true);

            UnknownTimer3.WriteMessageComponent(writer);

            writer.Write(Unknown53, true);
        }
    }
}
