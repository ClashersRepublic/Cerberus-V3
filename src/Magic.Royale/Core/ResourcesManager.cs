using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Magic.Royale.Extensions;
using Magic.Royale.Logic;

namespace Magic.Royale.Core
{
    internal static class ResourcesManager
    {
        // Socket Handle -> Client instance.
        private static ConcurrentDictionary<long, Device> _clients;

        // User Id -> Level instance.
        private static ConcurrentDictionary<long, Avatar> _inMemoryLevels;
        // Alliance Id -> Alliance instance.
        //private static ConcurrentDictionary<long, Alliance> _inMemoryAlliances;

        // Not sure why they are using this as well as InMemLevels.

        public static void Initialize()
        {
            OnlinePlayers = new List<Avatar>();
            _clients = new ConcurrentDictionary<long, Device>();

            _inMemoryLevels = new ConcurrentDictionary<long, Avatar>();
            //_inMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Device client)
        {
            _clients.TryAdd(client.GetSocketHandle(), client);
            Program.TitleAd();
        }

        public static bool DropClient(long socketHandle)
        {
            var closedSocket = false;
            try
            {
                var client = default(Device);
                if (_clients.TryRemove(socketHandle, out client))
                {
                    Program.TitleDe();

                    var socket = client.Socket;
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch
                    {
                        /* Swallow */
                    }
                    try
                    {
                        socket.Dispose();
                    }
                    catch
                    {
                        /* Swallow */
                    }

                    closedSocket = true;

                    // Clean level from memory if its Level has been loaded.
                    var level = client.Player;
                    if (level != null)
                        LogPlayerOut(level);

                    Logger.Write($"Client with socket handle {socketHandle} has been dropped.");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while dropping client.");
            }
            return closedSocket;
        }

        public static List<Device> GetConnectedClients()
        {
            return _clients.Values.ToList();
        }

        public static List<Avatar> GetInMemoryLevels()
        {
            return _inMemoryLevels.Values.ToList();
        }

        public static List<Avatar> OnlinePlayers { get; private set; }

        public static Avatar GetPlayer(long id, bool persistent = false)
        {
            // Try to get player from the memory, if not found
            // we look into the database.
            var result = GetInMemoryLevel(id);
            if (result == null)
            {
                result = DatabaseManager.GetLevel(id);
                if (result != null && persistent)
                    LoadLevel(result);
            }
            return result;
        }

        public static bool IsPlayerOnline(Avatar l)
        {
            return OnlinePlayers.Contains(l);
        }

        public static void LoadLevel(Avatar level)
        {
            _inMemoryLevels.TryAdd(level.UserId, level);
        }

        public static void LogPlayerIn(Avatar level)
        {
            var index = OnlinePlayers.IndexOf(level);
            if (index == -1)
            {
                OnlinePlayers.Add(level);

                // Register level in dictionary.
                LoadLevel(level);
            }
            else
            {
                Logger.Error("A client who is already logged in is trying to log in.");

                var oldLevel = OnlinePlayers[index];
                DropClient(oldLevel.Device.GetSocketHandle());

                OnlinePlayers.Add(level);
            }
        }

        public static void LogPlayerOut(Avatar level)
        {
            try
            {
                DatabaseManager.Save(level);
            }
            catch
            {
            }

            OnlinePlayers.Remove(level);
            _inMemoryLevels.TryRemove(level.UserId);
        }

        private static Avatar GetInMemoryLevel(long userId)
        {
            var level = default(Avatar);
            _inMemoryLevels.TryGetValue(userId, out level);
            return level;
        }

        /*public static List<Alliance> GetInMemoryAlliances() => _inMemoryAlliances.Values.ToList();

        public static void AddAllianceInMemory(Alliance alliance)
        {
            _inMemoryAlliances.TryAdd(alliance.AllianceId, alliance);
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
        }*/

        public static void DisconnectClient(Device device)
        {
            //Outofsync
            DropClient(device.GetSocketHandle());
        }
    }
}
