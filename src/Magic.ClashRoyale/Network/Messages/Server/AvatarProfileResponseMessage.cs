using Magic.Network.Messages.Components;

namespace Magic.Network.Messages.Server
{
    /// <summary>
    /// Message that is sent to confirm that connection to server is alive.
    /// </summary>
    public class AvatarProfileResponseMessage : Message
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProfileResponseMessage"/> class.
        /// </summary>
        public AvatarProfileResponseMessage()
		{
			// Space
		}

        /// <summary>
        /// Gets the ID of the <see cref="AvatarProfileResponseMessage"/>.
        /// </summary>
        public override ushort Id => 24113;

        public int Unknown1;
        public int Unknown2;

        public DeckMessageComponent Deck;
        public long UserId;

        public AvatarMessageComponent Avatar;

        public int Unknown3;

        public AvatarMessageComponent Avatar2;

        public int Unknown4;
        public int Unknown5;
        public int Unknown6;

        /// <summary>
        /// Reads the <see cref="AvatarProfileResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarProfileResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
		{
			ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadRRInt32(); // 1
            Unknown2 = reader.ReadRRInt32(); // 0

            Deck = new DeckMessageComponent();
            Deck.ReadMessageComponent(reader);

            UserId = reader.ReadInt64();

            Avatar = new AvatarMessageComponent();
            Avatar.ReadMessageComponent(reader);

            Unknown3 = reader.ReadRRInt32(); // 0

            Avatar2 = new AvatarMessageComponent();
            Avatar2.ReadMessageComponent(reader);

            Unknown4 = reader.ReadRRInt32();
            Unknown5 = reader.ReadRRInt32();
            Unknown6 = reader.ReadRRInt32();
        }

        /// <summary>
        /// Writes the <see cref="AvatarProfileResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarProfileResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
		{
			ThrowIfWriterNull(writer);
		}
	}
}
