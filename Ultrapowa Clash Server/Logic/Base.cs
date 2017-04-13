using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.Logic
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
