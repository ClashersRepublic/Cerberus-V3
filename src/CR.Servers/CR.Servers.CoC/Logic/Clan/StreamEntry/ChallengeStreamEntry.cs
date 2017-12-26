﻿using System;
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
    internal class ChallengeStreamEntry : StreamEntry
    {
        internal override AllianceStream Type => AllianceStream.FriendChallenge;

        public ChallengeStreamEntry()
        {

        }

        internal string Message;

        public ChallengeStreamEntry(Member Member) : base(Member)
        {
        }

        internal override void Encode(List<byte> Packet)
        {
            base.Encode(Packet);
            Packet.AddString(this.Message);
        }

        internal override void Load(JToken Json)
        {
            base.Load(Json);

            if (JsonHelper.GetJsonString(Json, "message", out string Message))
            {
                this.Message = Message ?? string.Empty;
            }
            else
            {
                this.Message = string.Empty;
            }
        }

        internal override JObject Save()
        {
            JObject Json = base.Save();
            Json.Add("message", this.Message);
            return Json;
        }
    }
}