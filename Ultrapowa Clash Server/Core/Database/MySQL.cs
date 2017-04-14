namespace UCS.Core.Database
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    using UCS.Logic;

    using MySql.Data.MySqlClient;
    using System.Configuration;
    using Helpers;

    #endregion Usings

    internal class MySQL
    {
        internal const string Credentials = "server=localhost;user id={0};pwd={1};CharSet=utf8mb4;persistsecurityinfo=True;database=ucsdb";

        /// <summary> //done
        /// Gets the seed.
        /// </summary>
        /// <returns>System.Int64.</returns>
        internal static long GetPlayerSeed()
        {
            const string SQL = "SELECT coalesce(MAX(PlayerId), 0) FROM player";
            long Seed = -1;

            var id = Utils.ParseConfigString("id");
            var pwd = Utils.ParseConfigString("pwd");
            using (MySqlConnection Conn = new MySqlConnection(string.Format(Credentials, id, pwd)))
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

        /// <summary> //done
        /// Gets the seed.
        /// </summary>
        /// <returns>System.Int64.</returns>
        internal static long GetAllianceSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ClanId), 0) FROM clan";
            long Seed = -1;

            var id = Utils.ParseConfigString("id");
            var pwd = Utils.ParseConfigString("pwd");
            using (MySqlConnection Conn = new MySqlConnection(string.Format(Credentials, id, pwd)))
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