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
    using System.Collections.Generic;

    internal class Accounts : ConcurrentDictionary<long, WeakReference<Account>>
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

        internal ConcurrentDictionary<long, WeakReference<Home>> Homes;
        internal ConcurrentDictionary<long, WeakReference<Player>> Players;
        private static Thread _savePlayerThread;
        private static Thread _saveHomeThread;
        private static ConcurrentQueue<Player> _savePlayerQueue;
        private static ConcurrentQueue<Home> _saveHomeQueue;

        private int Seed;

        internal Accounts()
        {
            this.Seed = Mongo.PlayerSeed;

            this.Homes = new ConcurrentDictionary<long, WeakReference<Home>>();
            this.Players = new ConcurrentDictionary<long, WeakReference<Player>>();

            _savePlayerQueue = new ConcurrentQueue<Player>();
            _saveHomeQueue = new ConcurrentQueue<Home>();
            _savePlayerThread = new Thread(SavePlayerTask)
            {

                Priority = ThreadPriority.Highest
            };
            _saveHomeThread = new Thread(SaveHomeTask)
            {
                Priority = ThreadPriority.Highest
            };
            _savePlayerThread.Start();
            _saveHomeThread.Start();
        }

        internal void Add(Player Player)
        {
            if (!this.Players.ContainsKey(Player.UserId))
            {
                if (!this.Players.TryAdd(Player.UserId, new WeakReference<Player>(Player)))
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
            if (!this.Homes.ContainsKey(((long)Home.HighID << 32) | (uint)Home.LowID))
            {
                if (!this.Homes.TryAdd(((long)Home.HighID << 32) | (uint)Home.LowID, new WeakReference<Home>(Home)))
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
            if (!this.ContainsKey(((long)account.HighId << 32) | (uint)account.LowId))
            {
                if (!this.TryAdd(((long)account.HighId << 32) | (uint)account.LowId, new WeakReference<Account>(account)))
                {
                    Logging.Error(this.GetType(), "Unable to add the player account " + account.HighId + "-" + account.LowId + " in list.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to add the player account " + account.HighId + "-" + account.LowId + ". The player account is already in the dictionary.");
            }
        }

        private bool TryGetAccount(long id, out Account account)
        {
            WeakReference<Account> tmpRef;
            if (TryGetValue(id, out tmpRef))
            {
                if (tmpRef.TryGetTarget(out account))
                    return true;

                return false;
            }

            account = null;
            return false;
        }

        private bool TryGetHome(long id, out Home home)
        {
            WeakReference<Home> tmpRef;
            if (Homes.TryGetValue(id, out tmpRef))
            {
                if (tmpRef.TryGetTarget(out home))
                    return true;

                return false;
            }

            home = null;
            return false;
        }

        private bool TryGetPlayer(long id, out Player player)
        {
            WeakReference<Player> tmpRef;
            if (Players.TryGetValue(id, out tmpRef))
            {
                if (tmpRef.TryGetTarget(out player))
                    return true;

                return false;
            }

            player = null;
            return false;
        }

        internal Account CreateAccount()
        {
            int LowID = Interlocked.Increment(ref this.Seed);

            string Token = string.Empty;
            string Password = string.Empty;

            for (int i = 0; i < 40; i++)
            {
                Token += (char)Resources.Random.Next('A', 'Z');
            }

            for (int i = 0; i < 12; i++)
            {
                Password += (char)Resources.Random.Next('A', 'Z');
            }

            Player Player = new Player(null, Constants.ServerId, LowID)
            {
                Token = Token,
                Password = Password
            };

            Home Home = new Home(Constants.ServerId, LowID) { LastSave = LevelFile.StartingHome };

            Mongo.Players.InsertOneAsync(new Players
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

        internal Account LoadRandomOfflineAccount(bool store = true)
        {
            int serverId = Constants.ServerId;
            int seed = this.Seed;
            int rnd = 0;

            Account account = null;

            for (int i = 0; i < 50; i++)
            {
                rnd = Resources.Random.Next(1, seed + 1);

                Account tmp;
                if (!this.TryGetAccount(rnd, out tmp))
                {
                    account = this.LoadAccount(serverId, rnd, store);

                    if (account != null)
                    {
                        break;
                    }
                }
                else
                {
                    if (tmp.Device == null)
                    {
                        if (tmp.Battle != null)
                        {
                            if (!tmp.Battle.Ended)
                            {
                                continue;
                            }
                        }

                        tmp.Battle = null;
                        account = tmp;
                        break;
                    }
                }
            }

            return account;
        }

        internal Account LoadAccount(int HighID, int LowID, bool Store = true)
        {
            long ID = ((long)HighID << 32) | (uint)LowID;

            Account Account;
            if (!this.TryGetAccount(ID, out Account))
            {
                Players Data = Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefault();

                if (Data != null)
                {
                    Player Player;
                    if (!this.TryGetPlayer(ID, out Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    Home Home;
                    if (!this.TryGetHome(ID, out Home))
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
                    this.TryAdd(ID, new WeakReference<Account>(Account));
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
                long ID = ((long)Data.HighId << 32) | (uint)Data.LowId;

                if (!this.TryGetAccount(ID, out Account))
                {
                    Player Player;
                    if (!this.TryGetPlayer(ID, out Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    Home Home;
                    if (!this.TryGetHome(ID, out Home))
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

                    this.Add(Account);
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
            long ID = ((long)HighID << 32) | (uint)LowID;

            Account Account;
            if (!this.TryGetAccount(((long)HighID << 32) | (uint)LowID, out Account))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    Player Player;
                    if (!this.TryGetPlayer(ID, out Player))
                    {
                        Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                        if (Store)
                        {
                            this.Add(Player);
                        }
                    }

                    Home Home;
                    if (!this.TryGetHome(ID, out Home))
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
                        return Account;
                    }

                    Account = new Account(HighID, LowID, Player, Home);

                    this.TryAdd(ID, new WeakReference<Account>(Account));
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
            long ID = ((long)HighID << 32) | (uint)LowID;

            Player Player;
            if (!this.TryGetPlayer(ID, out Player))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    Player = this.LoadPlayerFromSave(Data.Player.ToJson());

                    if (Store)
                    {
                        this.Add(Player);
                    }

                    Home Home;
                    if (!this.TryGetHome(ID, out Home))
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
            long ID = ((long)HighID << 32) | (uint)LowID;

            Home Home;
            if (!this.TryGetHome(ID, out Home))
            {
                Players Data = await Mongo.Players.Find(T => T.HighId == HighID && T.LowId == LowID).SingleOrDefaultAsync();

                if (Data != null)
                {
                    Home = this.LoadHomeFromSave(Data.Home.ToJson());

                    if (Store)
                    {
                        this.Add(Home);
                    }

                    Player Player;
                    if (!this.TryGetPlayer(ID, out Player))
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

        internal void Saves()
        {
            WeakReference<Player>[] Players = this.Players.Values.ToArray();
            WeakReference<Home>[] Homes = this.Homes.Values.ToArray();

            foreach (WeakReference<Player> playerRef in Players)
            {
                Player player;
                if (playerRef.TryGetTarget(out player))
                    _savePlayerQueue.Enqueue(player);
            }

            foreach (WeakReference<Home> homeRef in Homes)
            {
                Home home;
                if (homeRef.TryGetTarget(out home))
                    _saveHomeQueue.Enqueue(home);
            }
        }

        internal Player[] GetAllPlayers()
        {
            WeakReference<Player>[] playerRefs = this.Players.Values.ToArray();
            List<Player> players = new List<Player>(playerRefs.Length);

            for(int i = 0; i < playerRefs.Length; i++)
            {
                WeakReference<Player> playerRef = playerRefs[i];
                Player player;
                if (playerRef.TryGetTarget(out player))
                    players.Add(player);
            }

            return players.ToArray();
        }

        private static void SavePlayerTask()
        {
            while (true)
            {
                Player Player;
                while (_savePlayerQueue.TryDequeue(out Player))
                {
                    Mongo.Players.UpdateOne(Save => Save.HighId == Player.HighID && Save.LowId == Player.LowID,
                        Builders<Players>
                            .Update.Set(Save => Save.Player,
                                BsonDocument.Parse(JsonConvert.SerializeObject(Player, Resources.Accounts.Settings))));
                    //Mongo.Players.ReplaceOne(T => T. == account.UserId, BsonDocument.Parse(JsonConvert.SerializeObject(account, Accounts.Settings)));
                }

                Thread.Sleep(5);
            }
        }

        private static void SaveHomeTask()
        {
            while (true)
            {
                Home Home;
                while (_saveHomeQueue.TryDequeue(out Home))
                {
                    Mongo.Players.UpdateOne(Save => Save.HighId == Home.HighID && Save.LowId == Home.LowID,
                        Builders<Players>
                            .Update.Set(Save => Save.Home,
                                BsonDocument.Parse(JsonConvert.SerializeObject(Home, Resources.Accounts.Settings))));
                    //Mongo.Players.ReplaceOne(T => T. == account.UserId, BsonDocument.Parse(JsonConvert.SerializeObject(account, Accounts.Settings)));
                }

                Thread.Sleep(5);
            }
        }
    }
}