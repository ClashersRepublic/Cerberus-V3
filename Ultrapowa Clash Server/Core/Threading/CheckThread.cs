using System;
using System.Threading;
using UCS.Core.Checker;
using UCS.Core.Web;

namespace UCS.Core.Threading
{
    [Obsolete]
    internal class CheckThread
    {
        public static void Start()
        {
            // Thinking more threads makes an app faster at its finest.
            Thread T = new Thread(() =>
            {
                //LicenseChecker.CheckForSavedKey(); //disabled atm
                //VersionChecker.VersionMain();
                DirectoryChecker.CheckDirectories();
                DirectoryChecker.CheckFiles();
            });

            T.Start();
            T.Priority = ThreadPriority.Normal; 
        }
    }
}
