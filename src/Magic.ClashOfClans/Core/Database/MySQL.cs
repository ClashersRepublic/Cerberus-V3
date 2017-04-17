using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.Core.Database
{
    internal class MySQL
    {
        //internal const string Credentials = "server=localhost;user id={0};pwd={1};CharSet=utf8mb4;persistsecurityinfo=True;database=ucsdb";
        internal static readonly string Credentials = "server=localhost;user id={0}{1};CharSet=utf8mb4;persistsecurityinfo=True;database=ucsdb";

        static MySQL()
        {
            var id = Utils.ParseConfigString("id");
            var pwd = Utils.ParseConfigString("pwd");

            if (pwd != string.Empty)
                pwd = ";pwd=" + pwd;

            Credentials = string.Format(Credentials, id, pwd);
        }

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