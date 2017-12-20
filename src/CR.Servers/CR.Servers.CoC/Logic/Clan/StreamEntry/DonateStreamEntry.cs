using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.List;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Clan
{
    internal class DonateStreamEntry : StreamEntry
    {
        internal override AllianceStream Type => AllianceStream.Donate;

        public DonateStreamEntry()
        {

        }

        public DonateStreamEntry(Member Member) : base(Member)
        {
        }

        internal int MaxTroop;
        internal int MaxSpell;
        internal int UsedTroop;
        internal int UsedSpell;
        internal bool HaveMessage;
        internal string Message;

        internal int UsedTroopSend; //Do not save this
        internal AllianceUnitSlots Units = new AllianceUnitSlots();

        internal override void Encode(List<byte> Packet)
        {
            base.Encode(Packet);

            Packet.AddInt(this.LowId);
            Packet.AddInt(this.MaxTroop);
            Packet.AddInt(this.MaxSpell);
            Packet.AddInt(this.UsedTroopSend);
            Packet.AddInt(this.UsedSpell);


            var requestorDonatedUnit = this.Units.FindAll(T => T.DonatorId == this.RequesterId);

            if (requestorDonatedUnit.Count > 0)
            {
                Packet.AddInt(1);
                Packet.AddLong(this.RequesterId);

                Packet.AddInt(requestorDonatedUnit.Sum(unit => unit.Count));

                foreach (UnitItem Unit in requestorDonatedUnit)
                {
                    for (int i = 0; i < Unit.Count; i++)
                    {
                        Packet.AddInt(Unit.Data);
                        Packet.AddInt(Unit.Level);
                    }
                }
            }
            else
            {
                Packet.AddInt(0);
            }

            Packet.AddBool(this.HaveMessage);
            if (this.HaveMessage)
                Packet.AddString(this.Message);

            var donatedUnits = this.Units.FindAll(T => T.DonatorId != this.RequesterId);

            Packet.AddInt(donatedUnits.Count);

            foreach (var donatedUnit in donatedUnits)
            {
                Packet.AddInt(donatedUnit.Data);
                Packet.AddInt(donatedUnit.Count);
                Packet.AddInt(donatedUnit.Level);
            }
        }

        internal override void Load(JToken Json)
        {
            base.Load(Json);

            if (JsonHelper.GetJsonString(Json, "message", out string Message))
            {
                this.Message = Message;
                this.HaveMessage = true;
            }
            else
            {
                this.Message = string.Empty;
                this.HaveMessage = false;
            }

            JsonHelper.GetJsonNumber(Json, "max_troop", out this.MaxTroop);
            JsonHelper.GetJsonNumber(Json, "max_spell", out this.MaxSpell);
            JsonHelper.GetJsonNumber(Json, "used_troop", out this.UsedTroop);
            JsonHelper.GetJsonNumber(Json, "used_spell", out this.UsedSpell);

            JArray units = (JArray)Json["units"];

            if (units != null)
            {
                this.Units.Load(units);
            }

        }

        internal override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("message", this.Message);
            Json.Add("max_troop", this.MaxTroop);
            Json.Add("max_spell", this.MaxSpell);
            Json.Add("used_troop", this.UsedTroop);
            Json.Add("used_spell", this.UsedSpell);
           
            Json.Add("units", this.Units.Save());
            return Json;
        }

    }
}
