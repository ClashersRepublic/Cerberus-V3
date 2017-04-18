using System;

namespace Magic.Network
{
    public abstract class MessageComponent : IMessageComponent
    {
        public abstract void Decode(MessageReader reader);

        public abstract void Encode(MessageWriter writer);

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
