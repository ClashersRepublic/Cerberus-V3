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

    #endregion Usings

    internal class MySQL
    {
        internal const string Credentials = "server=localhost;user id=root;pwd=AK-53.com;CharSet=utf8mb4;persistsecurityinfo=True;database=ucsdb";

        /// <summary> //done
        /// Gets the seed.
        /// </summary>
        /// <returns>System.Int64.</returns>
        internal static long GetPlayerSeed()
        {
            const string SQL = "SELECT coalesce(MAX(PlayerId), 0) FROM player";
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

        /// <summary> //done
        /// Gets the seed.
        /// </summary>
        /// <returns>System.Int64.</returns>
        internal static long GetAllianceSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ClanId), 0) FROM clan";
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