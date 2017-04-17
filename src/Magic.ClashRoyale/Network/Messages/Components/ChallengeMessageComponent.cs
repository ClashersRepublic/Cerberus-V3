namespace Magic.Network.Messages.Components
{
    public class ChallengeMessageComponent : MessageComponent
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public long Unknown4;

        public string Name;
        public string Data;

        public int Unknown5;

        public string NameFinal;

        public int Unknown6;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            // Timestamps?
            Unknown1 = reader.ReadRRInt32(); // 1490860800
            Unknown2 = reader.ReadRRInt32(); // 1491206400
            Unknown3 = reader.ReadRRInt32(); // 1490688000

            Unknown4 = reader.ReadInt64(); // 0

            Name = reader.ReadString();
            Data = reader.ReadString();

            Unknown5 = reader.ReadRRInt32(); // 30

            NameFinal = reader.ReadString();

            Unknown6 = reader.ReadRRInt32(); // 1
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1, true);
            writer.Write(Unknown2, true);
            writer.Write(Unknown3, true);

            writer.Write(Unknown4);

            writer.Write(Name);
            writer.Write(Data);

            writer.Write(Unknown5, true);
            
            writer.Write(NameFinal);

            writer.Write(Unknown6, true);
        }
    }
}
