using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Remove_From_Bookmark : Message
    {
        internal long ID;

        public Remove_From_Bookmark(Device device, Reader reader) : base(device, reader)
        {
            // Remove_From_Bookmark.
        }

        public override void Decode()
        {
            ID = Reader.ReadInt64();
        }

        public override void Process()
        {
            var done = false;
            try
            {
                done = Device.Player.Avatar.Bookmarks.Remove(ID);
            }
            catch (Exception e)
            {
                ExceptionLogger.Log(e, $"Failed to remove {ID} to player {Device.Player.Avatar.UserId} bookmark list");
                done = false;
                throw;
            }
            finally
            {
                new Bookmark_Remove_Response(Device) { Response = done }.Send();
            }
        }
    }
}
