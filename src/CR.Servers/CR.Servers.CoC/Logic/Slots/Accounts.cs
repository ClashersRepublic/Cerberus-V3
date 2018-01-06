namespace CR.Servers.CoC.Logic.Slots
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Database;
    using CR.Servers.CoC.Core.Database.Models.Mongo;
    using CR.Servers.CoC.Files;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Newtonsoft.Json;

    internal class Accounts : ConcurrentDictionary<long, Account>
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

        internal ConcurrentDictionary<long, Home> Homes;
        internal ConcurrentDictionary<long, Player> Players;
        
        private int Seed;

        internal Accounts()
        {
            this.Seed = Mongo.PlayerSeed;

            this.Homes = new ConcurrentDictionary<long, Home>();
            this.Players = new ConcurrentDictionary<long, Player>();
        }

        internal void Add(Player Player)
        {
            if (!this.Players.ContainsKey(Player.UserId))
            {
                if (!this.Players.TryAdd(Player.UserId, Player))
                {
                    Logging.Error(this.GetType(), "Unable to add the player avatar " + Player.HighID + "-" + Player.LowID + " in list.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to add the player avatar " + Player.HighID + "-" + Player.LowID + ". The player avatar is already in the dictionary.");
            }
        }

        internal void Add(Home Home)
        {
            if (!this.Homes.ContainsKey(((long) Home.HighID << 32) | (uint) Home.LowID))
            {
                if (!this.Homes.TryAdd(((long) Home.HighID << 32) | (uint) Home.LowID, Home))
                {
                    Logging.Error(this.GetType(), "Unable to add the player home " + Home.HighID + "-" + Home.LowID + " in list.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to add the player home " + Home.HighID + "-" + Home.LowID + ". The player home is already in the dictionary.");
            }
        }

        internal void Add(Account account)
        {
            if (!this.ContainsKey(((long) account.HighId << 32) | (uint) account.LowId))
            {
                if (!this.TryAdd(((long) account.HighId << 32) | (uint) account.LowId, account))
                {
                    Logging.Error(this.GetType(), "Unable to add the player account " + account.HighId + "-" + account.LowId + " in list.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to add the player account " + account.HighId + "-" + account.LowId + ". The player account is already in the dictionary.");
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

            Player Player = new Player(null, Constants.ServerId, LowID)
            {
                Token = Token,
                Password = Password
            };

            Home Home = new Home(Constants.ServerId, LowID) {LastSave = LevelFile.StartingHome};
            
            Mongo.Players.InsertOne(new Players
            {
                HighId = Constants.ServerId,
                LowId = LowID,

                Player = BsonDocument.Parse(JsonConvert.SerializeObject(Player, this.Settings)),
                Home = BsonDocument.Parse(JsonConvert.SerializeObject(Home, this.Settings))
            });

            Account account = new Account(Constants.ServerId, LowID, Player, Home);

            this.Add(Player);
            this.Add(Home);
            this.Add(account);

            Level Level = new Level();
            Level.SetPlayer(Player);
            Level.SetHome(Home);
            Level.FastForwardTime(0);
            Level.Process();

            return account;
        }

        internal Account LoadRandomAccount(bool store = true)
        {
            int serverId = Constants.ServerId;
            int seed = this.Seed;
            int rnd = 0;

            Account account = null;

            for (int i = 0; i < 50; i++)
            {
                rnd = Resources.Random.Next(1, seed);

                if (!this.TryGetValue(rnd, out Account tmp))
                {
                    account = this.LoadAccount(serverId, rnd, store);

                    if (account != null)
                    {
                        break;
                    }
                }
                else
                {
                    if (!tmp.InBattle)
                    {
                        account = tmp;
                        break;
                    }
                }
            }

            return account;
        }

        internal Account LoadAccount(int HighID, int LowID, bool Store = true)
        {
            long ID = ((long) HighID << 32) | (uint) LowID;

            if (!this.TryGetValue(ID, out Account Account))
            {
                Players Data = Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefault();

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

            if (Account != null)
            {
                if (Account.Player.Level == null || Account.Home.Level == null)
                {
                    Level Level = new Level();
                    Level.SetPlayer(Account.Player);
                    Level.SetHome(Account.Home);
                    Level.FastForwardTime(0);
                    Level.Process();
                }
            }

            return Account;
        }

        internal Account LoadAccountViaFacebook(string FacebookID, bool Store = true)
        {
            Account Account = null;
            Players Data = Mongo.Players.Find(T => T.FacebookId == FacebookID).SingleOrDefault();

            if (Data != null)
            {
                long ID = ((long) Data.HighId << 32) | (uint) Data.LowId;
                if (!this.TryGetValue(ID, out Account))
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
                        Logging.Error(this.GetType(), $"Unable to load account id:{Data.HighId}-{Data.LowId} and with facebook id {FacebookID}.");
                        return new Account(-1, -1, null, null);
                    }

                    Account = new Account(Data.HighId, Data.LowId, Player, Home);
                    this.TryAdd(ID, Account);
                }

                if (Account != null)
                {
                    if (Account.Player.Level == null && Account.Home.Level == null)
                    {
                        Level Level = new Level();
                        Level.SetPlayer(Account.Player);
                        Level.SetHome(Account.Home);
                        Level.FastForwardTime(0);
                        Level.Process();
                    }
                }
            }
            return Account;
        }

        internal async Task<Account> LoadAccountAsync(int HighID, int LowID, bool Store = true)
        {
            long ID = ((long) HighID << 32) | (uint) LowID;
            if (!this.TryGetValue(((long) HighID << 32) | (uint) LowID, out Account Account))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

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

            if (Account != null)
            {
                if (Account.Player.Level == null && Account.Home.Level == null)
                {
                    Level Level = new Level();
                    Level.SetPlayer(Account.Player);
                    Level.SetHome(Account.Home);
                    Level.FastForwardTime(0);
                    Level.Process();
                }
            }

            return Account;
        }


        internal async Task<Player> LoadPlayerAsync(int HighID, int LowID, bool Store = true)
        {
            long ID = ((long) HighID << 32) | (uint) LowID;

            if (!this.Players.TryGetValue(ID, out Player Player))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

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
            long ID = ((long) HighID << 32) | (uint) LowID;

            if (!this.Homes.TryGetValue(ID, out Home Home))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

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
            Player Player = JsonConvert.DeserializeObject<Player>(JSON, this.Settings);
            return Player;
        }

        internal Home LoadHomeFromSave(string JSON)
        {
            //var Home = new Home();
            //JsonConvert.PopulateObject(JSON, Home, this.Settings);
            Home Home = JsonConvert.DeserializeObject<Home>(JSON, this.Settings);
            return Home;
        }

        internal async Task SavePlayer(Player Player)
        {
            await Mongo.Players.UpdateOneAsync(Save => Save.HighId == Player.HighID && Save.LowId == Player.LowID,
                Builders<Players>
                    .Update.Set(Save => Save.Player,
                        BsonDocument.Parse(JsonConvert.SerializeObject(Player, this.Settings))));
        }

        internal async Task SaveHome(Home Home)
        {
            await Mongo.Players.UpdateOneAsync(Save => Save.HighId == Home.HighID && Save.LowId == Home.LowID,
                Builders<Players>.Update.Set(Save => Save.Home,
                    BsonDocument.Parse(JsonConvert.SerializeObject(Home, this.Settings))));
        }

        internal async Task Saves()
        {
            Player[] Players = this.Players.Values.ToArray();
            Home[] Homes = this.Homes.Values.ToArray();


            foreach (Player Player in Players)
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

            foreach (Home Home in Homes)
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