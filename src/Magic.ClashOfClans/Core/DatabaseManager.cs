using Magic.ClashOfClans.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Magic.Core.Database;
using Magic.Core.Settings;
using Magic.Database;
using Magic.Logic;
using static Magic.Core.Logger;

namespace Magic.Core
{
    // This thing should have been a static class since the beginning.

    internal class DatabaseManager
    {
        public static DatabaseManager Instance => s_singleton;

        private static DatabaseManager s_singleton = new DatabaseManager();

        public DatabaseManager()
        {
            // Let them know we like it 1 instance.
            if (s_singleton != null)
                throw new InvalidOperationException("DatabaseManager is a singleton.");

            _connectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        private readonly string _connectionString;

        public long GetMaxAllianceId()
        {
            try
            {
                return MySQL.GetAllianceSeed();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to retrieve max alliance ID; check config.ucs.");
            }
            return -1;
        }

        public long GetMaxPlayerId()
        {
            try
            {
                return MySQL.GetPlayerSeed();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to retrieve max player ID; check config.ucs.");
            }
            return -1;
        }

        public void CreateLevel(Level level)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var newPlayer = new player
                    {
                        PlayerId = level.GetPlayerAvatar().GetId(),
                        AccountStatus = level.GetAccountStatus(),
                        AccountPrivileges = level.GetAccountPrivileges(),
                        LastUpdateTime = level.GetTime(),
                        IPAddress = level.GetIPAddress(),
                        Avatar = level.GetPlayerAvatar().SaveToJson(),
                        GameObjects = level.SaveToJson()
                    };

                    ctx.player.Add(newPlayer);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to create a new player account in database.");
            }
        }

        public void CreateAlliance(Alliance alliance)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var newClan = new clan
                    {
                        ClanId = alliance.GetAllianceId(),
                        LastUpdateTime = DateTime.Now,
                        Data = alliance.SaveToJson()
                    };

                    ctx.clan.Add(newClan);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to create a new alliance in database.");
            }
        }

        public Level GetLevel(long userId)
        {
            var level = default(Level);
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var player = ctx.player.Find(userId);
                    if (player != null)
                    {
                        level = new Level();
                        level.SetAccountStatus(player.AccountStatus);
                        level.SetAccountPrivileges(player.AccountPrivileges);
                        level.SetTime(player.LastUpdateTime);
                        level.GetPlayerAvatar().LoadFromJson(player.Avatar);
                        level.LoadFromJson(player.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to get a level {userId} from the database.");

                // In case the level instance was already created before the exception.
                level = null;
            }
            return level;
        }

        public Alliance GetAlliance(long allianceId)
        {
            var alliance = default(Alliance);
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var clan = ctx.clan.Find(allianceId);
                    if (clan != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJson(clan.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to get alliance from database.");

                // In case it fails to LoadFromJSON.
                alliance = null;
            }

            return alliance;
        }

        // Used whenever the clients searches for an alliance however no alliances is loaded in memory.
        public List<Alliance> GetAllAlliances()
        {
            List<Alliance> alliances = new List<Alliance>();
            try
            {
                using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
                {
                    var clans = ctx.clan;
                    Parallel.ForEach(clans, c =>
                    {
                        Alliance alliance = new Alliance();
                        alliance.LoadFromJson(c.Data);
                        alliances.Add(alliance);
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to get all alliances from database.");
            }
            return alliances;
        }

        public void RemoveAlliance(Alliance alliance)
        {
            long id = alliance.GetAllianceId();
            using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
            {
                ctx.clan.Remove(ctx.clan.Find((int)id));
                ctx.SaveChanges();
            }

            ObjectManager.RemoveInMemoryAlliance(id);
        }

        public async Task Save(Alliance alliance)
        {
            using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
            {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;
                clan c = await ctx.clan.FindAsync((int)alliance.GetAllianceId());
                if (c != null)
                {
                    c.LastUpdateTime = DateTime.Now;
                    c.Data = alliance.SaveToJson();
                    ctx.Entry(c).State = EntityState.Modified;
                }
                //else
                //{
                //    context.clan.Add(
                //        new clan
                //        {
                //            ClanId = alliance.GetAllianceId(),
                //            LastUpdateTime = DateTime.Now,
                //            Data = alliance.SaveToJSON()
                //        });
                //}
                await ctx.SaveChangesAsync();
            }
        }

        public void Save(Level level)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;

                    var player = ctx.player.Find(level.GetPlayerAvatar().GetId());
                    if (player != null)
                    {
                        player.LastUpdateTime = level.GetTime();
                        player.AccountStatus = level.GetAccountStatus();
                        player.AccountPrivileges = level.GetAccountPrivileges();
                        player.IPAddress = level.GetIPAddress();
                        player.Avatar = level.GetPlayerAvatar().SaveToJson();
                        player.GameObjects = level.SaveToJson();

                        ctx.Entry(player).State = EntityState.Modified;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to save a level {level.GetPlayerAvatar().GetId()} from the database. Check error for more information.");
                foreach (var entry in ex.EntityValidationErrors)
                {
                    foreach (var errs in entry.ValidationErrors)
                        Logger.Error($"{errs.PropertyName}:{errs.ErrorMessage}");
                }
                throw;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to save a level {level.GetPlayerAvatar().GetId()} from the database.");
                throw;
            }
        }

        public async Task Save(List<Level> levels)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    foreach (Level pl in levels)
                    {
                        lock (pl)
                        {
                            player p = ctx.player.Find(pl.GetPlayerAvatar().GetId());
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.GetTime();
                                p.AccountStatus = pl.GetAccountStatus();
                                p.AccountPrivileges = pl.GetAccountPrivileges();
                                p.IPAddress = pl.GetIPAddress();
                                p.Avatar = pl.GetPlayerAvatar().SaveToJson();
                                p.GameObjects = pl.SaveToJson();
                                ctx.Entry(p).State = EntityState.Modified;
                            }
                            //else
                            //    context.player.Add(
                            //        new player
                            //        {
                            //            PlayerId = pl.GetPlayerAvatar().GetId(),
                            //            AccountStatus = pl.GetAccountStatus(),
                            //            AccountPrivileges = pl.GetAccountPrivileges(),
                            //            LastUpdateTime = pl.GetTime(),
                            //            IPAddress = pl.GetIPAddress(),
                            //            Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                            //            GameObjects = pl.SaveToJSON()
                            //        });
                        }
                    }
                    await ctx.SaveChangesAsync();
                }
            }
            catch
            {
                // 0 Actual fucks given.
            }
        }

        public async Task Save(List<Alliance> alliances)
        {
            try
            {
                using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    foreach (Alliance alliance in alliances)
                    {
                        lock (alliance)
                        {
                            clan c = ctx.clan.Find((int)alliance.GetAllianceId());
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJson();
                                ctx.Entry(c).State = EntityState.Modified;
                            }
                            //else
                            //{
                            //    context.clan.Add(
                            //        new clan
                            //        {
                            //            ClanId = alliance.GetAllianceId(),
                            //            LastUpdateTime = DateTime.Now,
                            //            Data = alliance.SaveToJSON(),

                            //        });
                            //}
                        }
                    }
                    await ctx.SaveChangesAsync();
                }
            }
            catch
            {
                // 0 Actual fucks given.
            }
        }
    }
}
