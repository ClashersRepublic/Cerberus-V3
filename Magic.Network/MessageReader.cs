using Magic.Core.ZLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Magic.Network
{
    /// <summary>
    /// Wrapper of <see cref="BinaryReader"/> that implements methods to read <see cref="Message"/>s.
    /// </summary>
    public class MessageReader : BinaryReader
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReader"/> class
        /// based on the specified stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        public MessageReader(Stream input) : base(input)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        #endregion

        #region Methods
        /// <summary>
        /// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>Reads an 8-byte floating point value from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override double ReadDouble()
        {
            CheckDispose();

            var buffer = ReadByteArrayEndian(8);
            return BitConverter.ToDouble(buffer, 0);
        }

        public long ReadRRInt64()
        {
            var hi = (long)ReadRRInt32();
            var lo = (uint)ReadRRInt32();
            return hi << 32 | lo;
        }

        /// <summary>
        /// Reads a 8-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>A 8-byte signed integer from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override long ReadInt64()
        {
            CheckDispose();

            return (long)ReadUInt64();
        }

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte unsigned integer from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override ulong ReadUInt64()
        {
            CheckDispose();

            var buffer = ReadByteArrayEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Reads a 4-byte floating-point value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte floating-point value from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override float ReadSingle()
        {
            CheckDispose();

            var buffer = ReadByteArrayEndian(4);
            return BitConverter.ToSingle(buffer, 0);
        }

        public int ReadRRInt32NeedFix()
        {
            // Imported from https://github.com/royale-proxy/cr-messages-python/blob/master/reader.py#L80-L82.
            // Thank you :]

            var n = ReadRVarInt();
            return (((n) >> 1) ^ (-((n) & 1)));
        }

        public int ReadRRInt32()
        {
            byte num1 = ReadByte();
            int num2 = (int)num1 & 128;
            int num3 = (int)num1 & 63;
            if (((int)num1 & 64) != 0)
            {
                if (num2 != 0)
                {
                    byte num4 = ReadByte();
                    int num5 = (int)num4 << 6 & 8128 | num3;
                    if (((int)num4 & 128) != 0)
                    {
                        byte num6 = ReadByte();
                        int num7 = num5 | (int)num6 << 13 & 1040384;
                        if (((int)num6 & 128) != 0)
                        {
                            byte num8 = ReadByte();
                            int num9 = num7 | (int)num8 << 20 & 133169152;
                            if (((int)num8 & 128) != 0)
                            {
                                byte num10 = ReadByte();
                                num3 = (int)((long)(num9 | (int)num10 << 27) | 2147483648L);
                            }
                            else
                                num3 = (int)((long)num9 | 4160749568L);
                        }
                        else
                            num3 = (int)((long)num7 | 4293918720L);
                    }
                    else
                        num3 = (int)((long)num5 | 4294959104L);
                }
            }
            else if (num2 != 0)
            {
                byte num4 = ReadByte();
                num3 |= (int)num4 << 6 & 8128;
                if (((int)num4 & 128) != 0)
                {
                    byte num5 = ReadByte();
                    num3 |= (int)num5 << 13 & 1040384;
                    if (((int)num5 & 128) != 0)
                    {
                        byte num6 = ReadByte();
                        num3 |= (int)num6 << 20 & 133169152;
                        if (((int)num6 & 128) != 0)
                        {
                            byte num7 = ReadByte();
                            num3 |= (int)num7 << 27;
                        }
                    }
                }
            }
            return num3;
        }

        private int ReadRVarInt()
        {
            int value = 0;
            int i = 0;
            int b = 0;

            while (true)
            {
                b = ReadByte();
                if (i == 0)
                {
                    var seventh = (b & 0x40) >> 6;
                    var msb = (b & 0x80) >> 7;

                    b = b << 1;
                    b = b & ~(0x181);
                    b = b | (msb << 7) | (seventh);
                }

                value |= (b & 0x7F) << i;
                i += 7;

                if (i > 35)
                    throw new InvalidMessageException("Variable length quantity is too long");

                if ((b & 0x80) == 0)
                    break;
            }

            return value;
        }


        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte signed integer read from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override int ReadInt32()
        {
            CheckDispose();

            return (int)ReadUInt32();
        }

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte unsigned integer from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override uint ReadUInt32()
        {
            CheckDispose();

            var buffer = ReadByteArrayEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte signed integer read from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override short ReadInt16()
        {
            CheckDispose();

            return (short)ReadUInt16();
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte unsigned integer from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override ushort ReadUInt16()
        {
            CheckDispose();

            var buffer = ReadByteArrayEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Reads a length-prefixed string encoded in UTF-8 from the current stream and advances the stream position
        /// by the length of the string and the length of the prefix which is 4 bytes long.
        /// </summary>
        /// <returns>A string read from the current stream.</returns>
        /// <exception cref="InvalidMessageException">String length is invalid.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public override string ReadString()
        {
            CheckDispose();

            var length = ReadInt32();
            if (length == -1)
                return null;

            CheckLength(length, "string");
            var buffer = ReadBytes(length);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Reads a length-prefixed compressed byte array from the current stream and advances the stream position by the length
        /// of the length of the compressed byte array and the length of the prefix which is 4 bytes long.
        /// </summary>
        /// <returns>A decompressed string read from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public string ReadZlibString()
        {
            CheckDispose();

            var bytes = ReadBytes();
            if (bytes == null)
                return null;

            if (bytes.Length > 0)
            {
                var decompressedLength = base.ReadInt32();
                var decompressedBytes = new byte[decompressedLength];

                using (var zlib = new ZlibStream(BaseStream, CompressionMode.Decompress))
                {
                    var count = zlib.Read(decompressedBytes, 0, decompressedLength);
                    Debug.Assert(count == decompressedLength);
                }

                return Encoding.UTF8.GetString(decompressedBytes);
            }
            return null;
        }

        /// <summary>
        /// Reads a length-prefixed byte array from the current stream and advances the stream position
        /// by the length of the byte array and the length of the prefix which is 4 bytes long.
        /// </summary>
        /// <returns>A byte array read from the current stream.</returns>
        /// <exception cref="InvalidMessageException">Byte array length is invalid.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        public byte[] ReadBytes()
        {
            CheckDispose();

            var length = ReadInt32();
            if (length == -1)
                return null;

            CheckLength(length, "byte array");
            return ReadBytes(length);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="MessageReader"/> class.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release managed resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Not much.
            }

            _disposed = true;
            base.Dispose(true);
        }

        public int ReadVarInt32()
        {
            int value = 0;
            int i = 0;
            int b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                value |= (b & 0x7F) << i;
                i += 7;
                if (i > 35)
                    throw new InvalidMessageException("Variable length quantity is too long");
            }
            return value | (b << i);
        }

        private byte[] ReadByteArrayEndian(int count)
        {
            var buffer = ReadBytes(count);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            return buffer;
        }

        private void CheckLength(int length, string typeName)
        {
            if (length > Message.MaxSize)
                throw new InvalidMessageException("The length of a " + typeName + " was larger than the maximum size of a message '" + length + "'.");

            if (length < -1)
                throw new InvalidMessageException("The length of a " + typeName + " was invalid '" + length + "'.");

            if (length > BaseStream.Length - BaseStream.Position)
                throw new InvalidMessageException("The length of a " + typeName + " was larger than the remaining bytes '" + length + "'.");
        }

        internal void CheckDispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access the MessageReader object because it was disposed.");
        }
        #endregion
    }
}
