using Magic.Network.Messages.Server;
using System;
using System.Diagnostics;
using System.Threading;

namespace Magic.ClashRoyale
{
    public static class Program
    {
        public static Server Server { get; set; }

        public static void Main(string[] args)
        {
            //var ms = Utils.ReadMessage<OwnHomeDataMessage>("bin2.bin");
            //var msg = Utils.ReadMessage<OwnHomeDataMessage>("bin3.bin");
            //var msg1 = Utils.ReadMessage<OwnHomeDataMessage>("bin4.bin");
            var msg2 = Utils.ReadMessage<OwnHomeDataMessage>("bin5.bin");
            var msg3 = Utils.ReadMessage<OwnHomeDataMessage>("bin6.bin");
            var msg4 = Utils.ReadMessage<OwnHomeDataMessage>("bin7.bin");

            //var msg5 = Utils.ReadMessage<AvatarProfileResponseMessage>("bin8-profile.bin");
            //var msg6 = Utils.ReadMessage<AvatarProfileResponseMessage>("bin9-profile.bin");

            //Console.Title = "CrackRoyale - Custom Clash Royale emulator";

            var sw = Stopwatch.StartNew();
            Server = new Server();
            Server.Start();
            sw.Stop();

            Console.WriteLine("Done in {0}ms", sw.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
