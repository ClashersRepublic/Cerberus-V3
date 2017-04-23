using Magic.Network;
using System;

namespace Magic.Logic
{
    public struct Timer : IMessageComponent
    {
        public TimeSpan Done;
        public TimeSpan Total;
        public DateTime End;

        public void ReadMessageComponent(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            Done = ReadTimeSpan(reader);
            Total = ReadTimeSpan(reader);
            End = TimeUtils.FromUnixTimestamp(reader.ReadRRInt32());
        }

        public void WriteMessageComponent(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            WriteTimeSpan(writer, Done);
            WriteTimeSpan(writer, Total);
            writer.Write(End == DateTime.MinValue ? 63 : (int)TimeUtils.ToUnixTimestamp(End), true);
        }

        private static TimeSpan ReadTimeSpan(MessageReader reader) => TimeSpan.FromSeconds(reader.ReadRRInt32() / 20d);

        private static void WriteTimeSpan(MessageWriter writer, TimeSpan value)
        {
            writer.Write((int)value.TotalSeconds * 20, true);
        }
    }
}
