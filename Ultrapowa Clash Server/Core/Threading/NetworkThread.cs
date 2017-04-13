using System;
using System.Configuration;
using System.Threading;
using UCS.Core.Network;
using UCS.Core.Web;

namespace UCS.Core.Threading
{
    [Obsolete]
    internal class NetworkThread
    {
        public static Thread T { get; set; }

        public static void Start()
        {
            //T = new Thread(() =>
            //{
            //    new ResourcesManager();
            //    new CSVManager();
            //    new ObjectManager();
            //    new Logger();
            //    new OldGateway();
            //});

            T.Start();
            T.Priority = ThreadPriority.Highest;
        }

	    public static void Stop()
	    {
		    T.Abort();
	    }
    }
}
