using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.Core
{
    internal static class ResourcesManager
    {
        private static ConcurrentDictionary<long, Client> m_vClients;
        private static ConcurrentDictionary<long, Level> m_vInMemoryLevels;
        private static ConcurrentDictionary<long, Alliance> m_vInMemoryAlliances;
        private static List<Level> m_vOnlinePlayers;
        private static DatabaseManager m_vDatabase;

        public static void Initialize()
        {
            m_vDatabase = new DatabaseManager(); // Nice 1 DB manager
            m_vOnlinePlayers = new List<Level>();
            m_vClients = new ConcurrentDictionary<long, Client>();
            m_vInMemoryLevels = new ConcurrentDictionary<long, Level>();
            m_vInMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Client client)
        {
            //Client c = new Client(s);
            //c.CIPAddress = ((System.Net.IPEndPoint)s.RemoteEndPoint).Address.ToString();
            m_vClients.TryAdd(client.GetSocketHandle(), client);
            Program.TitleAd();
        }

        public static void DropClient(long socketHandle)
        {
            try
            {
                Client c;
                Socket s = m_vClients[socketHandle].Socket;
                string text = "Socket handle " + socketHandle + " dropped";

                m_vClients.TryRemove(socketHandle, out c);
                Program.TitleDe();
                s.Shutdown(SocketShutdown.Both);
                s.Close();

                if (c.GetLevel() != null)
                {
                    text = "Client with socket handle " + socketHandle + " dropped";
                    LogPlayerOut(c.GetLevel());
                }
                Logger.Write(text);
            }
            catch (Exception e)
            {
                Logger.Write("Dropping Client failed: " + e);
            }
        }

        public static List<long> GetAllPlayerIds() => m_vDatabase.GetAllPlayerIds();

        public static List<long> GetAllClanIds() => m_vDatabase.GetAllClanIds();

        public static Client GetClient(long socketHandle) => m_vClients.ContainsKey(socketHandle) ? m_vClients[socketHandle] : null;

        public static List<Client> GetConnectedClients() => m_vClients.Values.ToList();

        public static void GetAllPlayersFromDB()
        {
            var players = m_vDatabase.GetAllPlayers();
			Parallel.ForEach((players),t =>
			{
				if (!m_vInMemoryLevels.ContainsKey(t.Key))
					m_vInMemoryLevels.TryAdd(t.Key, t.Value);
			});
        }

        public static List<Level> GetInMemoryLevels()
        {
            var levels = new List<Level>();
            lock (m_vInMemoryLevels)
                levels.AddRange(m_vInMemoryLevels.Values);
            return levels;
        }

        public static List<Level> GetOnlinePlayers() => m_vOnlinePlayers;

        public static Level GetPlayer(long id, bool persistent = false)
        {
            var result = GetInMemoryPlayer(id);
            if (result == null)
            {
                var acc = m_vDatabase.GetAccount(id);
                acc.Wait();
                result = acc.Result;
                if (persistent)
                    LoadLevel(result);
            }
            return result;
        }

        public static bool IsClientConnected(long socketHandle) => m_vClients[socketHandle] != null && m_vClients[socketHandle].IsClientSocketConnected();

        public static bool IsPlayerOnline(Level l) => m_vOnlinePlayers.Contains(l);

        public static void LoadLevel(Level level)
        {
            m_vInMemoryLevels.TryAdd(level.GetPlayerAvatar().GetId(), level);
        }

        public static void LogPlayerIn(Level l, Client c)
        {
            l.SetClient(c);
            c.SetLevel(l);

            if (!m_vOnlinePlayers.Contains(l))
            {
                m_vOnlinePlayers.Add(l);
                LoadLevel(l);
            }
            else
            {
                int i = m_vOnlinePlayers.IndexOf(l);
                m_vOnlinePlayers[i] = l;
            }
        }

        public static void LogPlayerOut(Level level)
        {
            var user = DatabaseManager.Single().Save(level);
            user.Wait();
            m_vOnlinePlayers.Remove(level);
            m_vInMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        private static Level GetInMemoryPlayer(long id) => m_vInMemoryLevels.ContainsKey(id) ? m_vInMemoryLevels[id] : null;

        public static List<Alliance> GetInMemoryAlliances() => m_vInMemoryAlliances.Values.ToList();

        public static void AddAllianceInMemory(Alliance all)
        {
            m_vInMemoryAlliances.TryAdd(all.GetAllianceId(), all);
        }

        public static void AddAllianceInMemory(List<Alliance> all)
        {
            for (int i = 0, allCount = all.Count; i < allCount; i++)
            {
                Alliance a = all[i];
                m_vInMemoryAlliances.TryAdd(a.GetAllianceId(), a);
            }
        }

        public static bool InMemoryAlliancesContain(long key) => m_vInMemoryAlliances.Keys.Contains(key);

        public static bool InMemoryAlliancesContain(Alliance all) => m_vInMemoryAlliances.Values.Contains(all);

        public static Alliance GetInMemoryAlliance(long key)
        {
            Alliance a;
            m_vInMemoryAlliances.TryGetValue(key, out a);
            return a;
        }

        public static void RemoveAllianceFromMemory(long key)
        {
            m_vInMemoryAlliances.TryRemove(key);
        }

        public static void RemovePlayerFromMemory(Level l)
        {
            m_vInMemoryLevels.TryRemove(l.GetPlayerAvatar().GetId());
        }
        public static void DisconnectClient(Client c)
        {
            new OutOfSyncMessage(c).Send();
            DropClient(c.GetSocketHandle());
        }

        public static void SetGameObject(Level level, string json)
        {
            level.GetHomeOwnerAvatar().LoadFromJSON(json);
            DisconnectClient(level.GetClient());
        }
    }
}
