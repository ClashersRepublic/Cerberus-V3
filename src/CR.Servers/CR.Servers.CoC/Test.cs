using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC
{
    internal class Test
    {
        internal Test()
        {
            this.IntToVInt(20021382);
        }

        internal void IntToVInt(int _Value)
        {
            List<byte> _Writer = new List<byte>();
            _Writer.AddVInt(_Value);
            Console.WriteLine("Int To VInt : " + _Value + " = " + BitConverter.ToString(_Writer.ToArray()));
            _Writer = null;
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());


            /*
            int AllianceCount = br.ReadInt32();

            for (int i = 0; i < AllianceCount; i++)
            {

                Console.WriteLine($"AllianceId: {br.ReadInt64()}");
                Console.WriteLine($"Name: {br.ReadString()}");
                Console.WriteLine($"Badge: {br.ReadInt32()}");
                Console.WriteLine($"Type: {br.ReadInt32()}");
                Console.WriteLine($"NumberOfMembers: {br.ReadInt32()}");
                Console.WriteLine($"Score: {br.ReadInt32()}");
                Console.WriteLine($"DuelScore: {br.ReadInt32()}");
                Console.WriteLine($"RequiredScore: {br.ReadInt32()}");
                Console.WriteLine($"RequiredDuelScore: {br.ReadInt32()}");
                Console.WriteLine($"WonWarCount: {br.ReadInt32()}");
                Console.WriteLine($"LostWarCount: {br.ReadInt32()}");
                Console.WriteLine($"EqualWarCount: {br.ReadInt32()}");
                Console.WriteLine($"Locale: {br.ReadInt32()}");
                Console.WriteLine($"WarFrequency: {br.ReadInt32()}");
                Console.WriteLine($"Origin: {br.ReadInt32()}");
                Console.WriteLine($"ExpPoints: {br.ReadInt32()}");
                Console.WriteLine($"ExpLevel: {br.ReadInt32()}");
                Console.WriteLine($"ConsecutiveWarWinsCount: {br.ReadInt32()}");
                Console.WriteLine($"PublicWarLog: {br.ReadBoolean()}");
                Console.WriteLine($"Whoknow: {br.ReadInt32()}");
                Console.WriteLine($"AmicalWar: {br.ReadBoolean()}");
            }

            /*
            var a = br.ReadZlibStream();
            Console.WriteLine($"Unknown: {br.ReadString()}");
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
