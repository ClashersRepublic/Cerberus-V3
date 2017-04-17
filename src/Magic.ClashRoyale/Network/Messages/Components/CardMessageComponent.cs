namespace Magic.Network.Messages.Components
{
    public class CardMessageComponent:MessageComponent
    {
        public int Id;
        public int Level;
        public int Unknown1;
        public int Count;
        public int Unknown2;
        public int Unknown3;
        public bool New;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadRRInt32();
            Level = reader.ReadRRInt32();

            Unknown1 = reader.ReadRRInt32();

            Count = reader.ReadRRInt32();

            Unknown2 = reader.ReadRRInt32();
            Unknown3 = reader.ReadRRInt32();

            New = reader.ReadBoolean();
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id, true);
            writer.Write(Level, true);

            writer.Write(Unknown1, true);

            writer.Write(Count, true);

            writer.Write(Unknown2, true);
            writer.Write(Unknown3, true);

            writer.Write(New);
        }
    }
}
