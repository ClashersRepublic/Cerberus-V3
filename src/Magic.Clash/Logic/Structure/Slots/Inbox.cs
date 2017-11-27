using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class Inbox
    {
        internal Avatar Player;
        [JsonProperty("seed")] internal int Seed;
        [JsonProperty("slots")] internal List<Mail> Slots;

        internal object Gate = new object();

        internal Inbox()
        {
            Console.WriteLine("Hi2");
            Slots = new List<Mail>(20);
        }

        internal Inbox(Avatar Player, int Limit = 20)
        {Console.WriteLine("Hi");
            this.Player = Player;
            Slots = new List<Mail>(Limit);
        }

        internal void Add(Mail Message)
        {
            lock (Gate)
            {
                Message.Message_LowID = Seed++;
                Message.New = 2;

                if (Slots.Count < Slots.Capacity)
                {
                    Slots.Add(Message);
                }
                else
                {
                    Slots.RemoveAt(0);
                    Slots.Add(Message);
                }
            }

            var Avatar = ResourcesManager.GetPlayer(Player.UserId);
            if (Avatar?.Device != null)
                new Avatar_Stream_Entry(Avatar.Device, Message).Send();
        }

        internal void Remove(Mail Message)
        {
            if (Message != null)
            {

                lock (Gate)
                {
                    Slots.Remove(Message);
                }
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                var Packet = new List<byte>();

                Packet.AddInt(Slots.Count);

                foreach (var Message in Slots)
                    Packet.AddRange(Message.ToBytes);

                return Packet.ToArray();
            }
        }

        internal void Update()
        {
            foreach (var Entry in Slots)
            {
                /* if (Entry.Outdated)
                {
                    this.Remove(Entry);
                } */
            }
        }
    }
}
