using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;

namespace CR.Servers.CoC.Logic.Chat
{
    internal class Chats : Dictionary<int, ConcurrentStack<Chat>>
    {
        internal Chats()
        {
            foreach (LocaleData Data in CSV.Tables.Get(Gamefile.Locales).Datas)
            {
                for (int i = 0; i < Math.Max(Constants.MaxPlayers / 25, 1); i++)
                {
                    if (!this.ContainsKey(Data.GlobalId))
                    {
                        this.Add(Data.GlobalId, new ConcurrentStack<Chat>());
                    }

                    this[Data.GlobalId].Push(new Chat());
                }
            }
            
        }

        internal void Join(Device Device)
        {
            if (Device.Connected)
            {
                if (Device.Chat == null && Device.GameMode.Level.Player != null)
                {
                    if (Device.Info.LocaleData.GlobalId != 0)
                    {
                        if (this.TryGetValue(Device.Info.LocaleData.GlobalId, out ConcurrentStack<Chat> Chats))
                        {
                            if (!Chats.TryPop(out Chat Chat))
                            {
                                Chat = new Chat();
                            }

                            if (Chat.TryAdd(Device))
                            {
                                Device.Chat = Chat;
                                Chats.Push(Chat);
                            }
                        }
                    }
                }
            }
        }
    }
}