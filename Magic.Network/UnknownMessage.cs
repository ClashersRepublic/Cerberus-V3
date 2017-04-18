namespace Magic.Network
{
    /// <summary>
    /// Represents a Clash of Clans message that is not
    /// implemented in CoCSharp.
    /// </summary>
    public class UnknownMessage : Message
    {
        /// <summary>
        /// Gets the ID of the <see cref="UnknownMessage"/>.
        /// </summary>
        public override ushort Id { get; set; }

        /// <summary>
        /// Gets the version of the <see cref="UnknownMessage"/>.
        /// </summary>
        public override ushort Version { get; set; }

        /// <summary>
        /// Gets the length in bytes of the <see cref="UnknownMessage"/>.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Byte array that contains the raw decrypted <see cref="UnknownMessage"/> bytes.
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// Reads the <see cref="UnknownMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UnknownMessage"/>.
        /// </param>
        public override void Decode(MessageReader reader)
        {
            Bytes = reader.ReadBytes(Length);
        }

        /// <summary>
        /// Writes the <see cref="UnknownMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UnknownMessage"/>.
        /// </param>
        public override void Encode(MessageWriter writer)
        {
            writer.Write(Bytes);
        }
    }
}
