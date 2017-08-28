using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class Members : Dictionary<long, Member>
    {
        internal Clan Alliance;

        internal object Gate = new object();

        public Members()
        {
        }

        internal Members(Clan Alliance) : base(50)
        {
            this.Alliance = Alliance;
        }

        internal void Add(Avatar Player)
        {
            lock (Gate)
            {
                var member = new Member(Player);
                if (ContainsKey(member.UserID))
                {
                    this[member.UserID] = member;
                }
                else
                {
                    if (Count < 1)
                        member.Role = Role.Leader;

                    Add(member.UserID, member);
                }
            }
        }

        internal void Add(Member _Member)
        {
            lock (Gate)
            {
                if (ContainsKey(_Member.UserID))
                    this[_Member.UserID] = _Member;
                else
                    Add(_Member.UserID, _Member);
            }
        }

        internal void Remove(Avatar Player)
        {
            lock (Gate)
            {
                if (ContainsKey(Player.UserId))
                    Remove(Player.UserId);
            }
        }

        internal void Remove(Member Member)
        {
            lock (Gate)
            {
                if (ContainsKey(Member.UserID))
                    Remove(Member.UserID);
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                var _Packet = new List<byte>();

                _Packet.AddInt(Values.Count);

                foreach (var Member in Values.ToList())
                {
                    var _Player = Member.Player;
                    _Packet.AddLong(_Player.Avatar.UserId);
                    _Packet.AddString(_Player.Avatar.Name);
                    _Packet.AddInt((int) Member.Role);
                    _Packet.AddInt(_Player.Avatar.Level);
                    _Packet.AddInt(_Player.Avatar.League);
                    _Packet.AddInt(_Player.Avatar.Trophies);
                    _Packet.AddInt(_Player.Avatar.Builder_Trophies); //Builder Base Trophies
                    _Packet.AddInt(Member.Donations);
                    _Packet.AddInt(Member.Received);
                    _Packet.AddInt(0); // Order ?
                    _Packet.AddInt(0); // Previous Order ?
                    _Packet.AddInt(0); // Builder Base Order ?
                    _Packet.AddInt(0); // Builder Base Previous Order ?=

                    //_Packet.AddInt(Member.Connected ? 1 : 0);
                    _Packet.AddInt(Member.New ? 1 : 0);
                    _Packet.AddInt(0); //War Cooldown
                    _Packet.AddInt(_Player.Avatar.WarState ? 1 : 0);
                    _Packet.AddByte(1);
                    _Packet.AddLong(_Player.Avatar.UserId);
                }
                return _Packet.ToArray();
            }
        }
    }
}
