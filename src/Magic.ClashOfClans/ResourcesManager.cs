using Magic.ClashOfClans.Network;
using System;
using System.Collections.Generic;

namespace Magic.ClashOfClans
{
    internal class ResourcesManager
    {
        private static readonly ResourcesManager s_instance = new ResourcesManager();

        public static ResourcesManager Instance => s_instance;

        public ResourcesManager()
        {
            _unloggedInClients = new List<Client>();
            _loggedInClients = new Dictionary<long, Client>();
        }

        // Clients that have connected but haven't logged in yet.
        private readonly List<Client> _unloggedInClients;
        // Clients that have logged in and has a Level associated with them.
        private readonly Dictionary<long, Client> _loggedInClients;

        public void RegisterClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _unloggedInClients.Add(client);
        }
    }
}
