using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network
{
    internal class GameOpCommand
    {
        public static void SendCommandFailedMessage(Device c)
        {

        }

        public virtual void Execute(Level level)
        {
            // Space
        }

        public byte RequiredPrivileges { get; set; }
    }
}