using System.Collections.Generic;
using System.IO;
using Magic.Helpers;

namespace Magic.Logic
{
    internal class Base
    {
        public virtual void Decode(byte[] baseData)
        {
        }

        public virtual byte[] Encode()
        {
            return new List<byte>().ToArray();
        }
    }
}
