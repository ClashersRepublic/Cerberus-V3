using Magic.ClashOfClans.Logic;
using Magic.Files;
using System;
using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans.Files;

namespace Magic.ClashOfClans.Core

{
    internal static class ObjectManager
    {
        private static readonly object s_sync = new object();
        private static readonly Random s_rand = new Random();

        private static long AllianceSeed;
        private static long AvatarSeed;

        public static int DonationSeed;

        public static void Initialize()
        {
            AvatarSeed = DatabaseManager.GetMaxPlayerId() + 1;
            AllianceSeed = DatabaseManager.GetMaxAllianceId() + 1;

            // Shit went down, should probs shutdown.

            // Every 30 minutes.
           // const int TIMER_PERIOD = 1000 * 60 * 30;
            //m_vSaveTimer = new Timer(SaveCycle, null, 0, TIMER_PERIOD);
        }

       /* private static void SaveCycle(object state)
        {
            var level = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryLevels());
            var alliance = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryAlliances());

            level.Wait();
            alliance.Wait();
        }*/

        /*public static Alliance CreateAlliance()
        {
            Alliance alliance;

            var seed = AllianceSeed;

            alliance = new Alliance(seed);
            AllianceSeed++;

            DatabaseManager.Instance.CreateAlliance(alliance);

            ResourcesManager.AddAllianceInMemory(alliance);
            return alliance;
        }*/

        public static Level CreateLevel(long seed, string token = "")
        {
            // Increment & manage AvatarSeed thread safely.
            lock (s_sync)
            {
                if (seed == 0 || AvatarSeed == seed)
                {
                    seed = AvatarSeed++;
                }
                else
                {
                    if (seed > AvatarSeed)
                        AvatarSeed = seed + 1;
                }
            }

            var level = new Level(seed);

            if (string.IsNullOrEmpty(token))
            {
                if (string.IsNullOrEmpty(level.Avatar.Token))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        char Letter = (char)s_rand.Next('A', 'Z');
                        level.Avatar.Token += Letter;
                    }
                }
            }
            else
            {
                level.Avatar.Token = token;
            }

            if (string.IsNullOrEmpty(level.Avatar.Password))
            {
                for (int i = 0; i < 6; i++)
                {
                    char Letter = (char)s_rand.Next('A', 'Z');
                    char Number = (char)s_rand.Next('1', '9');
                    level.Avatar.Password += Letter;
                    level.Avatar.Password += Number;
                }
            }

            level.Json = Home.Starting_Home;

            DatabaseManager.CreateLevel(level);

            return level;
        }

        /*public static Alliance GetAlliance(long allianceId)
        {
            var alliance = default(Alliance);

            // Try to get alliance from memory first then db.
            // Could be improved.
            if (ResourcesManager.InMemoryAlliancesContain(allianceId))
                return ResourcesManager.GetInMemoryAlliance(allianceId);

            alliance = DatabaseManager.Instance.GetAlliance(allianceId);

            if (alliance != null)
                ResourcesManager.AddAllianceInMemory(alliance);

            return alliance;
        }

        public static List<Alliance> GetInMemoryAlliances()
        {
            return ResourcesManager.GetInMemoryAlliances();
        }*/

        public static Level GetRandomOnlinePlayer()
        {
            var levels = ResourcesManager.GetInMemoryLevels();
            int index = s_rand.Next(0, levels.Count);
            return levels[index];
        }

        

        public static void RemoveInMemoryAlliance(long id)
        {
            //ResourcesManager.RemoveAllianceFromMemory(id);
        }
    }
}
