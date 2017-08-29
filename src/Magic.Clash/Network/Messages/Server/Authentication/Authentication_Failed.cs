using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Magic.Files;

namespace Magic.ClashOfClans.Network.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            Identifier = 20103;
            Version = 3;
            this.Reason = Reason;
        }


        internal Reason Reason = Reason.Default;

        internal string PatchingHost => Fingerprint.Custom
            ? Constants.PatchServer
            : "https://www.clashersrepublic.com/game-content/projectmagic/";

        internal string Message;
        internal string RedirectDomain;

        public override void Encode()
        {
            Data.AddInt((int) Reason);
            Data.AddString(Reason == Reason.Patch ? Fingerprint.Json : null);
            Data.AddString(RedirectDomain);
            Data.AddString(PatchingHost);
            Data.AddString(Constants.UpdateServer);
            Data.AddString(Message);
            Data.AddInt(Reason == Reason.Maintenance ? 0 : 1);
            Data.AddByte(0);
            Data.AddCompressed(Reason == Reason.Patch ? Fingerprint.Json : null, false);
            Data.AddInt(-1);
            Data.AddInt(2);
            Data.AddInt(0);
            Data.AddInt(-1);
        }
    }
}
