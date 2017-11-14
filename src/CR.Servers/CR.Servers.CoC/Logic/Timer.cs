namespace CR.Servers.CoC.Logic
{
    internal class Timer
    {
        internal int EndSubTick;

        internal bool Started => this.EndSubTick != -1;

        internal void StartTimer(Time Time, int Seconds)
        {
            this.EndSubTick = Time.SubTick + Time.GetSecondsInTicks(Seconds);
        }
        
        internal void IncreaseTimer(int Seconds)
        {
            this.EndSubTick += Time.GetSecondsInTicks(Seconds);
        }
        
        internal void StopTimer()
        {
            this.EndSubTick = -1;
        }
  
        internal void FastForward(int Seconds)
        {
            this.EndSubTick -= Time.GetSecondsInTicks(Seconds);
        }
        
        internal void FastForwardSubTicks(int SubTick)
        {
            if (this.EndSubTick > 0)
            {
                this.EndSubTick -= SubTick;
            }
        }

        internal int GetRemainingSeconds(Time Time)
        {
            int SubTicks = this.EndSubTick - Time.SubTick;

            if (SubTicks > 0)
            {
                return Math.Max((int)(16L * SubTicks / 1000) + 1, 1);
            }

            return 0;
        }

        internal int GetRemainingMs(Time Time)
        {
            int SubTicks = this.EndSubTick - Time.SubTick;

            if (SubTicks > 0)
            {
                return 16 * SubTicks;
            }

            return 0;
        }

        internal void AdjustSubTick(Time Time)
        {
            this.EndSubTick -= Time.SubTick;

            if (this.EndSubTick < 0)
            {
                this.EndSubTick = 0;
            }
        }
    }
}
