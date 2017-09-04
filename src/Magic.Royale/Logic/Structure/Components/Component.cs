using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Structure.Game;
using Magic.Royale.Logic.Structure.Slots;
using Cards = Magic.Royale.Logic.Structure.Game.Cards;

namespace Magic.Royale.Logic.Structure.Components
{
    internal class Component
    {
        internal Avatar Player;

        internal Component(Avatar Player)
        {
            this.Player = Player;
        }

        internal byte[] ToBytes
        {
            get
            {
                var Data = new List<byte>();
                var TimeStamp = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                Data.AddLong(Player.UserId);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0); //donationCooldown
                Data.AddVInt(0); //donationCapacity
                Data.AddVInt(TimeStamp);

                Data.AddVInt(0);
                
                Data.AddVInt(Player.Decks.Count);
                {
                    foreach (var deck in Player.Decks)
                    {
                        Data.AddVInt(8);

                        foreach (var _Card in deck.Cards)
                        {
                            if (_Card != null)
                                Data.AddVInt((int) _Card.Scid.Value);
                            else
                            {
                                Data.AddVInt(0);
                            }
                        }

                    }
                }

                Data.AddByte(255);
                Data.AddRange(Player.Decks[Player.Active_Deck].Get8FirstCard());
                Data.AddRange(Player.Cards.ToBytes());

                Data.AddVInt(Player.Active_Deck); //Active deck
                Data.AddHexa("FF 2B 00 7F 00 00 00 00 04 00 7F 00 00 00 00 12 00 7F 00 00 00 00 0E 00 7F 00 00 00 00 93 01 00 7F 00 00 00 00 09 00 7F 00 00 00 00 17 00 7F 00 00 00 00 89 01 00 7F 00 00 00 00 09 98 11 "); //Unknown nigger

                Data.AddVInt(TimeStamp);
                Data.AddVInt(1);

                Data.AddVInt(0);
                Data.AddVInt(1); //Challange + events count

                Data.AddVInt(1); //Just cuted version //id
                Data.AddString("2v2 Button");
                Data.AddVInt(8);
                Data.AddVInt(1503298800);
                Data.AddVInt(1534834800);
                Data.AddVInt(1502694000);
                Data.AddHexa("0000000000000000");
                Data.AddString("2v2 Button");
                Data.AddString("{\"HideTimer\":true,\"HidePopupTimer\":true}");
                Data.AddHexa(
                    "00000000040080B692850B0000000A8309008409008509009311019511019611019711019811019911019A1101079311039511029611039711029811029911029A110102"); //Not event related?
                Data.AddString(
                    "{\"ID\":\"CARD_RELEASE\",\"Params\":{\"Bats\":\"20170707\",\"MovingCannon\":\"20170805\",\"MegaKnight\":\"20170908\"}}");
                Data.AddVInt(
                    1); //0 = disable below and have chest, 1 = 2 = 3  enable below and without chest,4 enable below and with chest
                Data.AddString(
                    "{\"ID\":\"CLAN_CHEST\",\"Params\":{\"StartTime\":\"20170317T070000.000Z\",\"ActiveDuration\":\"P3dT0h\",\"InactiveDuration\":\"P4dT0h\",\"ChestType\":[\"ClanCrowns\"]}}");

                var chestcount = 0;
                /*Data.AddVInt(4);
                //Static = 0
                //Opened = 1
                //Opened = 8

                //Chest local id should be different for each chest 
                Data.AddVInt(1); //First chest! (Must be 1 for the first chest)
                Data.AddSCID(new SCID(19000196));
                Data.AddVInt(1); //State
                Data.AddVInt(1); //Chest Local Id
                Data.AddVInt(1);
                Data.AddVInt(0); //Slot id
                Data.AddVInt(0);


                Data.AddVInt(0); //Chest 2
                Data.AddVInt(8);
                Data.AddSCID(new SCID(19000197));
                Data.AddVInt(0); //State
                Data.AddVInt(2); //Chest Local Id
                Data.AddVInt(1);
                Data.AddVInt(1); //Slot id
                Data.AddVInt(0);

                Data.AddVInt(0); //Chest 3
                Data.AddVInt(8);
                Data.AddSCID(new SCID(19000198));
                Data.AddVInt(1); //State
                Data.AddVInt(3); //Chest local id
                Data.AddVInt(1);
                Data.AddVInt(2); //Slot id
                Data.AddVInt(0);

                Data.AddVInt(0); //Chest 4
                Data.AddVInt(8);
                Data.AddSCID(new SCID(19000199));
                Data.AddVInt(1); //State
                Data.AddVInt(4); //Chest local id
                Data.AddVInt(1);
                Data.AddVInt(3); //Slot id
                Data.AddVInt(0);*/

                Data.AddVInt(0);
                Data.AddVInt(0);


                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);

                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);
                Data.AddVInt(1); //First chest! (Must be 1 for the first chest)
                Data.AddSCID(new SCID(19000191));
                Data.AddVInt(1); //State
                Data.AddVInt(5); //Chest local id

                Data.AddVInt(0);
                Data.AddVInt(63);
                Data.AddHexa("00000000000000000000");

