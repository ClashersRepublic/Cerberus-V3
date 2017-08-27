using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Royale.Extensions.Binary;
using Magic.Royale.Extensions.List;
using Magic.Royale.Files;
using Magic.Royale.Files.CSV_Logic;

namespace Magic.Royale
{
    internal static class Test
    {
        internal static void Unpack()
        {
            var a = new List<byte>();
            a.AddVInt(6669);
            Console.WriteLine(BitConverter.ToString(a.ToArray()).Replace("-", ""));

            Reader br = new Reader("38 03 07 A3 CD 91 17 A4 1E 2A 00 00 08 02 7F 0D 00 00 00 84 01 06 86 E4 91 17 87 01 03 00 00 1F 09 BC 80 AA 17 B0 0C 1B 00 00 80 01 03 84 A8 92 17 91 03 0B 00 00 18 01 7F 01 00 00 00 07 01 8E DE 93 17 11 09 00 00 8A 01 07 00 92 1D 00 00 00 3D 04 95 D4 96 17 B6 27 AA 04 00 00 81 01 06 BB E6 92 17 8A 21 20 00 00 0C 06 BE D2 91 17 10 04 00 00 8E 01 01 B9 9F 95 17 09 00 00 00 14 08 A8 CF 91 17 A4 18 1F 00 00 1A 02 91 CF A7 17 0A 00 00 00 8C 01 03 AD FD 91 17 9C 03 02 00 00 13 07 82 E6 91 17 01 01 00 00 82 01 06 95 D4 96 17 84 02 00 00 00 01 09 00 8C 09 2B 00 00 0A 01 7F 0B 00 00 00 3C 03 A1 CF 91 17 8B 03 00 00 00 85 01 06 A2 97 9F 17 21 0A 00 00 23 02 7F 04 00 00 00 05 02 B0 89 95 17 0B 00 00 00 93 01 00 7F 01 00 00 00 02 05 00 A7 20 21 00 00 0D 03 B0 89 95 17 05 00 00 00 3B 06 A5 F9 91 17 81 1F 2D 00 00 1C 02 9A BA 9A 17 06 00 00 00 8D 01 02 B0 89 95 17 0B 01 00 00 83 01 04 BA F3 94 17 02 00 00 00 8F 01 03 7F 02 00 00 00 19 09 AE BE 99 17 92 0C 00 00 00 21 01 9E BC A6 17 01 00 00 00 92 01 03 B1 AC 9A 17 06 00 00 00 1D 06 7F 8D 01 02 00 00 3F 04 A5 F5 91 17 B1 04 29 00 00 1E 01 A9 E9 B4 17 01 00 00 00 26 00 7F 01 00 00 00 12 07 A3 90 95 17 80 01 02 00 00 24 00 7F 01 00 00 00 15 01 7F 11 00 00 00 91 01 0A 9C F3 93 17 8F 15 00 00 00 28 06 98 CD B7 17 18 04 00 00 11 01 9B AD 96 17 0D 00 00 00 0F 05 99 CD 91 17 AA 01 15 00 01 8B 01 01 B0 89 95 17 0E 00 00 00 90 01 02 BF DA 9C 17 07 02 00 00 0B 03 86 E4 91 17 9E 25 20 00 00 06 09 86 E4 91 17 93 10 0C 00 01 94 01 00 7F 01 00 00 00 27 02 89 E7 BB 17 9B 02 03 00 00 10 02 9D CD 91 17 0F 00 00 00 20 05 B2 80 9F 17 83 21 37 00 00 95 01 00 7F 0A 00 00 00 2C 00 94 94 C8 17 17 0B 00 00 25 00 AB 88 E2 17 01 01 00 02".HexaToBytes());

            int b = br.ReadVInt();
            for (int i = 0; i < b; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
                Console.WriteLine($"Resource Value {br.ReadVInt()}");
            }

            var data = br.ReadFully();
            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }

    }
}
