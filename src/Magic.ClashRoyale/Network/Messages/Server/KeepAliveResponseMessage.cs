using System;

namespace Magic.Network.Messages.Server
{
    /// <summary>
    /// Message that is sent to confirm that connection to server is alive.
    /// </summary>
    public class KeepAliveResponseMessage : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeepAliveResponseMessage"/> class.
		/// </summary>
		public KeepAliveResponseMessage()
		{
			// Space
		}

		/// <summary>
		/// Gets the ID of the <see cref="KeepAliveResponseMessage"/>.
		/// </summary>
		public override ushort Id => 20108;

		/// <summary>
		/// Reads the <see cref="KeepAliveResponseMessage"/> from the specified <see cref="MessageReader"/>.
		/// </summary>
		/// <param name="reader">
		/// <see cref="MessageReader"/> that will be used to read the <see cref="KeepAliveResponseMessage"/>.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public override void ReadMessage(MessageReader reader)
		{
			ThrowIfReaderNull(reader);
		}

		/// <summary>
		/// Writes the <see cref="KeepAliveResponseMessage"/> to the specified <see cref="MessageWriter"/>.
		/// </summary>
		/// <param name="writer">
		/// <see cref="MessageWriter"/> that will be used to write the <see cref="KeepAliveResponseMessage"/>.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
		public override void WriteMessage(MessageWriter writer)
		{
			ThrowIfWriterNull(writer);
		}
	}
}
