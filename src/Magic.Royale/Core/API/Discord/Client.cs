using System;
using Discord;
using Discord.WebSocket;
using Magic.Royale.Core.Settings;

namespace Magic.Royale.Core.API.Discord
{
    internal static class Client
    {
        internal static DiscordSocketClient _Client;
        internal static CommandHandler CommandHandler;

        internal static async void Initialize()
        {
            try
            {
                _Client = new DiscordSocketClient(
                    new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Error,
                    });

                CommandHandler = new CommandHandler();
                CommandHandler.InstallAsync(_Client).Wait();
                
                await _Client.LoginAsync(TokenType.Bot, Constants.DiscordToken);
                await _Client.StartAsync();
                await _Client.SetGameAsync("Hearing for Status request");
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Fail to connect to Discord API server");
            }
        }
        internal static async void Deinitialize()
        {
            try
            {
                await _Client.LogoutAsync();
                await _Client.StopAsync();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Fail to diconnect from Discord API server");
            }
        }
    }
}