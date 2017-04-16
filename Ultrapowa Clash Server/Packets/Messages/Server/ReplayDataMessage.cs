using System.Collections.Generic;
using System.IO;
using UCS.Helpers;
using UCS.Utilities.ZLib;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class ReplayData : Message
    {
        public ReplayData(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24114);
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var text = File.ReadAllText("replay-json.txt");
            //data.AddRange(ZlibStream.CompressString(text));
            data.AddCompressedString(text);
            //data.AddRange(File.ReadAllBytes("test.bin"));
            Encrypt(data.ToArray());
        }
    }
}