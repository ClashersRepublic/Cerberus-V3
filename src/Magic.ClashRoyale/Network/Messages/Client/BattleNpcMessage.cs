namespace Magic.Network.Messages.Client
{
    /// <summary>
    /// Description of BattleNpcMessage.
    /// </summary>
    public class BattleNpcMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleNpcMessage"/> class.
        /// </summary>
        public BattleNpcMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BattleNpcMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14104; } }

        public int Unknown1;
        public int Unknown2;

        /// <summary>
        /// Reads the <see cref="BattleNpcMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BattleNpcMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown2 = reader.ReadRRInt32();
        }

        /// <summary>
        /// Writes the <see cref="BattleNpcMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BattleNpcMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown2);
        }
    }
}
