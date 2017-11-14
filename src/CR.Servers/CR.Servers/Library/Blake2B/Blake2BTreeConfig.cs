﻿namespace CR.Servers.Library.Blake2B
{
    public sealed class Blake2BTreeConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blake2BTreeConfig"/> class.
        /// </summary>
        public Blake2BTreeConfig()
        {
            this.IntermediateHashSize = 64;
        }

        public int FanOut
        {
            get;
            set;
        }

        public int IntermediateHashSize
        {
            get;
            set;
        }

        public long LeafSize
        {
            get;
            set;
        }

        public int MaxHeight
        {
            get;
            set;
        }

        public static Blake2BTreeConfig CreateInterleaved(int _Parallel)
        {
            Blake2BTreeConfig _Result = new Blake2BTreeConfig
            {
                FanOut = _Parallel, MaxHeight = 2, IntermediateHashSize = 64
            };
            return _Result;
        }
    }
}