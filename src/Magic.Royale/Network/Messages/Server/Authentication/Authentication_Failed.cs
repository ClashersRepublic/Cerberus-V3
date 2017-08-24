using Magic.Royale.Core.Settings;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Enums;
using Magic.Files;

namespace Magic.Royale.Network.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            Identifier = 20103;
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
