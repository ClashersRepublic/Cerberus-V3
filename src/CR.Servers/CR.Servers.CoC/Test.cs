using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC
{
    internal class Test
    {
        internal Test()
        {
            this.Uncompress("04369FBE");
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());
            Console.WriteLine(br.ReadInt32());
            /*Console.WriteLine($"Unknown: {br.ReadString()}");
            Console.WriteLine($"ReplayData: {br.ReadZlibStream()}");
            Console.WriteLine($"InitialStreamEndSubTick: {br.ReadInt32()}");
            Console.WriteLine($"InitialStreamCommandCount: {br.ReadInt32()}");

            for (int i = 0; i < 8; i++)
            {
                var CommandID = br.ReadInt32();

                if (Factory.Commands.ContainsKey(CommandID))
                {
                    var Command = Factory.CreateCommand(CommandID, new Device(null), br);

                    if (Command != null)
                    {
                        Console.WriteLine(CommandID);
                        Command.Decode();
                    }
                    else
                    {
                        Logging.Info(this.GetType(), "Command is null! (" + CommandID + ")");
                        break;
                    }
                }
            }*/

            var data = br.ReadFully();
            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }
    }
}
