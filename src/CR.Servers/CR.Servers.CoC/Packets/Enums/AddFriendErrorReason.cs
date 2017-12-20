using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Servers.CoC.Packets.Enums
{
    public enum AddFriendErrorReason
    {
        Unknown = 0,
        YouSendTooManyRequest = 1,
        TargetHasTooManyRequest = 2,
        TryToAddOwnAvatar = 4,
        AvatarNotExist = 5,
        YouHaveTooManyFriend = 7,
        TargetHaveTooManyFriend = 8,

    }
}
