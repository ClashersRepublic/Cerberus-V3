using CR.Servers.CoC.Core.Database.Models.Mongo;
using MongoDB.Driver;

namespace CR.Servers.CoC.Core.Database
{
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
                var last = Clans.Find(T => T.HighId == Constants.ServerId).Sort(Builders<Clans>.Sort.Descending(T => T.LowId)).Limit(1).SingleOrDefault();

                return last?.LowId ?? 0;
            }
        }

        internal static int PlayerSeed
        {
            get
            {
                var last = Players.Find(T => T.HighId == Constants.ServerId).Sort(Builders<Players>.Sort.Descending(T => T.LowId)).Limit(1).SingleOrDefault();

                return last?.LowId ?? 0;
            }
        }

        internal static void Initialize()
        {
            Mongo.Client = new MongoClient("mongodb://127.0.0.1:27017");
            Mongo.Database = Mongo.Client.GetDatabase("ClashOfClans");

            Mongo.Clans = Mongo.Database.GetCollection<Clans>("Clans");
            Mongo.Players = Mongo.Database.GetCollection<Players>("Players");
        }
    }
}
