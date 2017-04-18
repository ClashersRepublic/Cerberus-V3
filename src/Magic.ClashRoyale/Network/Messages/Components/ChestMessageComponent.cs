using System;

namespace Magic.Network.Messages.Components
{
    public class ChestMessageComponent : MessageComponent
    {
        public int Type;
        public int Id;
        public int UnlockState;
        public int RemainingTimeSeconds;
        public int TotalTimeSeconds;
        public int EndTimestamp;

        public int Unknown1;
        public int Unknown2;

        public int Index;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;

        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            //TODO: Fix it has some slight issues since it does not properly decode bin2.
            Type = reader.ReadRRInt32(); // Chest type? 19
            Id = reader.ReadRRInt32(); // Chest Id? 9

            // 0 No unlocked?
            // 1 Unlocked and complete?
            // 8 Unlocked in progress?
            UnlockState = reader.ReadRRInt32(); // 1, unlocked? Can be 8 sometimes
            // Not too sure about this.
            if (UnlockState == 8)
            {
                RemainingTimeSeconds = reader.ReadRRInt32() / 20; // 31
                TotalTimeSeconds = reader.ReadRRInt32() / 20; // 1
                EndTimestamp = reader.ReadRRInt32(); // Some form of timestamp.
            }

            Unknown1 = reader.ReadRRInt32(); // 36, 47, 49 sometimes.
            Unknown2 = reader.ReadRRInt32(); // 1

            Index = reader.ReadRRInt32(); // Index(zero-based) of where the chest is in the slots.

            Unknown4 = reader.ReadRRInt32(); // 0
            Unknown5 = reader.ReadRRInt32(); // 0

            // 0 when no more chest in slots.
            // 4 when more chest in slots.
            // 8 when next slot is empty.
            Unknown6 = reader.ReadRRInt32(); 
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Type, true);
            writer.Write(Id, true);
            writer.Write(UnlockState, true);

            if (UnlockState == 8)
            {
                writer.Write(RemainingTimeSeconds * 20, true);
                writer.Write(TotalTimeSeconds * 20, true);
                writer.Write(EndTimestamp, true);
            }

            writer.Write(Unknown1, true);
            writer.Write(Unknown2, true);

            writer.Write(Index, true);

            writer.Write(Unknown4, true);
            writer.Write(Unknown5, true);
            writer.Write(Unknown6, true);
        }

#if DEBUG
        public override string ToString()
        {
            return "Id: " + (Type * 1000000 + Id);
        }
#endif
    }
}
