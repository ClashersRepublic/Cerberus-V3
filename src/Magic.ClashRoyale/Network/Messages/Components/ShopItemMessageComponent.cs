namespace Magic.Network.Messages.Components
{
    public class ShopItemMessageComponent : MessageComponent
    {
        public int Type;

        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;

        public int ItemType;
        public int ItemId;

        public int Unknown11;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Type = reader.ReadRRInt32(); // 1

            Unknown1 = reader.ReadRRInt32(); // 66
            Unknown2 = reader.ReadRRInt32(); // 2
            Unknown3 = reader.ReadRRInt32(); // 3
            Unknown4 = reader.ReadRRInt32(); // 0
            Unknown5 = reader.ReadRRInt32(); // 0
            Unknown6 = reader.ReadRRInt32(); // 0
            Unknown7 = reader.ReadRRInt32(); // 0
            Unknown8 = reader.ReadRRInt32(); // 12, 20

            ItemType = reader.ReadRRInt32(); // 28, 26 Actual item's type
            ItemId = reader.ReadRRInt32(); // 1, 15 Actual item's id

            Unknown11 = reader.ReadRRInt32(); // 0, 3
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Type, true);

            writer.Write(Unknown1, true);
            writer.Write(Unknown2, true);
            writer.Write(Unknown3, true);
            writer.Write(Unknown4, true);
            writer.Write(Unknown5, true);
            writer.Write(Unknown6, true);
            writer.Write(Unknown7, true);
            writer.Write(Unknown8, true);

            writer.Write(ItemType, true);
            writer.Write(ItemId, true);

            writer.Write(Unknown11, true);
        }
    }
}
