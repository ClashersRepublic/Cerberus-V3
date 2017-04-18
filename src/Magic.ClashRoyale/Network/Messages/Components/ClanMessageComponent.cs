namespace Magic.Network.Messages.Components
{
    public class ClanMessageComponent : MessageComponent
    {
        public long Id;
        public string Name;

        public int Unknown1;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadRRInt64();
            Name = reader.ReadString();

            Unknown1 = reader.ReadRRInt32(); // 114, Badge?
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id, true);
            writer.Write(Name);

            writer.Write(Unknown1, true);
        }
    }
}
