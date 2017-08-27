using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Royale.Extensions.Binary;

namespace Magic.Royale.Network.Commands.Client
{
    internal class Set_Active_Deck : Command
    {
        internal int Tick_Start;
        internal int Tick_End;
        internal long AccountID;
        internal int Slot;

        public Set_Active_Deck(Reader Reader, Device Device, int _ID) : base(Reader, Device, _ID)
        {

        }

        public override void Decode()
        {
            Tick_Start = Reader.ReadVInt();
            Tick_End = Reader.ReadVInt();
            AccountID = Reader.ReadVLong();
            Slot = Reader.ReadVInt();
        }

        public override void Process()
        {
            Device.Player.Active_Deck = Slot;
        }
    }
}