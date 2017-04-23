using Magic.Logic;
using Magic.Network;
using Magic.Network.Cryptography.NaCl;
using Magic.Network.Messages;
using Magic.Network.Messages.Client;
using Magic.Network.Messages.Components;
using Magic.Network.Messages.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class MessageHandler
    {
        private delegate Task Handler(Message message, Client client);

        public MessageHandler(Server server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _handlers = new Dictionary<ushort, Handler>();

            _handlers.Add(new HandshakeRequestMessage().Id, HandleHandshakeRequest);
            _handlers.Add(new LoginRequestMessage().Id, HandleLoginRequest);
            _handlers.Add(new KeepAliveRequestMessage().Id, HandleKeepAliveRequest);
            _handlers.Add(new BattleNpcMessage().Id, HandleBattleNpc);
        }

        private Task HandleBattleNpc(Message message, Client client)
        {
            var ohd = new UnknownMessage
            {
                Id = 21903,
                DecryptedBytes = File.ReadAllBytes("npc.bin")
            };

            client.SendMessage(ohd);

            return Task.FromResult<object>(null);
        }

        private readonly Server _server;
        private readonly Dictionary<ushort, Handler> _handlers;

        public async void HandleAsync(Message message, Client client)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var handler = default(Handler);
            if (_handlers.TryGetValue(message.Id, out handler))
                await handler(message, client);
            else
                Console.WriteLine("Message with ID {0} was unhandled.", message.Id);
        }

        private Task HandleHandshakeRequest(Message message, Client client)
        {
            client.Connection._id = PublicKeyBox.GenerateNonce();
            var hs = new HandshakeSuccessMessage
            {
                SessionKey = client.Connection._id
            };
            client.SendMessage(hs);
            return Task.FromResult<object>(null);
        }

        private static readonly KeepAliveResponseMessage s_keepAliveResponse = new KeepAliveResponseMessage();
        private Task HandleKeepAliveRequest(Message message, Client client)
        {
            client.SendMessage(s_keepAliveResponse);
            return Task.FromResult<object>(null);
        }

        private Task HandleLoginRequest(Message message, Client client)
        {
            var lr = (LoginRequestMessage)message;
            var level = default(Level);
            if (lr.UserId == 0)
            {
                if (lr.UserToken != null)
                {
                    // Error we can fuck off.
                    return Task.FromResult<object>(null);
                }
                else
                {
                    // Create new acc.
                }
            }
            else
            {
                if (lr.UserToken == null)
                {
                    // Error we can fuck off.
                    return Task.FromResult<object>(null);
                }
                else
                {
                    // find acc from db.
                }
            }

            var ls = new LoginSuccessMessage
            {
                UserId = lr.UserId,
                HomeId = lr.UserId,
                UserToken = lr.UserToken,
                MajorVersion = 3,
                BuildVersion = 193,
                BuildVersion2 = 193,
                DateCreated = DateTime.UtcNow,
                DateLastSave = DateTime.UtcNow,
            };

            //var ohd = new UnknownMessage
            //{
            //    Id = 24101,
            //    DecryptedBytes = File.ReadAllBytes("bin7.bin")
            //};
            var ohd2 = Utils.ReadMessage<OwnHomeDataMessage>("bin5.bin");
            //ohd2.Avatar.Username = "CrackRoyale";

            var ohd = new OwnHomeDataMessage
            {
                UserId = lr.UserId,

                Unknown1 = 32,
                Unknown2 = 4,

                UnknownTimer1 = ohd2.UnknownTimer1,

                Unknown6 = 3,
                Unknown7 = 255,
                Unknown8 = 127,
                Unknown9 = 63,
                Unknown10 = 1491827316,
                Unknown11 = 1,

                BattleDeck = ohd2.BattleDeck,
                Deck = ohd2.Deck,
                Deck2 = ohd2.Deck2,
                BattleCards = ohd2.BattleCards,
                UnusedCards = ohd2.UnusedCards,

                ChallengeName = string.Empty,

                Unknown17 = 63,

                Unknown76 = 2, // Critical can crash app

                UnknownJsonData = "{\"ID\":\"CARD_RELEASE\",\"Params\":{\"Assassin\":\"20170324\"}}",
                Unknown21 = 4,

                Unknown22 = 0,
                //Chests = ohd2.Chests,

                Unknown26 = 0,

                Unknown53 = 15,
                Unknown55 = 2,

                ExpLevels = 1,
                ArenaType = 54,
                ArenaId = 1,

                Unknown34 = 542354048,

                DayOfWeek = 1,
                DayOfWeek2 = 1,

                Unknown35 = 3,
                ShopItems = ohd2.ShopItems,

                Unknown45 = 16844288,

                Unknown48 = ohd2.Unknown48,
                UnknownCharacterArray = ohd2.UnknownCharacterArray,

                Unknown50 = 9,

                Unknown52 = 248,

                Unknown56 = 7,
                UnknownComponent1 = ohd2.UnknownComponent1,
                Unknown57 = 54000010,

                Unknown68 = 1,

                //Avatar = ohd2.Avatar,
                Avatar = new AvatarMessageComponent
                {
                    UserId = lr.UserId,
                    HomeId = lr.UserId,
                    UserId2 = lr.UserId,

                    Username = "CrackRoyale",
                    Unknown4 = 2,
                    Unknown5 = 315,

                    ExpLevels = 1,
                    ExpPoints = 1,

                    Unknown10 = 30,

                    Unknown13 = 7,

                    Resources = ohd2.Avatar.Resources,
                    CompletedAchievements = ohd2.Avatar.CompletedAchievements,
                    Achievements = ohd2.Avatar.Achievements,
                    Stats = ohd2.Avatar.Stats,
                    Characters = ohd2.Avatar.Characters,

                    Unknown19 = 9,
                    Clan = ohd2.Avatar.Clan,

                    Unknown20 = 1,
                    Unknown21 = 16,

                    Unknown24 = 11,
                    Unknown25 = 5,
                    Unknown26 = 3,
                    Unknown27 = 8,
                },

                Unknown72 = 170,

                Unknown73 = 12022299,
                Unknown74 = 1491892504,
                Unknown75 = 1733095
            };

            client.SendMessage(ls);
            client.SendMessage(ohd);

            return Task.FromResult<object>(null);
        }
    }
}
