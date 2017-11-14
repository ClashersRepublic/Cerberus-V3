﻿using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Mode.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Obstacle : GameObject
    {
        internal bool Destructed;

        internal Timer ClearTimer;

        internal ObstacleData ObstacleData => (ObstacleData) this.Data;

        internal override int Type => 3;

        internal override int Checksum => 0;

        internal override int WidthInTiles => this.ObstacleData.Width;

        internal override int HeightInTiles => this.ObstacleData.Height;

        internal override int VillageType => this.ObstacleData.VillageType;

        internal int RemainingClearingTime => ClearTimer?.GetRemainingSeconds(this.Level.Time) ?? 0;

        internal bool ClearingOnGoing => this.ClearTimer != null;

        public Obstacle(Data Data, Level Level) : base(Data, Level)
        {

        }
        internal bool CanClearing()
        {
            if (!this.ClearingOnGoing)
            {
                if (!this.Destructed)
                {
                    return true;
                }
            }

            return false;
        }

        internal void ClearingFinished()
        {
            Player Player = this.Level.Player;

            if (Player?.Level.GameMode.State == State.Home)
            {
                // LogicAchievementManager::obstacleCleared();
                this.Level.WorkerManager.DeallocateWorker(this);

                this.Destructed = true;
            }
        }

        internal void StartClearing()
        {
            ObstacleData Data = this.ObstacleData;

            if (this.ClearingOnGoing)
            {
                if (Data.ClearTimeSeconds <= 0)
                {
                    this.ClearingFinished();
                }
            }
        }

        internal override void FastForwardTime(int Seconds)
        {
            if (this.ClearingOnGoing)
            {
                this.ClearTimer.FastForward(Seconds);
            }

            base.FastForwardTime(Seconds);
        }

        internal override void Tick()
        {
            if (this.ClearingOnGoing)
            {
                if (this.ClearTimer.GetRemainingSeconds(this.Level.Time) <= 0)
                {
                    this.ClearingFinished();
                }
            }

            base.Tick();
        }

        internal override void Load(JToken Json)
        {
            base.Load(Json);

            if (JsonHelper.GetJsonNumber(Json, "clear_t", out int ClearTime))
            {
                if (ClearTime > -1)
                {
                    this.ClearTimer = new Timer();
                    this.ClearTimer.StartTimer(this.Level.Time, ClearTime);
                }
            }
        }

        internal override void Save(JObject Json)
        {
            base.Save(Json);

            if (this.ClearingOnGoing)
            {
                Json.Add("clear_t", this.ClearTimer.GetRemainingSeconds(this.Level.Time));
            }
        }
    }
}
