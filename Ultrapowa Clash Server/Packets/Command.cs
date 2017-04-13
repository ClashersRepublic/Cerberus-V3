using System.Collections.Generic;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class Command
    {
        public const int MaxEmbeddedDepth = 10;

        internal int Depth { get; set; }

        internal Client Device;

        public virtual byte[] Encode() => new List<byte>().ToArray();

        public virtual void Execute(Level level)
        {
        }
    }
}
