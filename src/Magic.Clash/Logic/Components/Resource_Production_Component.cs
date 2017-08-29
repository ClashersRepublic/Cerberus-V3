using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;
using Resource = Magic.ClashOfClans.Files.CSV_Logic.Resource;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Resource_Production_Component : Component
    {
        public Resource_Production_Component(Game_Object ci, Level level) : base(ci)
        {
            TimeSinceLastClick = level.Avatar.LastTick;
            ProductionResourceData =
                CSV.Tables.Get(Gamefile.Resources).GetData(((Buildings) ci.Data).ProducesResource) as Resource;
            ResourcesPerHour = ((Buildings) ci.Data).ResourcePer100Hours;
            MaxResources = ((Buildings) ci.Data).ResourceMax;
        }

        public override int Type => 5;

        internal int[] MaxResources;
        internal Resource ProductionResourceData;
        internal int[] ResourcesPerHour;
        internal DateTime TimeSinceLastClick;

        internal void CollectResources()
        {
            var ci = (Construction_Item) Parent;
            var span = ci.Level.Avatar.LastTick - TimeSinceLastClick;
            float currentResources = 0;
            if (!ci.IsBoosted)
            {
                currentResources = (float) (ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) *
                                   (float) span.TotalSeconds;
            }
            else
            {
                if (ci.GetBoostEndTime >= ci.Level.Avatar.LastTick)
                {
                    currentResources = (float) (ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) *
                                       (float) span.TotalSeconds;
                    currentResources *= ci.GetBoostMultipier;
                }
                else
                {
                    var boostedTime = (float) span.TotalSeconds -
                                      (float) (ci.Level.Avatar.LastTick - ci.GetBoostEndTime).TotalSeconds;
                    var notBoostedTime = (float) span.TotalSeconds - boostedTime;
                    currentResources = (float) (ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) *
                                       notBoostedTime;
                    currentResources += (float) (ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) *
                                        boostedTime * ci.GetBoostMultipier;
                    ci.IsBoosted = false;
                }
            }

            currentResources = Math.Min(Math.Max(currentResources, 0), MaxResources[ci.UpgradeLevel]);

            if (currentResources >= 1)
            {
                var ca = ci.Level.Avatar;
                if (ca.Resources_Cap.Get(ProductionResourceData.GetGlobalId()) >= ca.Resources.Get(ProductionResourceData.GetGlobalId()) || ProductionResourceData.GetGlobalId() == 3000000)
                {
                    if (ProductionResourceData.GetGlobalId() != 3000000)
                        if (ca.Resources_Cap.Get(ProductionResourceData.GetGlobalId()) -
                            ca.Resources.Get(ProductionResourceData.GetGlobalId()) < currentResources)
                        {
                            var newCurrentResources =
                                ca.Resources_Cap.Get(ProductionResourceData.GetGlobalId() -
                                                     ca.Resources.Get(ProductionResourceData.GetGlobalId()));
                            TimeSinceLastClick =
                                ci.Level.Avatar.LastTick.AddSeconds(-((currentResources - newCurrentResources) /
                                                                      ((float) (ResourcesPerHour[ci.UpgradeLevel] /
                                                                                100.0) / (60f * 60f))));
                            currentResources = newCurrentResources;
                        }
                        else
                        {
                            TimeSinceLastClick = ci.Level.Avatar.LastTick;
                        }
                    else
                        TimeSinceLastClick = ci.Level.Avatar.LastTick;
#if DEBUG
                    Logger.Say($"Resource System : Collecting {currentResources} of {ProductionResourceData.Name}");
#endif
                    ca.Resources.Plus(ProductionResourceData.GetGlobalId(), (int) currentResources);
                }
            }
        }

        public override void Load(JObject jsonObject)
        {
            var productionObject = (JObject) jsonObject["production"];
            if (productionObject != null)
                TimeSinceLastClick = TimeUtils.FromUnixTimestamp(productionObject["t_lastClick"].ToObject<int>());
        }

        internal void Reset()
        {
            TimeSinceLastClick = Parent.Level.Avatar.LastTick;
        }

        public override JObject Save(JObject jsonObject)
        {
            if (((Construction_Item) Parent).GetUpgradeLevel != -1)
            {
                var productionObject =
                    new JObject {{"t_lastClick", (int) TimeUtils.ToUnixTimestamp(TimeSinceLastClick)}};

                jsonObject.Add("production", productionObject);
                var ci = (Construction_Item) Parent;
                var seconds = (float) (Parent.Level.Avatar.LastTick - TimeSinceLastClick).TotalSeconds;

                if (ci.IsBoosted)
                    if (ci.GetBoostEndTime >= ci.Level.Avatar.LastTick)
                    {
                        seconds *= ci.GetBoostMultipier;
                    }
                    else
                    {
                        var boostedTime =
                            seconds - (float) (ci.Level.Avatar.LastTick - ci.GetBoostEndTime).TotalSeconds;
                        var notBoostedTime = seconds - boostedTime;
                        seconds = boostedTime * ci.GetBoostMultipier + notBoostedTime;
                    }
                jsonObject.Add("res_time",
                    (int) (MaxResources[ci.GetUpgradeLevel] / (float) (ResourcesPerHour[ci.UpgradeLevel] / 100.0) *
                           3600f - seconds));
            }
            return jsonObject;
        }
    }
}
