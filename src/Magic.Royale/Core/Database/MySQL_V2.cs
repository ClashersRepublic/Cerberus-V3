using MySql.Data.MySqlClient;

namespace Magic.ClashOfClans.Core.Database
{
    internal class MySQL_V2
    {
        internal static readonly string Credentials;

        static MySQL_V2()
        {
            Credentials = new MysqlEntities().Database.Connection.ConnectionString;
        }

        internal static long GetPlayerSeed()
        {
            const string SQL = "SELECT coalesce(MAX(Id), 0) FROM player";
            long Seed = -1;

            using (var conn = new MySqlConnection(Credentials))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(SQL, conn))
                {
                    cmd.Prepare();
                    Seed = (long)cmd.ExecuteScalar();
                    Logger.Say("Successfully retrieved max player ID: " + Seed + " player(s).");
                }
            }

            return Seed;
        }

        internal static long GetAllianceSeed()
        {
            const string SQL = "SELECT coalesce(MAX(Id), 0) FROM clan";
            long Seed = -1;
               
            using (var conn = new MySqlConnection(Credentials))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(SQL, conn))
                {
                    cmd.Prepare();
                    Seed = (long)cmd.ExecuteScalar();
                    Logger.Say("Successfully retrieved max alliance ID: " + Seed + " alliance(s).");
                }
            }

            return Seed;
        }
    }
}