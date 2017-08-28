using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Add_To_Bookmark : Message
    {
        internal long ID;

        public Add_To_Bookmark(Device device, Reader reader) : base(device, reader)
        {
            // Add_To_Bookmark.
        }

        public override void Decode()
        {
            ID = Reader.ReadInt64();
        }

        public override void Process()
        {
            var done = true;
            try
            {
                Device.Player.Avatar.Bookmarks.Add(ID);
            }
            catch (Exception e)
            {
                ExceptionLogger.Log(e, $"Failed to add {ID} to player {Device.Player.Avatar.UserId} bookmark list");
                done = false;
                throw;
            }
            finally
            {
                new Bookmark_Add_Response(Device) {Response = done}.Send();
            }
        }
    }
}
