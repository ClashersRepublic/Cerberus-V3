namespace CR.Servers.CoC.Core.Database
{
    using CR.Servers.CoC.Core.Database.Models.Mongo;
    using MongoDB.Driver;

    internal class Mongo
    {
        private static IMongoClient Client;
        private static IMongoDatabase Database;

        internal static IMongoCollection<Clans> Clans;
        internal static IMongoCollection<Players> Players;

        internal static int ClanSeed
        {
            get
            {
                Clans last = Clans.Find(T => T.HighId == Constants.ServerId).Sort(Builders<Clans>.Sort.Descending(T => T.LowId)).Limit(1).SingleOrDefault();

                return last?.LowId ?? 0;
            }
        }

        internal static int PlayerSeed
        {
            get
            {
                Players last = Players.Find(T => T.HighId == Constants.ServerId).Sort(Builders<Players>.Sort.Descending(T => T.LowId)).Limit(1).SingleOrDefault();

                return last?.LowId ?? 0;
            }
        }

        internal static void Initialize()
        {
            Client = new MongoClient("mongodb://127.0.0.1:27017");
            Database = Client.GetDatabase("ClashOfClans");

            Clans = Database.GetCollection<Clans>("Clans");
            Players = Database.GetCollection<Players>("Players");
        }
    }
}