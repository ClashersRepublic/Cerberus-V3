using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server.Clans;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class Chats
    {
        [JsonProperty("seed")] internal int Seed;
        [JsonProperty("slots")] internal List<Entry> Slots;

        internal object Gate = new object();
        internal Clan Clan;

        internal Chats()
        {
            Slots = new List<Entry>(50);
        }

        internal Chats(Clan Clan, int Limit = 50)
        {
            this.Clan = Clan;
            Slots = new List<Entry>(Limit);
        }

        internal void Add(Entry Message)
        {
            lock (Gate)
            {
                Message.Message_LowID = Seed++;

                if (Slots.Count <= Slots.Capacity)
                {
                    Slots.Add(Message);
                }
                else
                {
                    Remove(Slots[0]);
                    Slots.Add(Message);
                }
            }

            foreach (var Member in Clan.Members.Values)
                if (Member.Connected)
                {
                    new Alliance_Stream_Entry(Member.Player.Device, Message).Send();
                }
        }

        internal void Remove(Entry Message)
        {
            if (Message != null)
            {
                var MessageID = Message.Message_LowID;
                var Deleted = true;

                lock (Gate)
                {
                    Deleted = Slots.Remove(Message);
                }

                if (Deleted)
                    foreach (var Member in Clan.Members.Values)
                        if (Member.Connected)
                        {
                            new Alliance_Remove_Stream(Member.Player.Device) { Message_ID = MessageID }.Send();
                        }
            }
        }

        internal Entry Get(int MessageID)
        {
            lock (Gate)
            {
                return Slots.Find(Input => Input.Message_LowID == MessageID);
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                var Packet = new List<byte>();

                Packet.AddInt(Slots.Count);

                foreach (var Message in Slots)
                    Packet.AddRange(Message.ToBytes());

                return Packet.ToArray();
            }
        }
    }
}
