using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Messages.Server.Clans;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Send_Alliance_Invitation : Message
    {
        internal long UserID;

        public Send_Alliance_Invitation(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            UserID = Reader.ReadInt64();
        }

        public override void Process()
        {
            var avatar = Device.Player.Avatar;
            var target = ResourcesManager.GetPlayer(UserID);
            if (target != null)
                if (target.Avatar.Castle_Level > -1)
                    if (target.Avatar.ClanId == 0)
                        if (target.Avatar.Inbox.Slots.FindAll(M => M.Stream_Type == Avatar_Stream.INVITATION).Count < 10)
                        {
                            var has_been_send =
                                target.Avatar.Inbox.Slots.FindAll(M => M.Sender_ID == avatar.UserId &&
                                                                       M.Stream_Type == Avatar_Stream.INVITATION);
                            var same_alliance =
                                target.Avatar.Inbox.Slots.FindAll(M => M.Alliance_ID == avatar.ClanId &&
                                                                       M.Stream_Type == Avatar_Stream.INVITATION);
                            if (has_been_send.Count == 0 && same_alliance.Count == 0)
                            {
                                target.Avatar.Inbox.Add(new Mail
                                {
                                    Stream_Type = Avatar_Stream.INVITATION,
                                    Sender_ID = avatar.UserId,
                                    Sender_Name = avatar.Name,
                                    Sender_Level = avatar.Level,
                                    Sender_League = avatar.League,
                                    Alliance_ID = avatar.ClanId
                                });
                                new Alliance_Invitation_SentOk(Device).Send();
                            }
                            else
                            {
                                new Alliance_Invitation_SendFailed(Device)
                                    {
                                        Response = Alliance_Invite.Already_Has_An_Invite
                                    }
                                    .Send();
                            }
                        }
                        else
                            new Alliance_Invitation_SendFailed(Device) {Response = Alliance_Invite.Has_To_Many_Invite}.Send();
                    else
                        new Alliance_Invitation_SendFailed(Device) {Response = Alliance_Invite.Already_In_Alliance}.Send();
                else
                    new Alliance_Invitation_SendFailed(Device) {Response = Alliance_Invite.No_Castle}.Send();
            else
                new Alliance_Invitation_SendFailed(Device) {Response = Alliance_Invite.Banned}.Send();
        }
    }
}