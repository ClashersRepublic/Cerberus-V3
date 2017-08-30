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
                    Logger.Say("Successfully retrieved max player seed: " + Seed);
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
                    Logger.Say("Successfully retrieved highest alliance seed: " + Seed);
                }
            }

            return Seed;
        }

        internal static long GetPlayerCount()
        {
            const string SQL = "SELECT COUNT(*) FROM player";
            long Seed = -1;

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Seed = (long)CMD.ExecuteScalar();

                }
            }
            return Seed;
        }

        internal static long GetClanCount()
        {
            const string SQL = "SELECT COUNT(*) FROM clan";
            long Seed = -1;

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Seed = (long)CMD.ExecuteScalar();

                }
            }
            return Seed;
        }
    }
}