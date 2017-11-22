using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, LoginFailedReason Reason = LoginFailedReason.Default) : base(Device)
        {
            this.Reason = Reason;
            Version = 9;
        }

        internal override short Type => 20103;

        internal LoginFailedReason Reason;
        internal string PatchingHost => Fingerprint.Custom  ? Settings.PatchServer  : "https://www.clashersrepublic.com/game-content/projectmagic/";


        internal string Message;
        internal string RedirectDomain;

        internal override void Encode()
        {
            Data.AddInt((int)Reason);
            Data.AddString(null);
            Data.AddString(this.RedirectDomain);
            Data.AddString(null); //Old Patching Host
            Data.AddString(Settings.UpdateServer);
            Data.AddString(this.Message);
            Data.AddInt(Reason == LoginFailedReason.Maintenance ? 0 : 1); // Remaining time
            Data.AddByte(0);
            Data.AddCompressed(Reason == LoginFailedReason.Patch ? Fingerprint.Json : null, false);
            Data.AddInt(2);
            Data.AddString(Settings.GameAssetsServer);
            Data.AddString(PatchingHost);
            Data.AddInt(0);
            Data.AddInt(0);
            Data.AddInt(-1);
            Data.AddInt(-1);
        }
    }
}
