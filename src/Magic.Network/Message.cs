using System;

namespace Magic.Network
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Represents the maximum size in bytes of a Clash of Clans message. This field is
        /// constant
        /// </summary>
        public const int MaxSize = 16777215; // (2^24) - 1

        /// <summary>
        /// Represents the minimum size in bytes of a Clash of Clans message. This field is
        /// constant.
        /// </summary>
        public const int MinSize = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Message"/>.
        /// </summary>
        public virtual ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the <see cref="Message"/>.
        /// </summary>
        public virtual ushort Version { get; set; }

        /// <summary>
        /// Reads the <see cref="Message"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Message"/>.
        /// </param>
        public virtual void Decode(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="Message"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Message"/>.
        /// </param>
        public virtual void Encode(MessageWriter writer)
        {
            // Space
        }

        /// <summary>
        /// Gets the <see cref="MessageDirection"/> of the specified message ID.
        /// </summary>
        /// <param name="messageId">Message ID to get its direction.</param>
        /// <returns><see cref="MessageDirection"/> of the specified message ID.</returns>
        public static MessageDirection GetMessageDirection(int messageId) => messageId >= 20000 ? MessageDirection.Client : MessageDirection.Server;

        /// <summary>
        /// Gets the <see cref="MessageDirection"/> of the specified <see cref="Message"/> based on its
        /// message ID.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to get its direction.</param>
        /// <returns><see cref="MessageDirection"/> of the specified <see cref="Message"/>.</returns>
        public static MessageDirection GetMessageDirection(Message message) => GetMessageDirection(message.Id);

        /// <summary>
        /// Gets the <see cref="MessageDirection"/> of the specified <see cref="Message"/> type based on its
        /// message ID.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Message"/> to get its direction.</typeparam>
        /// <returns><see cref="MessageDirection"/> of the specified <see cref="Message"/> type.</returns>
        public static MessageDirection GetMessageDirection<T>() where T : Message => GetMessageDirection((T)Activator.CreateInstance(typeof(T)));

        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }

        internal void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
        }
    }
}
