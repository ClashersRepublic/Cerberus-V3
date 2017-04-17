using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;
using Magic.ClashOfClans;

namespace Magic.Core
{
    internal static class ResourcesManager
    {
        // Socket Handle -> Client instance.
        private static ConcurrentDictionary<long, Client> _clients;

        // User Id -> Level instance.
        private static ConcurrentDictionary<long, Level> _inMemoryLevels;
        // Alliance Id -> Alliance instance.
        private static ConcurrentDictionary<long, Alliance> _inMemoryAlliances;

        // Not sure why they are using this as well as InMemLevels.
        private static List<Level> _onlinePlayers;

        public static void Initialize()
        {
            _onlinePlayers = new List<Level>();
            _clients = new ConcurrentDictionary<long, Client>();

            _inMemoryLevels = new ConcurrentDictionary<long, Level>();
            _inMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Client client)
        {
            _clients.TryAdd(client.GetSocketHandle(), client);
            Program.TitleAd();
        }

        public static void DropClient(long socketHandle)
        {
            try
            {
                var client = default(Client);
                if (!_clients.TryRemove(socketHandle, out client))
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
                    var level = client.Level;
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

        public static List<Client> GetConnectedClients() => _clients.Values.ToList();

        public static List<Level> GetInMemoryLevels()
        {
            var levels = new List<Level>();

            lock (_inMemoryLevels) // ??
                levels.AddRange(_inMemoryLevels.Values);

            return levels;
        }

        public static List<Level> GetOnlinePlayers() => _onlinePlayers;

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

        public static bool IsPlayerOnline(Level l) => _onlinePlayers.Contains(l);

        public static void LoadLevel(Level level)
        {
            _inMemoryLevels.TryAdd(level.GetPlayerAvatar().GetId(), level);
        }

        public static void LogPlayerIn(Level level, Client client)
        {
            // Set the back refs.
            level.SetClient(client);
            client.Level = level;

            if (!_onlinePlayers.Contains(level))
            {
                _onlinePlayers.Add(level);
                LoadLevel(level);
            }
            // Should kill old client maybe?
            else
            {
                Logger.Error("A client who is already logged in is trying to log in.");
                int i = _onlinePlayers.IndexOf(level);
                _onlinePlayers[i] = level;
            }
        }

        public static void LogPlayerOut(Level level)
        {
            // Make sure to tick before dropping client because
            // we're not morons right.
            level.Tick();

            try
            {
                DatabaseManager.Instance.Save(level);
            }
            catch
            {
                // No need logging since its already done in the Save method.
            }

            _onlinePlayers.Remove(level);
            _inMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        private static Level GetInMemoryLevel(long id) => _inMemoryLevels.ContainsKey(id) ? _inMemoryLevels[id] : null;

        public static List<Alliance> GetInMemoryAlliances() => _inMemoryAlliances.Values.ToList();

        public static void AddAllianceInMemory(Alliance alliance)
        {
            _inMemoryAlliances.TryAdd(alliance.GetAllianceId(), alliance);
        }

        public static void AddAllianceInMemory(List<Alliance> all)
        {
            for (int i = 0, allCount = all.Count; i < allCount; i++)
            {
                Alliance a = all[i];
                _inMemoryAlliances.TryAdd(a.GetAllianceId(), a);
            }
        }

        public static bool InMemoryAlliancesContain(long key) => _inMemoryAlliances.Keys.Contains(key);

        public static Alliance GetInMemoryAlliance(long key)
        {
            Alliance a;
            _inMemoryAlliances.TryGetValue(key, out a);
            return a;
        }

        public static void RemoveAllianceFromMemory(long key)
        {
            _inMemoryAlliances.TryRemove(key);
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
