using System;

namespace Magic.Network.Messages.Client
{
	/// <summary>
	/// Message that is sent to server to request if connection to server is alive.
	/// </summary>
	public class KeepAliveRequestMessage : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeepAliveRequestMessage"/> class.
		/// </summary>
		public KeepAliveRequestMessage()
		{
			// Space
		}

		/// <summary>
		/// Gets the ID of the <see cref="KeepAliveRequestMessage"/>.
		/// </summary>
		public override ushort Id => 10108;

		/// <summary>
		/// Reads the <see cref="KeepAliveRequestMessage"/> from the specified <see cref="MessageReader"/>.
		/// </summary>
		/// <param name="reader">
		/// <see cref="MessageReader"/> that will be used to read the <see cref="KeepAliveRequestMessage"/>.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public override void ReadMessage(MessageReader reader)
		{
			ThrowIfReaderNull(reader);
		}

		/// <summary>
		/// Writes the <see cref="KeepAliveRequestMessage"/> to the specified <see cref="MessageWriter"/>.
		/// </summary>
		/// <param name="writer">
		/// <see cref="MessageWriter"/> that will be used to write the <see cref="KeepAliveRequestMessage"/>.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
		public override void WriteMessage(MessageWriter writer)
		{
			ThrowIfWriterNull(writer);
		}
	}
}
