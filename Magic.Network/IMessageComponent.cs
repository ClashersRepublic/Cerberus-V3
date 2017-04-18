namespace Magic.Network
{
    public interface IMessageComponent
    {
        void Decode(MessageReader reader);

        void Encode(MessageWriter writer);
    }
}
