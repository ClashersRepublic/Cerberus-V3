using System.Collections.Generic;
using System.IO;
using Magic.Helpers;
using Magic.Utilities.ZLib;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class ReplayData : Message
    {
        public ReplayData(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24114;
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