using Magic.Network;
using System.Diagnostics;
using System.IO;

namespace Magic
{
    internal static class Utils
    {
        public static void DumpBuffer(MessageReader reader)
        {
            var stream = reader.BaseStream as MemoryStream;
            if (stream != null)
            {
                var buffer = stream.ToArray();
                File.WriteAllBytes("dump", buffer);
            }
        }

        public static void DumpBuffer(MessageWriter writer)
        {
            var stream = writer.BaseStream as MemoryStream;
            if (stream != null)
            {
                var buffer = stream.ToArray();
                File.WriteAllBytes("dump", buffer);
            }
        }

        public static T ReadMessage<T>(string path) where T : Message, new()
        {
            var message = new T();
            var data = File.ReadAllBytes(path);
            using (var reader = new MessageReader(new MemoryStream(data)))
            {
                try { message.ReadMessage(reader); }
                catch { }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                    Debug.WriteLine("Did not fully read message dump.");
            }

            return message;
        }
    }
}
