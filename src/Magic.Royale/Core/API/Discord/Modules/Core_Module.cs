using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Network;

namespace Magic.ClashOfClans.Core.API.Discord.Modules
{
    [Name("Core")]
    internal class Core_Module : ModuleBase<SocketCommandContext>
    {
        internal CommandService _CService;

        public Core_Module(CommandService CService)
        {
            _CService = CService;
        }

        [Command("status", RunMode = RunMode.Async)]
        public async Task Status()
        {
            var _Builder = new EmbedBuilder
            {
                Color = new Color(255, 131, 0),
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Requested by @{Context.User.Username}"
                },
                Timestamp = DateTime.UtcNow,
            };
            _Builder.WithTitle("Savage Magic Status");

            _Builder.AddInlineField("SocketAsyncEventArgs", $"Created: {Gateway.NumberOfArgsCreated}\nIn-use: {Gateway.NumberOfArgsInUse}\nAvailable: {Gateway.NumberOfArgs}");
            _Builder.AddInlineField("Buffer", $"Created: {Gateway.NumberOfBuffersCreated}\nIn-use: {Gateway.NumberOfBuffersInUse}\nAvailable: {Gateway.NumberOfBuffers}");
   
            _Builder.AddInlineField("Players Online", $"{ResourcesManager.OnlinePlayers.Count}");
            _Builder.AddInlineField("Saved Players", ObjectManager.AvatarSeed - 1);
            
                 
            await ReplyAsync("", false, _Builder);
        }
    }
}