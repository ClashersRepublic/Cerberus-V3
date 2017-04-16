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
        // Socket Handle -> Client instance.
        private static ConcurrentDictionary<long, Client> m_vClients;

        // User Id -> Level instance.
        private static ConcurrentDictionary<long, Level> m_vInMemoryLevels;
        // Alliance Id -> Alliance instance.
        private static ConcurrentDictionary<long, Alliance> m_vInMemoryAlliances;

        // Not sure why they are using this as well as InMemLevels.
        private static List<Level> m_vOnlinePlayers;

        public static void Initialize()
        {
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
                    catch { /* Swallow */ }
                    try { socket.Dispose(); }
                    catch { /* Swallow */ }

                    // Mark the client as dropped.
                    Interlocked.CompareExchange(ref client._dropped, 1, 0);

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
                ExceptionLogger.Log(ex, "Exception while dropping client.");
            }
        }

        public static List<Client> GetConnectedClients() => m_vClients.Values.ToList();

        public static List<Level> GetInMemoryLevels()
        {
            var levels = new List<Level>();

            lock (m_vInMemoryLevels) // ??
                levels.AddRange(m_vInMemoryLevels.Values);

            return levels;
        }

        public static List<Level> GetOnlinePlayers() => m_vOnlinePlayers;

        public static Level GetPlayer(long id, bool persistent = false)
        {
            // Try to get player from the memory, if not found
            // we look into the database.
            var result = GetInMemoryLevel(id);
            if (result == null)
            {
                result = DatabaseManager.Instance.GetLevel(id);
                if (result != null && persistent)
                    LoadLevel(result);
            }
            return result;
        }

        public static bool IsPlayerOnline(Level l) => m_vOnlinePlayers.Contains(l);

        public static void LoadLevel(Level level)
        {
            m_vInMemoryLevels.TryAdd(level.GetPlayerAvatar().GetId(), level);
        }

        public static void LogPlayerIn(Level level, Client client)
        {
            // Set the back refs.
            level.SetClient(client);
            client.SetLevel(level);

            if (!m_vOnlinePlayers.Contains(level))
            {
                m_vOnlinePlayers.Add(level);
                LoadLevel(level);
            }
            // Should kill old client maybe?
            else
            {
                Logger.Error("A client who is already logged in is trying to log in.");
                int i = m_vOnlinePlayers.IndexOf(level);
                m_vOnlinePlayers[i] = level;
            }
        }

        public static void LogPlayerOut(Level level)
        {
            // Make sure to tick before dropping client because
            // we're not morons right.
            level.Tick();

            DatabaseManager.Instance.Save(level);

            m_vOnlinePlayers.Remove(level);
            m_vInMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        private static Level GetInMemoryLevel(long id) => m_vInMemoryLevels.ContainsKey(id) ? m_vInMemoryLevels[id] : null;

        public static List<Alliance> GetInMemoryAlliances() => m_vInMemoryAlliances.Values.ToList();

        public static void AddAllianceInMemory(Alliance alliance)
        {
            m_vInMemoryAlliances.TryAdd(alliance.GetAllianceId(), alliance);
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

        public static void DisconnectClient(Client c)
        {
            new OutOfSyncMessage(c).Send();
            DropClient(c.GetSocketHandle());
        }

        public static void SetGameObject(Level level, string json)
        {
            level.GetHomeOwnerAvatar().LoadFromJson(json);
            DisconnectClient(level.GetClient());
        }
    }
}
