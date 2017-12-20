using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Respond_To_Alliance_Join_Request : Message
    {
        internal override short Type => 14321;

        public Respond_To_Alliance_Join_Request(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal long StreamId;
        internal bool Decision;

        internal override void Decode()
        {
            this.StreamId = this.Reader.ReadInt64();
            this.Decision = this.Reader.ReadBoolean();
        }

        internal override void Process()
        {
            var Player = this.Device.GameMode.Level.Player;
            var Alliance = Player.Alliance;

            if (Alliance != null)
            {
                var AllianceMember = Player.AllianceMember;
                if (AllianceMember != null)
                {
                    if (AllianceMember.Role != Role.Member)
                    {
                        var Stream = Alliance.Streams.Get(this.StreamId);

                        if (Stream != null)
                        {
                            if (Stream is JoinRequestStreamEntry JoinRequest)
                            {
                                if (this.Decision)
                                {
                                    var Target = Resources.Accounts.LoadAccount(JoinRequest.SenderHighId, JoinRequest.SenderLowId)?.Player;

                                    if (Target != null)
                                    {
                                        if (Target.AllianceId == 0)
                                        {
                                            foreach (var entry in Target.Inbox.Entries.Values .Where(T => T.Type == AvatarStream.Invitation).ToArray())
                                            {
                                                Target.Inbox.Remove(entry);
                                            }

                                            if (Alliance.Members.Join(Target, out Member Member))
                                            {
                                                Target.SetAlliance(Alliance, Member);
                                                Target.AllianceHighId = Alliance.HighId;
                                                Target.AllianceLowId = Alliance.LowId;

                                                if (Target.Connected)
                                                {
                                                    new Alliance_Full_Entry(this.Device) {Alliance = Alliance}.Send();
                                                    new Alliance_Stream(this.Device) {Alliance = Alliance}.Send();

                                                    Target.Level.GameMode.CommandManager.AddCommand(
                                                        new Joined_Alliance(this.Device)
                                                        {
                                                            AllianceId = Alliance.AllianceId,
                                                            AllianceName = Alliance.Header.Name,
                                                            AllianceBadge = Alliance.Header.Badge,
                                                            AllianceLevel = Alliance.Header.ExpLevel,
                                                            CreateAlliance = false
                                                        }
                                                    );

                                                    Alliance.IncrementTotalConnected();
                                                }
                                                else
                                                {
                                                    Alliance.RefreshTotalConnected();
                                                }

                                                Alliance.Streams.AddEntry(new EventStreamEntry(Member, AllianceMember, AllianceEvent.Accepted));
                                            }
                                            else
                                                Logging.Error(this.GetType(), "Unable to respond to alliance join request. Join() function returned false!");
                                        }
                                        else
                                        {
                                            Alliance.Streams.RemoveEntry(JoinRequest);
                                        }
                                    }
                                    else
                                        Logging.Error(this.GetType(), "Unable to respond to alliance join request. The target player is null!");
                                }

                                JoinRequest.Judge = Player.Name;
                                JoinRequest.State = this.Decision ? InviteState.Accepted : InviteState.Refused;
                                Alliance.Streams.Update(JoinRequest);
                            }
                            else
                                Logging.Error(this.GetType(), "Unable to respond to alliance join request. The stream is not JoinRequestStreamEntry!");
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to respond to alliance join request. The stream is null!");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to respond to alliance join request. The executer have an member role and is not permited to accept join request!");
                }
                else
                    Logging.Error(this.GetType(), "Unable to respond to alliance join request. Alliance member is null and this a major bug, Please inform server developer ASAP!");
            }
            else
                Logging.Error(this.GetType(), "Unable to respond to alliance join request. Alliance is null and this a major bug, Please inform server developer ASAP!");
        }
    }
}
