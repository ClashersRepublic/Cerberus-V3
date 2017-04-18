using System;

namespace Magic.Network.Messages.Components
{
    public class UnknownMessageComponent : MessageComponent
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadRRInt32();
            Unknown2 = reader.ReadRRInt32();

            Unknown3 = reader.ReadRRInt32();
            Unknown4 = reader.ReadRRInt32();
            Unknown5 = reader.ReadRRInt32();
            Unknown6 = reader.ReadRRInt32();
            Unknown7 = reader.ReadRRInt32();
            Unknown8 = reader.ReadRRInt32();
            Unknown9 = reader.ReadRRInt32();

            Unknown10 = reader.ReadRRInt32();
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1, true);
            writer.Write(Unknown2, true);

            writer.Write(Unknown3, true);
            writer.Write(Unknown4, true);
            writer.Write(Unknown5, true);
            writer.Write(Unknown6, true);
            writer.Write(Unknown7, true);
            writer.Write(Unknown8, true);
            writer.Write(Unknown9, true);

            writer.Write(Unknown10, true);
        }
    }
}
