using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UCS.Core.Network;
using UCS.Database;
using UCS.Files;
using UCS.Files.CSV;
using UCS.Files.Logic;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;
using static UCS.Core.Logger;
using Timer = System.Threading.Timer;

namespace UCS.Core

{
    internal static class ObjectManager
    {
        private static long m_vAllianceSeed;
        private static long m_vAvatarSeed;
        public static int m_vDonationSeed;
        private static int m_vRandomBaseAmount;
        private static string m_vHomeDefault;
        public static bool m_vTimerCanceled;
        public static Timer TimerReference;
        public static Dictionary<int, string> NpcLevels;
        public static Dictionary<int, string> m_vRandomBases;
        public static FingerPrint FingerPrint;

        public static void Initialize()
        {
            m_vTimerCanceled = false;

            NpcLevels = new Dictionary<int, string>();
            m_vRandomBases = new Dictionary<int, string>();
            FingerPrint = new FingerPrint();

            m_vAvatarSeed = DatabaseManager.Instance.GetMaxPlayerId() + 1;
            m_vAllianceSeed = DatabaseManager.Instance.GetMaxAllianceId() + 1;
            m_vHomeDefault = File.ReadAllText(@"Gamefiles/starting_home.json");

            //m_vDatabase.CheckConnection();

            LoadNpcLevels();
            //LoadRandomBase(); // Don't waste time on this stuff, since not used.

            TimerReference = new Timer(Save, null, 0, 5000);
            Say("UCS Database has been successfully loaded.");
        }

        private static void Save(object state)
        {
            var level = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryLevels());
            var alliance = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryAlliances());

            level.Wait();
            alliance.Wait();

            if (m_vTimerCanceled)
                TimerReference.Dispose();
        }

        public static Alliance CreateAlliance()
        {
            Alliance alliance;

            var seed = m_vAllianceSeed;

            alliance = new Alliance(seed);
            m_vAllianceSeed++;
            DatabaseManager.Instance.CreateAlliance(alliance);
            ResourcesManager.AddAllianceInMemory(alliance);
            return alliance;
        }

        public static Level CreateAvatar(long seed, string token)
        {
            var level = default(Level);
            if (seed == 0 || m_vAvatarSeed == seed)
                seed = m_vAvatarSeed++;
            else
            {
                if (seed > m_vAvatarSeed)
                    m_vAvatarSeed = seed + 1;
            }

            level = new Level(seed, token);
            level.LoadFromJSON(m_vHomeDefault);

            DatabaseManager.Instance.CreateAccount(level);

            return level;
        }

        public static void Load100AlliancesFromDB()
        {
            ResourcesManager.AddAllianceInMemory(DatabaseManager.Instance.Get100Alliances());
        }

        public static void LoadAllAlliancesFromDB()
        {
            ResourcesManager.AddAllianceInMemory(DatabaseManager.Instance.GetAllAlliances());
        }

        public static Alliance GetAlliance(long allianceId)
        {
            Alliance alliance;
            if (ResourcesManager.InMemoryAlliancesContain(allianceId))
            {
                return ResourcesManager.GetInMemoryAlliance(allianceId);
            }
            var alliancedb = DatabaseManager.Instance.GetAlliance(allianceId);
            alliancedb.Wait();

            alliance = alliancedb.Result;
            if (alliance != null)
            {
                ResourcesManager.AddAllianceInMemory(alliance);
                return alliance;
            }
            return null;
        }

        public static List<Alliance> GetInMemoryAlliances()
        {
            return ResourcesManager.GetInMemoryAlliances();
        }

        public static Level GetRandomOnlinePlayer()
        {
            int index = new Random().Next(0, ResourcesManager.GetInMemoryLevels().Count);
            return ResourcesManager.GetInMemoryLevels()[index];
        }

        public static Level GetRandomPlayerFromAll()
        {
            int index = new Random().Next(0, ResourcesManager.GetAllPlayerIds().Count);
            return ResourcesManager.GetPlayer(ResourcesManager.GetAllPlayerIds()[index]);
        }

        public static void LoadNpcLevels()
        {
            NpcLevels.Add(17000000, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc.json").ReadToEnd());
            NpcLevels.Add(17000001, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc2.json").ReadToEnd());
            for (int i = 2; i < 50; i++)
                using (StreamReader sr = new StreamReader(@"Gamefiles/level/NPC/level" + (i + 1) + ".json"))
                    NpcLevels.Add(i + 17000000, sr.ReadToEnd());
            Say("NPC Levels  have been succesfully loaded.");
        }

        public static void LoadRandomBase()
        {
            m_vRandomBaseAmount = Directory.GetFiles(@"Gamefiles/level/PVP", "Base*.json").Count();
            for (int i = 0; i < m_vRandomBaseAmount; i++)
                using (StreamReader sr2 = new StreamReader(@"Gamefiles/level/PVP/Base" + (i + 1) + ".json"))
                    m_vRandomBases.Add(i, sr2.ReadToEnd());
            Say("PVP Levels  have been succesfully loaded.");
        }

        public static void RemoveInMemoryAlliance(long id)
        {
            ResourcesManager.RemoveAllianceFromMemory(id);
        }

        public static int RandomBaseCount()
        {
            return m_vRandomBaseAmount;
        }

        //public void Dispose()
        //{
        //    if (TimerReference != null)
        //    {
        //        TimerReference.Dispose();
        //        TimerReference = null;
        //    }
        //}

    }
}
