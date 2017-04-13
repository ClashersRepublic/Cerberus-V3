namespace UCS.Core.Threading
{
    using System;
    #region Usings

    using System.Threading;
    using System.Timers;

    using UCS.Logic;

    #endregion Usings

    internal class MemoryThread
    {
        internal Thread Thread;

        /// <summary>
        /// Intializes an instance of the <see cref="MemoryThread"/> class. 
        /// </summary>
        internal MemoryThread()
        {
            this.Thread = new Thread(this.Check);
            this.Thread.Name = this.GetType().Name;
            this.Thread.Priority = ThreadPriority.Normal;

            this.Thread.Start();
        }

        /// <summary>
        /// Checks if there is dead sockets.
        /// </summary>
        /// <param name="_Object"></param>
        internal void Check(object _Object)
        {
            System.Timers.Timer _Timer = new System.Timers.Timer();

            _Timer.Interval = 900000;
            _Timer.Elapsed += (s, a) =>
            {
                // Should probably do this using a keep alive timeout mechanism.

                // Copying a shit load of references to a new array cause why not right.
                // Well, he's right, but i can't do anything else, the whole emu need a rework
                // ClashServers will be a rework, xD
                Level[] Players = ResourcesManager.GetInMemoryLevels().ToArray();

                for (int Index = 0; Index < Players.Length; Index++)
                {
                    if (ResourcesManager.IsClientConnected(Players[Index].GetClient().GetSocketHandle()))
                    {
                        ResourcesManager.DropClient(Players[Index].GetClient().GetSocketHandle());
                    }
                }
            };
            _Timer.Enabled = true;

        }
    }
}