﻿using System;
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

            Reader br = new Reader(
                " 00000040000000470044f65d00000004687465745f002558000000010000002c0000507100000000000007080000000e0000000d00000000001e84800000000101e8480600000274000000030000000001000000000100000023003dc84d00000006647261676f6e67001955000000010000002c00004f3600000000000007080000002d0000005500000000001e84800000000101e8480600000e640000000600000002000000000001000000230038355c0000000c416c6c2042726f746865727365002250000000010000002c00004eaf0000000000000708000000260000003900000000001e84800000000101e8481a0000066700000005000000000100000000010000000b003c042a0000000f4a6f696e204b4f59414e20323031375c003155000000010000002b00004e2300000000000006a40000002b0000003000000002001e84800000000101e848060000064c00000005000000000000000000010000004700193059000000094c5445ed81b4eb9e9c66004855000000010000002c00004dfb0000000000000708000000380000005d00000000001e84800000000101e848d800001009000000060000000000000000000100000023002c10940000000b46414d494c59204755595369004655000000010000002c00004dbb0000000000000708000000560000002300000000001e84800000000101e848730000009200000007000000000000000000010000002f0040ce500000001457c3a35252c38fc3b8522046c38f474854c38b5267002a55000000010000002c00004daf00000000000007080000003d0000001900000005001e84800000000101e8481a000009d900000006000000000000000000010000000b0044f99c000000084d7920776f726c645f000659000000010000002c00004ccf0000000000000708000000220000000f00000000001e84800000000501e848060000018f000000050000000001000000000100000053003142d80000000d424c415a494e475f4c4f4f4e535b002558000000010000002c00004c8700000000000007080000000e0000001900000000001e84800000000501e848390000045a0000000300000000010000000001000000470002ab3c0000000b4f7572204b696e67646f6d69002255000000010000002c00004c5400000000000007080000008c0000006c00000007001e84800000000101e8480600000af400000009000000000000000000010000001700371e4300000009e9809fe4b98be88da36800165a000000010000002600004c3100000000000006a4000000730000003000000002001e84800000000101e848380000082b000000090000000000000000000100000023000df1000000000c50656e616c74794f665761726a002555000000010000002c00004c270000000000000708000000830000007300000004001e84800000000101e8480600000e3200000008000000000000000000010000002f0020655b0000000c55415120464f5220455645525d000055000000010000002b00004c060000000000000708000000000000001200000000001e84800000000101e848f7000001d8000000020000000001000000000100000017003127990000000f4d4520414e44204d592042414259535d003458000000010000002b00004bb10000000000000708000000050000001f00000000001e84800000000001e848060000002800000003000000000100000000010000002300378caf0000000d5269736b7920506c617965727367002f50000000010000002c00004b960000000000000708000000510000005400000000001e84800000000101e848b20000014f00000007000000010100000000010000000b000402030000000d534720567569394e68617531306700165a000000010000002c00004b5900000000000007080000008a0000006400000010001e84800000000101e8480600000e76000000060000000001000000000100000017002e4c820000000b232241707061706945232269003355000000010000002c00004b530000000000000708000000540000005700000001001e84800000000101e8480600001049000000070000000001000000000100000053000242e50000000b42617465617920737265796b001e55000000010000002c00004b1b000000000000070800000062000000bd00000000001e84800000000101e8482d00001171000000080000000201000000000100000047001d71f20000001cd985d8b1d8afd8a7d8a7d98620d8a8db8c20d8a7d8afd8b9d8a7d8a767001a55000000010000002400004af10000000000000708000000630000004300000000001e84800000000501e84873000003e7000000080000000000000000000100000023003f1f38000000046c6176696300105a000000010000002b00004ad90000000000000708000000280000003200000001001e84800000000101e848060000050b00000005000000000000000000010000003b003b37100000000d636c617368657273207761727363002b55000000010000002a00004a9c00000000000007080000000b0000002900000000001e84800000000101e8480600000012000000040000000000000000000100000053002edb430000000d524f59414c2042414c414e434562002f58000000010000002b00004a6b0000000000000708000000330000006700000000001e84800000000101e8481a000002de000000060000000001000000000100000023003e492e0000000f44726167616e204b696e672031737463003355000000010000002a00004a6a00000000000007080000002e0000003f00000000001e84800000000101e8480600000ea2000000050000000000000000000100000023001bbf2e0000000941726162205465616d63001a58000000010000002c00004a6500000000000007080000000e0000003300000002001e84800000000101e848b100000531000000040000000001000000000100000047001a91e90000000f53696e6261756e6777652041726d796700465a000000010000002c00004a4d000000000000070800000072000000b200000000001e84800000000101e848a2000019db000000090000000000000000000100000023004248c2000000134b494e4720434f425241e29885e29885e2988565001d59000000010000002c000049b300000000000006a40000003c0000002000000000001e84800000000101e8481a0000077100000006000000000100000000010000004700133b440000000841765020436c616e62003352000000010000002c000049830000000000000708000000140000009b00000001001e84800000000001e8480600000cbd000000050000000000000000000100000047002b74c10000000942696720636c6173686900475300000001000000210000497a0000000000000708000000630000005f00000000001e84800000000101e848060000126000000007000000000000000000010000000b0009ba66000000094368692042656172735f00004f000000010000001f00004979000000000000070800000067000000a200000005001e84800000000301e848f900000b0800000005000000010000000000010000003b0049fc5e0000000e22476f6420697320676f6f642e225c001e55000000010000001b0000496c0000000000000708000000040000000800000000001e84800000000101e84806000000ee0000000200000000010000000001000000470023141f0000000d486f756e6473204f66205761726c001955000000010000001f000049480000000000000708000000700000007b00000000001e84800000000101e848a8000003aa000000090000000200000000000100000017004aca7b0000000d757020696e20736d6f6b6520325b0010550000000100000021000049000000000000000708000000000000000100000000001e84800000000301e848f80000004800000001000000000000000000010000000b0047d6280000000d52455120414e44204c45415645600026550000000100000027000048f60000000000000708000000090000001000000000001e84800000000101e84806000001740000000300000000010000000001000000170040d62b0000000f5448554e44455220574f52494f525363002555000000010000002a000048dc00000000000007080000001c0000003000000000001e84800000000101e848b200000199000000050000000001000000000100000023004508580000000c4b696e67732428416a617929650034550000000100000022000048d90000000000000708000000250000001700000000001e84800000000101e8481a00000019000000050000000101000000000100000053002f69770000000f494e4449414e2057415252494f525362000e500000000100000029000048d00000000000000708000000540000004400000002001e84800000000101e8487100000de3000000060000000000000000000100000047004678d3000000084244204b694e67535e000a58000000010000002c000048ad00000000000006a4000000070000000b00000000001e84800000000101e8481a0000026000000002000000010000000000010000002f004a6f730000000e2a2a484150505920424f59532a2a5e00195a0000000100000027000048aa0000000000000708000000050000000a00000000001e84800000000101e848db000002dd00000002000000000000000000010000002f00446c91000000094b494e47204845524f61002255000000010000002b000048a200000000000006a4000000120000001500000000001e84800000000101e84806000002b20000000300000001010000000001000000170022fca100000014e58fb0e781a3e588b7e78986e881afe79b9fc2ae60002555000000010000002c0000484f0000000000000708000000100000001300000000001e84800000000401e848e40000046900000003000000000100000000010000002300022e2c0000000a54686520736869656c6465000f55000000010000002c0000481600000000000006400000002a0000006600000003001e84800000000301e848f9000002430000000600000000010000000001000000170048dc5b0000000e4448414b4120434f4320434c414e5e00425700000001000000290000480600000000000007080000000b0000000b00000000001e84800000000101e8481a00000139000000030000000201000000000100000023003ef5790000000f574152204348414c4c454e4745525365002552000000010000002b000047fc00000000000007080000003a0000002c00000001001e84800000000101e8480600000b7d00000005000000000000000000010000002f0046d159000000076b72697368696c5f0029590000000100000029000047f600000000000006a40000000d0000001500000000001e84800000000101e848710000028f0000000300000000000000000001000000470034f3a2000000114441524b20524944455253204244e284a26500225a000000010000002c000047f000000000000006a4000000260000004400000000001e84800000000101e8481a00000a8400000005000000000100000000010000003b00128d340000000e70756e65206c6567656e64732e2e6a00034f000000010000001a000047e10000000000000708000000950000004c00000002001e84800000000101e848710000044200000009000000020000000000010000004700342b3b0000002de180bbe18099e18094e180b9e18099e180ace1808ae180aee18080e180ade180afe18099e180bae180ace180b864003f59000000010000002c000047c600000000000006a40000005e0000004c00000001001e84800000000301e848a200000ed0000000070000000000000000000100000053003140460000000f576172202620526571202620476f2e65004455000000010000002a000047c10000000000000708000000290000004500000000001e84800000000101e848b200000121000000060000000001000000000100000017002d50680000001ee180b1e18080e180ace180b7e180b1e1809ee180ace18084e180b9e180b86e00484f000000010000001a000047ab0000000000000708000000880000007c00000000001e84800000000301e848a20000170d0000000900000000010000000001000000230046a103000000074d532053494d415d001055000000010000002c0000477a00000000000006a4000000250000001700000000001e84800000000101e8481a00000997000000040000000101000000000100000053000e319d0000001bd985d8a720d987d985d98720db8cdaa920d986d981d8b1db8cd98565001b5a000000010000002c000047760000000000000640000000280000003b00000000001e84800000000001e84873000000f00000000500000005010000000001000000530037e03a0000000f417369616e204e6174696f6e2049495b002558000000010000002a0000476e0000000000000708000000150000000600000000001e84800000000401e848f9000001d4000000040000000000000000000100000053002782670000000e546865204461726b2053746f726d5d001a5500000001000000290000476700000000000007080000000a0000001b00000000001e84800000000101e848f90000004e00000003000000000000000000010000003b0027c50d00000011432e4f2e43205469c3aa6e204cc6b0cc8363001957000000010000001c0000476700000000000007080000001a0000000f00000000001e84800000000101e848ff000002ea00000004000000000000000000010000001700101bb600000007537761676765725b000250000000010000002c0000475e0000000000000708000000070000001e00000000001e84800000000501e8480600000144000000010000000001000000000100000023003a35630000000d4244205355504552204b494e4766000c5a0000000100000024000047560000000000000708000000450000004100000000001e84800000000101e8480600000a8e00000006000000000100000000010000003b0030a20d00000009574152204845524f5367000555000000010000001a0000473000000000000007080000003c0000003d00000001001e84800000000101e848710000007a00000006000000000000000000010000003b00066e860000000f436173746c652043726173686572735b001a55000000010000002b0000472600000000000007080000000f0000002700000000001e84800000000501e84806000003f00000000300000000010000000001000000230040e36300000018e68898e7a59ee4b880e68092e8a180e6b581e5b9b2e9878c62002b55000000010000002c0000471300000000000005dc0000001c0000002000000000001e84800000000301e8483800000756000000040000000200000000000100000023004c72b60000000fe59381efbc8ce982a3e6aeb5e683855e00215100000001000000280000471100000000000006a4000000060000000500000000001e84800000000301e84838000001b200000002000000030100000000010000002f001438b90000000e5468652057617220526f7374657263003c56000000010000002e0000476900000000000005dc000000350000005600000002001e84800000000201e8480600000ccc000000060000000000000000000100000017002ab6d40000000647616e6e6f6e5b003359000000010000002b000046aa00000000000006a4000000050000000c00000000001e84800000000101e848f9000002ae00000002000000000100000000010000003b003faf250000000b6d616e206669676874657260001e5a0000000100000021000046930000000000000708000000080000003300000000001e84800000000201e848060000003b00000004000000010100000000010000000b0005eb740000000d56494c4c414747494f204248525b001f58000000010000002a000046780000000000000708000000160000004500000003001e84800000000001e84819000001ef0000000300000000000000000001"
                    .HexaToBytes());
            int b = br.ReadInt32();
            for (int i = 0; i < b; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine($"Clan ID {br.ReadInt64()}");
                Console.WriteLine($"Clan Name {br.ReadString()}");
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadInt32();
                br.ReadByte();
                br.ReadByte();
            }

            var data = br.ReadFully();
            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }

    }
}
