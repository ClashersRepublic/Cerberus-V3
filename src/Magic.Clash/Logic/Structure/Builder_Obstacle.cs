using System;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json.Linq;
using Resource = Magic.ClashOfClans.Logic.Enums.Resource;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Builder_Obstacle : Game_Object
    {
        public Builder_Obstacle(Data data, Level l) : base(data, l, false)
        {
        }

        internal override bool Builder => true;
        internal override int ClassId => 10;
        internal override int OppositeClassId => 3;
        internal Obstacles GetObstacleData => (Obstacles) Data;
        internal Timer Timer;
        internal bool IsClearing;

        internal void CancelClearing()
        {
            if (!IsClearing)
                throw new InvalidOperationException("Obstacle object is not being cleared.");

            Level.BuilderWorkerManager.DeallocateWorker(this);
            IsClearing = false;
            Timer = null;
            var od = GetObstacleData;
            Level.Avatar.Resources.Plus(od.GetClearingResource().GetGlobalId(), od.ClearCost);
        }

        internal void StartClearing()
        {
            var constructionTime = GetObstacleData.ClearTimeSeconds;
            if (constructionTime < 1)
            {
                ClearingFinished();
            }
            else
            {
                Timer = new Timer();
                IsClearing = true;
                Timer.StartTimer(Level.Avatar.LastTick, constructionTime);
                Level.BuilderWorkerManager.AllocateWorker(this);
            }
        }

        internal void SpeedUpClearing()
        {
            var remainingSeconds = 0;
            if (IsClearing)
                remainingSeconds = Timer.GetRemainingSeconds(Level.Avatar.LastTick);

            var cost = GameUtils.GetSpeedUpCost(remainingSeconds);
            var ca = Level.Avatar;
            if (ca.Resources.Gems >= cost)
            {
                ca.Resources.Minus(Resource.Diamonds, cost);
                ClearingFinished();
            }
        }

        internal readonly int[] GemDrops =
        {
            3, 0, 1, 2, 0, 1, 1, 0, 0, 3,
            1, 0, 2, 2, 0, 0, 3, 0, 1, 0
        };

        internal DateTime ClearEndTime
        {
            get
            {
                if (!IsClearing)
                    throw new InvalidOperationException("Obstacle object is not clearing.");

                return TimeUtils.FromUnixTimestamp(Timer.EndTime);
            }
        }

        internal void ClearingFinished()
        {
            Level.BuilderWorkerManager.DeallocateWorker(this);
            IsClearing = false;
            Timer = null;
            var constructionTime = GetObstacleData.ClearTimeSeconds;
            var exp = (int) Math.Pow(constructionTime, 0.5f);

            var gems = GemDrops[Level.Avatar.ObstacleClearCount++];
            if (Level.Avatar.ObstacleClearCount >= GemDrops.Length)
                Level.Avatar.ObstacleClearCount = 0;

            Level.Avatar.AddExperience(exp);
            Level.Avatar.Resources.Plus(Resource.Diamonds, gems);

            var rd = CSV.Tables.Get(Gamefile.Resources).GetData(GetObstacleData.LootResource);

            Level.Avatar.Resources.Plus(rd.GetGlobalId(), GetObstacleData.LootCount);

            Level.GameObjectManager.RemoveGameObject(this);
        }

        internal int GetRemainingClearingTime()
        {
            return Timer.GetRemainingSeconds(Level.Avatar.LastTick);
        }

        public new void Load(JObject jsonObject)
        {
            var remTimeToken = jsonObject["clear_t"];
            var remTimeEndToken = jsonObject["clear_t_end"];
            if (remTimeToken != null && remTimeEndToken != null)
            {
                Timer = new Timer();
                IsClearing = true;
                var remainingClearingEndTime = remTimeEndToken.ToObject<int>();
                var startTime = (int) TimeUtils.ToUnixTimestamp(Level.Avatar.LastTick);
                var duration = remainingClearingEndTime - startTime;

                if (duration < 0)
                    duration = 0;

                Timer.StartTimer(Level.Avatar.LastTick, duration);
                Level.BuilderWorkerManager.AllocateWorker(this);
            }
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            if (IsClearing)
            {
                jsonObject.Add("clear_t", Timer.GetRemainingSeconds(Level.Avatar.LastTick));
                jsonObject.Add("clear_t_end", Timer.EndTime);
            }
            base.Save(jsonObject);
            return jsonObject;
        }

        public override void Tick()
        {
            base.Tick();
            if (IsClearing)
                if (Timer.GetRemainingSeconds(Level.Avatar.LastTick) <= 0)
                    ClearingFinished();
        }
    }
}