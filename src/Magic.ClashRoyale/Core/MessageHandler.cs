using Magic.Logic;
using Magic.Network;
using Magic.Network.Cryptography.NaCl;
using Magic.Network.Messages;
using Magic.Network.Messages.Client;
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
                UserId = 1,
                HomeId = 1,
                UserToken = "randomstuffxd",
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
            var ohd = Utils.ReadMessage<OwnHomeDataMessage>("bin5.bin");

            client.SendMessage(ls);
            client.SendMessage(ohd);

            return Task.FromResult<object>(null);
        }
    }
}