                Data.AddVInt(10);
                Data.AddVInt(0);

                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);

                Data.AddVInt(501540);
                Data.AddVInt(668400);
                Data.AddVInt(1503580122);

                Data.AddVInt(1); //First chest! (Must be 1 for the first chest)
                Data.AddSCID(new SCID(19000073));
                Data.AddVInt(1); //State
                Data.AddVInt(6); //Chest local id

                Data.AddVInt(0);
                Data.AddVInt(63);

                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);
                Data.AddVInt(1);
                Data.AddHexa("00 00 00 00 00 00 00");

                Data.AddVInt(2); // 0, 1 = Animation Page Card (Tuto)
                Data.AddVInt(10); //Old Level
                Data.AddVInt(54);
                Data.AddVInt(Arenas.Get("Arena9").Index); //Old Arena
                Data.AddHexa("F990E0CA03");
                Data.AddVInt(5);
                Data.AddVInt(5);
                Data.AddVInt(1200); // 72000 = 1 hour => 1200 = 1 min => 20 = 1 sec
                Data.AddVInt(1200); // 72000 = 1 hour => 1200 = 1 min => 20 = 1 sec
                Data.AddVInt(TimeStamp);

                Data.AddVInt(3); //Card In Store Count

                var slot = 0;
                //Label None = 0
                //Label Upgradable = 1
                //Label New = 2
                var card = Cards.Get("MegaKnight");
                Data.AddVInt(1);
                Data.AddVInt(card.Index); //Card Index
                Data.AddVInt(2); //Level
                Data.AddVInt(0); //Bough time
                Data.AddVInt(0); //Count
                Data.AddVInt(0); //unknown 2
                Data.AddVInt(0); //Label
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddSCID(card.Scid);
                Data.AddVInt(slot++);

                var card2 = Cards.Get("MegaMinion");
                Data.AddVInt(1);
                Data.AddVInt(card2.Index);
                Data.AddVInt(2); //Level
                Data.AddVInt(0); //Bough time
                Data.AddVInt(0); //Count
                Data.AddVInt(0); //unknown 2
                Data.AddVInt(0); //Label
                Data.AddVInt(0);
                Data.AddVInt(0);

                Data.AddSCID(card2.Scid);
                Data.AddVInt(slot++);

                var card3 = Cards.Get("DartBarrell");
                Data.AddVInt(1);
                Data.AddVInt(card3.Index);
                Data.AddVInt(2); //Level
                Data.AddVInt(0); //Bough time
                Data.AddVInt(0); //Count
                Data.AddVInt(0); //unknown 2
                Data.AddVInt(0); //Label
                Data.AddVInt(0);
                Data.AddVInt(0);

                Data.AddSCID(card3.Scid);
                Data.AddVInt(slot++);

                Data.AddVInt(0);

                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);
                Data.AddVInt(0);
                Data.AddVInt(0);
                Data.AddVInt(63);

                Data.AddHexa(
                    "12139331AB10BF0102A6220700011A00010A0088280000FA07160595DA9217A2030107003E06ABD492178C010402000406000E0404001709ABD492179E0A900205000E0700862000040009099CF991170B0003001B01AED69E170300050089"); //Some Shit

                Data.AddVInt(1);
                Data.AddVInt(7);
                Data.AddVInt(0);

                Data.AddVInt(2);
                Data.AddVInt(5);
                Data.AddVInt(1);
                Data.AddVInt(0);

                Data.AddVInt(2); //Card sort List?
                Data.AddSCID(new SCID(26, 54));
                Data.AddSCID(new SCID(26, 49));


                Data.AddVInt(5); //Released card list?
                Data.AddSCID(new SCID(26, 46));
                Data.AddSCID(new SCID(28, 16));
                Data.AddSCID(new SCID(26, 48));
                Data.AddSCID(new SCID(26, 49));
                Data.AddSCID(new SCID(26, 54));


                Data.AddVInt(0);
                Data.AddVInt(1);
                Data.AddVInt(3);
                Data.AddHexa("DFD241");

                Data.AddVInt(0);
                Data.AddVInt(1);
                Data.AddVInt(0);


                //List stuff?

                Data.AddVInt(0); //List
                //Data.AddVInt(66000014);

                Data.AddVInt(0); //List
                //Data.AddVInt(66000015);
                //Data.AddVInt(66000014);
                //Data.AddVInt(66000049);

                Data.AddVInt(0);

                Data.AddVInt(0);
                //Data.AddVInt(66000017);

                Data.AddVInt(1);
                Data.AddVInt(1608786000); //Timestamp
                Data.AddVInt(0);
                Data.AddVInt(13);
                Data.AddVInt(1);

                Data.AddVInt(1);
                Data.AddVInt(54000010);

                Data.AddVInt(2); // 1 - without reward, 2 - with reward
                Data.AddVInt(1);
                Data.AddVInt(234802);
                Data.AddVInt(1);
                Data.AddVInt(25);
                Data.AddVInt(5709);
                Data.AddVInt(25);
                Data.AddVInt(25);
                Data.AddVInt(4945);
                Data.AddVInt(449470809);
                Data.AddHexa("0000000000000000000000");
                Data.AddVInt(0);
                Data.AddVInt(121);

                return Data.ToArray();
            }
        }
    }
}
