using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Messages.Server.Clans;
using Magic.ClashOfClans.Network.Messages.Server.Errors;
using Resource = Magic.ClashOfClans.Files.CSV_Logic.Resource;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Create_Alliance : Message
    {
        internal string Name;
        internal string Description;
        internal int Badge;
        internal Hiring Type;
        internal int Required_Trophies;
        internal int War_Frequency;
        internal int Origin;
        internal bool War_History;
        internal bool War_Amical;

        public Create_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            Name = Reader.ReadString();
            Description = Reader.ReadString();
            Badge = Reader.ReadInt32();
            Type = (Hiring) Reader.ReadInt32();
            Required_Trophies = Reader.ReadInt32();
            War_Frequency = Reader.ReadInt32();
            Origin = Reader.ReadInt32();
            War_History = Reader.ReadByte() > 0;
            War_Amical = Reader.ReadByte() > 0;
        }

        public override void Process()
        {
            var Avatar = Device.Player;
            var ResourceID = (CSV.Tables.Get(Gamefile.Resources)
                .GetData((CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_RESOURCE") as Globals)
                    .TextValue) as Resource).GetGlobalId();
            var Cost = (CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_COST") as Globals).NumberValue;
            if (Avatar.Avatar.HasEnoughResources(ResourceID, Cost))
            {
                var Clan = new Clan
                {
                    Name = Name,
                    Description = Description,
                    Badge = Badge,
                    Type = Type,
                    Required_Trophies = Required_Trophies,
                    War_Frequency = War_Frequency,
                    Origin = Origin,
                    War_History = War_History,
                    War_Amical = War_Amical
                };

                var Alliance = ObjectManager.CreateClan(Clan);

                Avatar.Avatar.Resources.Minus(ResourceID, Cost);
                Avatar.Avatar.ClanId = Alliance.Clan_ID;
                Avatar.Avatar.Alliance_Level = Alliance.Level;
                Avatar.Avatar.Alliance_Name = Name;
                Avatar.Avatar.Alliance_Role = (int) Role.Leader;
                Avatar.Avatar.Badge_ID = Badge;

                Alliance.Members.Add(Device.Player.Avatar);

                Alliance.Chats.Add(new Entry
                {
                    Stream_Type = Alliance_Stream.EVENT,
                    Sender_ID = Device.Player.Avatar.UserId,
                    Sender_Name = Device.Player.Avatar.Name,
                    Sender_Level = Device.Player.Avatar.Level,
                    Sender_League = Device.Player.Avatar.League,
                    Sender_Role = Role.Leader,
                    Event_ID = Events.JOIN_ALLIANCE,
                    Event_Player_ID = Device.Player.Avatar.UserId,
                    Event_Player_Name = Device.Player.Avatar.Name
                });

                new Alliance_Full_Entry(Device) {Clan = Alliance}.Send();
                new Server_Commands(Device) {Command = new Joined_Alliance(Device) {Clan = Alliance}.Handle()}.Send();
                new Server_Commands(Device)
                {
                    Command = new Role_Update(Device) {Clan = Alliance, Role = (int) Role.Leader}.Handle()
                }.Send();
                DatabaseManager.Save(Alliance);
                DatabaseManager.Save(Avatar);
            }
            else
            {
                new Out_Of_Sync(Device).Send();
            }
        }
    }
}
