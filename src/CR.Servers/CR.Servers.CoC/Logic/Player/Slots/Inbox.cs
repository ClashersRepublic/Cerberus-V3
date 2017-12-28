using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic
{
    internal class Inbox
    {
        internal Player Player;

        [JsonProperty] internal int Seed;
        [JsonProperty] internal ConcurrentDictionary<long, MailEntry> Entries;

        public Inbox(Player Player)
        {
            this.Player = Player;
            this.Entries = new ConcurrentDictionary<long, MailEntry>();
        }

        internal void Add(MailEntry StreamEntry)
        {
            if ((int) StreamEntry.Type != -1)
            {
                StreamEntry.LowId = Interlocked.Increment(ref this.Seed);

                if (this.Entries.TryAdd(StreamEntry.StreamId, StreamEntry))
                {
                    if (this.Entries.Count > 50)
                    {
                        if (this.Entries.TryRemove(this.Entries.Keys.First(), out MailEntry Removed))
                        {
                            this.RemoveEntry(Removed);
                        }
                    }

                    if (this.Player.Connected)
                    {
                        new Avatar_Stream_Entry(this.Player.Level.GameMode.Device){StreamEntry = StreamEntry}.Send();
                    }
                }
            }
            else
                Logging.Error(this.GetType(), "Impossible to add a base of AvatarStreamEntry.");
        }

        internal void Remove(MailEntry MailEntry)
        {
            if (this.Entries.TryRemove(MailEntry.StreamId, out MailEntry Removed))
            {
                this.RemoveEntry(Removed);
            }
        }

        internal void Remove(int StreamId)
        {
            if (this.Entries.TryRemove(StreamId, out MailEntry Removed))
            {
                this.RemoveEntry(Removed);
            }
        }

        internal void RemoveEntry(MailEntry StreamEntry)
        {
            if (this.Player.Connected)
            {
                //new Avatar_Stream_Entry_Removed_Message(this.Player.Device, StreamEntry).Send();
            }
        }

        internal void Update(MailEntry StreamEntry)
        {
            if (this.Entries.ContainsKey(StreamEntry.StreamId))
            {
                this.Entries[StreamEntry.StreamId] = StreamEntry;

                if (this.Player.Connected)
                {
                    new Avatar_Stream_Entry(this.Player.Level.GameMode.Device) { StreamEntry = StreamEntry }.Send();
                }
            }
        }

        internal void Encode(List<byte> Packet)
        {
            var Streams = this.Entries.Values.ToArray();
            Packet.AddInt(Streams.Length);

            foreach (var Stream in Streams)
            {
                Stream.Encode(Packet);
            }
        }


        internal void Tick()
        {
            foreach (MailEntry StreamEntry in this.Entries.Values.ToArray())
            {
                if (StreamEntry.Age >= 86400 * 30)
                {
                    this.Remove(StreamEntry);
                }
            }
        }

    }
}
