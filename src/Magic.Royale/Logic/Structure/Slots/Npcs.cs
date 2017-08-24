﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Npcs : List<Npc>
    {
        internal Npcs()
        {
            // Npcs.
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Count);
                foreach (Npc Npc in this)
                {
                    Packet.AddInt(Npc.NPC_Id);
                    Packet.AddInt(Npc.Star_Gained);
                }
                Packet.AddInt(0);
                Packet.AddInt(0);

                /* Packet.AddInt(this.Count);
                 foreach (Npc Npc in this)
                 {
                     Packet.AddInt(Npc.Elixir_Looted);
                 }

                 Packet.AddInt(this.Count);
                 foreach (Npc Npc in this)
                 {
                     Packet.AddInt(Npc.Gold_Looted);
                 }*/

                return Packet.ToArray();
            }
        }
    }
}