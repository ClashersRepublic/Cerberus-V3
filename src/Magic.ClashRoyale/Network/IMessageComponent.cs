namespace Magic.Network
{
    public interface IMessageComponent
    {
        void ReadMessageComponent(MessageReader reader);

        void WriteMessageComponent(MessageWriter writer);
    }
}
