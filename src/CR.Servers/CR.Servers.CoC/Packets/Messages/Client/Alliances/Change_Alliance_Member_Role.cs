using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Change_Alliance_Member_Role : Message
    {
        internal override short Type => 14306;

        public Change_Alliance_Member_Role(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int HighId;
        internal int LowId;
        internal Role Role;

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
            this.Role = (Role) this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Level = this.Device.GameMode.Level;
            var Target = Resources.Accounts.LoadAccount(this.HighId, this.LowId)?.Player;

            if (Target != null)
            {
                if (Level.Player.InAlliance)
                {
                    var Alliance = Level.Player.Alliance;
                    var Executer = Alliance.Members.Get(Level.Player.UserId);
                    var TargetMember = Alliance.Members.Get(Level.Player.UserId);

                    if (Executer != null)
                    {
                        if (TargetMember != null)
                        {
                            Role ExecuterRole = Executer.Role;
                            Role CurrentRole = TargetMember.Role;

                            if (ExecuterRole == Role.Leader || ExecuterRole == Role.CoLeader)
                            {
                                if (this.Role == Role.Leader)
                                {

                                }
                                else
                                {
                                    TargetMember.Role = this.Role;
                                    if (Target.Connected)
                                    {
                                        Target.Level.GameMode.CommandManager.AddCommand(
                                            new Changed_Alliance_Role(Target.Level.GameMode.Device)
                                            {
                                                AllianceId = Level.Player.AllianceId,
                                                AllianceRole = this.Role
                                            });
                                    }

                                    Alliance.Streams.AddEntry(new EventStreamEntry(Executer, Executer,
                                        this.Role.Superior(CurrentRole)
                                            ? AllianceEvent.Promoted
                                            : AllianceEvent.Demoted));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
