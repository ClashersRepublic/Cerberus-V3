using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Database;
using CR.Servers.CoC.Logic.Clan;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Slots
{
    internal class Clans : ConcurrentDictionary<long, Alliance>
    {
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        private int Seed;

        internal Clans()
        {
            //TODO: Player gameobject seems to be called 2 time
            this.Seed = Mongo.ClanSeed;
            this.GetRange();

            Logging.Info(this.GetType(), this.Count + " alliances loaded to the memory.");

        }

        internal void Add(Alliance Clan)
        {
            if (this.ContainsKey(Clan.AllianceId))
            {
                if (!this.TryUpdate(Clan.AllianceId, this[Clan.AllianceId], Clan))
                {
                    Logging.Error(this.GetType(), "Unsuccessfuly update the specified clan to the dictionnary.");
                }
            }
            else
            {
                if (!this.TryAdd(Clan.AllianceId, Clan))
                {
                    Logging.Error(this.GetType(), "Unsuccessfuly add the specified clan to the dictionnary.");
                }
            }
        }

        internal void Remove(Alliance Clan)
        {
            if (this.ContainsKey(Clan.AllianceId))
            {
                if (!this.TryRemove(Clan.AllianceId, out Alliance tmpClan))
                {
                    Logging.Error(this.GetType(), "Unsuccessfuly removed the specified clan from the dictionnary.");
                }
                else
                {
                    if (!tmpClan.Equals(Clan))
                    {
                        Logging.Error(this.GetType(),
                            "We successfully removed a clan from the list but the returned clan was not equal to our clan.");
                    }
                }
            }

            this.Save(Clan).Wait();
        }

        internal async Task<Alliance> GetAsync(int HighID, int LowID, bool Store = true)
        {
            long Id = (long) HighID << 32 | (uint) LowID;

            if (!this.TryGetValue(Id, out Alliance Clan))
            {
                var Save = await Mongo.Clans.Find(T => T.HighId == HighID && T.LowId == LowID).Limit(1)
                    .SingleOrDefaultAsync();

                if (Save != null)
                {
                    Clan = JsonConvert.DeserializeObject<Alliance>(Save.Data.ToJson(), this.Settings);

                    if (Store)
                    {
                        this.Add(Clan);
                    }
                }
            }

            return Clan;
        }

        internal Alliance Get(int HighID, int LowID, bool Store = true)
        {
            long Id = (long) HighID << 32 | (uint) LowID;

            if (!this.TryGetValue(Id, out Alliance Clan))
            {
                var Save = Mongo.Clans.Find(T => T.HighId == HighID && T.LowId == LowID).Limit(1).SingleOrDefault();

                if (Save != null)
                {
                    Clan = JsonConvert.DeserializeObject<Alliance>(Save.Data.ToJson(), this.Settings);

                    if (Store)
                    {
                        this.Add(Clan);
                    }
                }
            }
            return Clan;
        }

        internal List<Alliance> GetRange(int Offset = 0, int Limit = 50, bool Store = true)
        {
            var Clans = new List<Alliance>(Limit);
            var Saves = Mongo.Clans.Find(T => T.HighId == 0 && T.LowId >= Offset).Limit(Limit).ToList();

            foreach (var Save in Saves)
            {
                if (Save != null)
                {
                    var Clan = JsonConvert.DeserializeObject<Alliance>(Save.Data.ToJson(), this.Settings);

                    if (Store && Clan.Members.Slots.Count > 0)
                    {
                        this.Add(Clan);
                    }

                    Clans.Add(Clan);
                }
            }
            return Clans;
        }

        internal Alliance New(bool Store = true)
        {
            Alliance Clan = new Alliance(Constants.ServerId, Interlocked.Increment(ref this.Seed));

            Mongo.Clans.InsertOneAsync(new Core.Database.Models.Mongo.Clans
            {
                HighId = Clan.HighId,
                LowId = Clan.LowId,
                Data = BsonDocument.Parse(JsonConvert.SerializeObject(Clan, this.Settings))
            });

            if (Store)
            {
                this.Add(Clan);
            }

            return Clan;
        }

        internal Alliance New(Alliance Clan, bool Store = true)
        {
            if (Clan.LowId == 0)
            {
                Clan.LowId = Interlocked.Increment(ref this.Seed);
            }

            Mongo.Clans.InsertOneAsync(new Core.Database.Models.Mongo.Clans
            {
                HighId = Clan.HighId,
                LowId = Clan.LowId,
                Data = BsonDocument.Parse(JsonConvert.SerializeObject(Clan, this.Settings))
            });

            if (Store)
            {
                this.Add(Clan);
            }

            return Clan;

        }

        internal async Task Save(Alliance Clan) => await Mongo.Clans.UpdateOneAsync(T => T.HighId == Clan.HighId && T.LowId == Clan.LowId, Builders<Core.Database.Models.Mongo.Clans>.Update.Set(T => T.Data, BsonDocument.Parse(JsonConvert.SerializeObject(Clan, this.Settings))));

        internal async Task Saves()
        {
            Alliance[] Clans = this.Values.ToArray();

            foreach (var Clan in Clans)
            {
                try
                {
                    await this.Save(Clan);
                }
                catch (Exception Exception)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the save of the clan id " + Clan + " due to " + Exception +  ".");
                }
            }
        }
    }
}
