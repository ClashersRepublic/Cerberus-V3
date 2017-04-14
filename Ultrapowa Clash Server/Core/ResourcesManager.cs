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
        //private static DatabaseManager m_vDatabase;

        public static void Initialize()
        {
            //m_vDatabase = new DatabaseManager(); // Nice 1 DB manager

            m_vOnlinePlayers = new List<Level>();
            m_vClients = new ConcurrentDictionary<long, Client>();

            m_vInMemoryLevels = new ConcurrentDictionary<long, Level>();
            m_vInMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Client client)
        {
            m_vClients.TryAdd(client.GetSocketHandle(), client);
            Program.TitleAd();
        }

        public static void DropClient(long socketHandle)
        {
            try
            {
                var client = default(Client);
                if (!m_vClients.TryRemove(socketHandle, out client))
                {
                    Logger.Error("Tried to drop a client who is not registered in the client dictionary.");
                }
                else
                {
                    var socket = client.Socket;
                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { }
                    try { socket.Dispose(); }
                    catch { }

                    // Clean level from memory if its Level has been loaded.
                    var level = client.GetLevel();
                    if (level != null)
                        LogPlayerOut(level);

                    Logger.Write($"Client with socket handle {socketHandle} has been dropped.");
                    Program.TitleDe();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to drop client: " + ex);
            }
        }

        public static List<long> GetAllPlayerIds() => DatabaseManager.Instance.GetAllPlayerIds();

        public static List<long> GetAllClanIds() => DatabaseManager.Instance.GetAllClanIds();

        public static Client GetClient(long socketHandle) => m_vClients.ContainsKey(socketHandle) ? m_vClients[socketHandle] : null;

        public static List<Client> GetConnectedClients() => m_vClients.Values.ToList();

        public static void GetAllPlayersFromDB()
        {
            var players = DatabaseManager.Instance.GetAllPlayers();
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
            var result = GetInMemoryLevel(id);
            if (result == null)
            {
                result = DatabaseManager.Instance.GetAccount(id);
                if (persistent)
                    LoadLevel(result);
            }
            return result;
        }

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
            // Make sure to tick before dropping client because
            // we're not morons right.
            level.Tick();

            var user = DatabaseManager.Instance.Save(level);
            // Waiting for asynchronous work because we're smart.
            user.Wait();

            m_vOnlinePlayers.Remove(level);
            m_vInMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        private static Level GetInMemoryLevel(long id) => m_vInMemoryLevels.ContainsKey(id) ? m_vInMemoryLevels[id] : null;

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
