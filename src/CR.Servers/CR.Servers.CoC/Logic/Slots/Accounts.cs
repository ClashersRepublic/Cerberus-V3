using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Database;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Logic.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Slots
{
    internal class Accounts : ConcurrentDictionary<long, Account>
    {
        internal ConcurrentDictionary<long, Player> Players;
        internal ConcurrentDictionary<long, Home> Homes;

        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.Auto,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            /*PreserveReferencesHandling  = PreserveReferencesHandling.All,*/   ReferenceLoopHandling   = ReferenceLoopHandling.Ignore,
            Converters                  = new List<JsonConverter> { new DataConverter(),  },
            Formatting                  = Formatting.None
        };

        private int Seed;

        internal Accounts()
        {
            this.Seed = /*Constants.Database == DBMS.Mongo ? Mongo.PlayerSeed : MySQL_Backup.GetSeed("Players")*/ Mongo.PlayerSeed;

            this.Homes = new ConcurrentDictionary<long, Home>();
            this.Players = new ConcurrentDictionary<long, Player>();
        }

        internal void Add(Player Player)
        {
            if (!this.Players.TryAdd(Player.UserId, Player))
            {
                Logging.Error(this.GetType(), "Unable to add the player " + Player.HighID + "-" + Player.LowID + " in list.");
            }
        }

        internal void Add(Home Home)
        {
            if (!this.Homes.TryAdd((long)Home.HighID << 32 | (uint)Home.LowID, Home))
            {
                Logging.Error(this.GetType(), "Unable to add the player " + Home.HighID + "-" + Home.LowID + " in list.");
            }
        }

        internal Account CreateAccount()
        {
            int LowID = Interlocked.Increment(ref this.Seed);

            string Token = string.Empty;
            string Password = string.Empty;

            for (int i = 0; i < 40; i++)
            {
                Token += (char) Resources.Random.Next('A', 'Z');
            }

            for (int i = 0; i < 12; i++)
            {
                Password += (char) Resources.Random.Next('A', 'Z');
            }

            var Player = new Player(null, Constants.ServerId, LowID)
            {
                Token = Token,
                Password = Password
            };

            var Home = new Home(Constants.ServerId, LowID) {LastSave = LevelFile.StartingHome};



            Mongo.Players.InsertOneAsync(new Core.Database.Models.Mongo.Players
            {
                HighId = Constants.ServerId,
                LowId = LowID,

                Player = BsonDocument.Parse(JsonConvert.SerializeObject(Player, this.Settings)),
                Home = BsonDocument.Parse(JsonConvert.SerializeObject(Home, this.Settings))
            });

            this.Add(Player);
            this.Add(Home);

            return new Account(Constants.ServerId, LowID, Player, Home);
        }

        internal Account LoadAccount(int HighID, int LowID, bool Store = true)
        {
            long ID = (long) HighID << 32 | (uint) LowID;
            if (!this.TryGetValue(ID, out Account Account))
            {

                var Data = Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefault();

                if (Data != null)
                {
                    if (!this.Players.TryGetValue(ID, out Player Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    if (!this.Homes.TryGetValue(ID, out Home Home))
                    {
                        Home = this.LoadHomeFromSave(Data.Home.ToJson());

                        if (Store)
                        {
                            this.Add(Home);
                        }
                    }

                    if (Player == null || Home == null)
                    {
                        Logging.Error(this.GetType(), "Unable to load account id:" + HighID + "-" + LowID + ".");
                        return new Account(-1, -1, null, null);
                    }

                    Account = new Account(HighID, LowID, Player, Home);
                    this.TryAdd(ID, Account);
                }

            }

            return Account;
        }

        internal async Task<Account> LoadAccountAsync(int HighID, int LowID, bool Store = true)
        {
            long ID = (long) HighID << 32 | (uint) LowID;
            if (!this.TryGetValue((long) HighID << 32 | (uint) LowID, out Account Account))
            {
                var Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    if (!this.Players.TryGetValue(ID, out Player Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    if (!this.Homes.TryGetValue(ID, out Home Home))
                    {
                        Home = this.LoadHomeFromSave(Data.Home.ToJson());

                        if (Store)
                        {
                            this.Add(Home);
                        }
                    }


                    if (Player == null || Home == null)
                    {
                        Logging.Error(this.GetType(),
                            "Unable to load account id:" + HighID + "-" + LowID + ".");
                        return Account;
                    }

                    Account = new Account(HighID, LowID, Player, Home);
                    this.TryAdd(ID, Account);
                }
            }

            return Account;
        }

        internal async Task<Player> LoadPlayerAsync(int HighID, int LowID, bool Store = true)
        {
            long ID = (long) HighID << 32 | (uint) LowID;

            if (!this.Players.TryGetValue(ID, out Player Player))
            {

                var Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                    if (Store)
                    {
                        this.Add(Player);
                    }

                    if (!this.Homes.TryGetValue(ID, out Home Home))
                    {
                        Home = this.LoadHomeFromSave(Data.Home.ToJson());

                        if (Store)
                        {
                            this.Add(Home);
                        }
                    }

                    if (Player == null)
                    {
                        Logging.Error(this.GetType(), "Unable to load player id:" + HighID + "-" + LowID + ".");
                        return null;
                    }
                }
            }
            return Player;
        }

        internal async Task<Home> LoadHomeAsync(int HighID, int LowID, bool Store = true)
        {
            long ID = (long) HighID << 32 | (uint) LowID;

            if (!this.Homes.TryGetValue(ID, out Home Home))
            {

                var Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    Home = this.LoadHomeFromSave(Data.Home.ToJson());

                    if (Store)
                    {
                        this.Add(Home);
                    }

                    if (!this.Players.TryGetValue(ID, out Player Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    if (Home == null)
                    {
                        Logging.Error(this.GetType(), "Unable to load home id:" + HighID + "-" + LowID + ".");
                        return null;
                    }
                }
            }
            return Home;
        }

        internal Player LoadPlayerFromSave(string JSON)
        {
            var Player = JsonConvert.DeserializeObject<Player>(JSON, this.Settings);
            return Player;
        }

        internal Home LoadHomeFromSave(string JSON)
        {
            //var Home = new Home();
            //JsonConvert.PopulateObject(JSON, Home, this.Settings);
            var Home = JsonConvert.DeserializeObject<Home>(JSON, this.Settings);
            return Home;
        }

        internal async Task SavePlayer(Player Player)
        {
           await Mongo.Players.UpdateOneAsync(Save => Save.HighId == Player.HighID && Save.LowId == Player.LowID,
                Builders<Core.Database.Models.Mongo.Players>
                    .Update.Set(Save => Save.Player,
                        BsonDocument.Parse(JsonConvert.SerializeObject(Player, this.Settings))));
        }

        internal async Task SaveHome(Home Home)
        {

            await Mongo.Players.UpdateOneAsync(Save => Save.HighId == Home.HighID && Save.LowId == Home.LowID,
                Builders<Core.Database.Models.Mongo.Players>.Update.Set(Save => Save.Home,
                    BsonDocument.Parse(JsonConvert.SerializeObject(Home, this.Settings))));

        }

        internal async Task Saves()
        {
            Player[] Players = this.Players.Values.ToArray();
            Home[] Homes = this.Homes.Values.ToArray();


            foreach (var Player in Players)
            {
                try
                {
                    await this.SavePlayer(Player);
                }
                catch (Exception Exception)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the save of the player id " + Player.HighID + "-" + Player.LowID + " due to " + Exception + ".");
                }

            }

            foreach (var Home in Homes)
            {
                try
                {
                    await this.SaveHome(Home);
                }
                catch (Exception Exception)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the save of the home id " + Home.HighID + "-" + Home.LowID + " due to " + Exception + ".");
                }

            }


            /* Parallel.ForEach(Players, async Player =>
             {
                 try
                 {
                     await this.SavePlayer(Player);
                 }
                 catch (Exception Exception)
                 {
                     Logging.Error(this.GetType(), "An error has been throwed when the save of the player id " + Player.HighID + "-" + Player.LowID + ".");
                 }
             });

             Parallel.ForEach(Homes, async Home =>
             {
                 try
                 {
                     await this.SaveHome(Home);
                 }
                 catch (Exception Exception)
                 {
                     Logging.Error(this.GetType(), "An error has been throwed when the save of the home id " + Home.HighID + "-" + Home.LowID + ".");
                 }
             });*/
        }
    }
}
