using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magic.Network
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
        }
        
        private readonly Server _server;
        private readonly Dictionary<ushort, Handler> _handlers;
    }
}
