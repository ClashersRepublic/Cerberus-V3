namespace CR.Servers.CoC.Logic.Slots
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Packets.Messages.Server.Home;

    internal class Duels
    {
        internal Thread Matchmaking;
        internal ConcurrentDictionary<long, Device> WaitingDeviceQueue;

        internal Duels()
        {
            this.Matchmaking = new Thread(this.MatchmakingTask);
            this.WaitingDeviceQueue = new ConcurrentDictionary<long, Device>();
        }

        internal void MatchmakingTask()
        {
            while (true)
            {
                List<long> deviceKeys = this.WaitingDeviceQueue.Keys.ToList();

                for (int i = 0; i + 1 < deviceKeys.Count; i++)
                {
                    if (this.WaitingDeviceQueue.TryRemove(deviceKeys[i], out Device attacker1))
                    {
                        retry:

                        if (this.WaitingDeviceQueue.TryRemove(deviceKeys[++i], out Device attacker2))
                        {

                        }
                        else
                        {
                            if (i + 1 < deviceKeys.Count)
                            {
                                goto retry;
                            }
                            else
                            {
                                this.WaitingDeviceQueue.TryAdd(deviceKeys[i], attacker1);
                            }
                        }
                    }
                }

                Thread.Sleep(500);
            }
        }

        internal void Join(Device device)
        {
            if (device.Account != null)
            {
                this.WaitingDeviceQueue.TryAdd(device.Account.AccountId, device);
            }
        }

        internal void Quit(Device device)
        {
            if (device.Account != null)
            {
                if (this.WaitingDeviceQueue.TryRemove(device.Account.AccountId, out _))
                {
                    new OwnHomeDataMessage(device).Send();
                }
            }
        }
    }
}