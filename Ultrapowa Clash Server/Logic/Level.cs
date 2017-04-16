using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UCS.Logic.Manager;
using UCS.PacketProcessing;

namespace UCS.Logic
{
    internal class Level
    {
        public GameObjectManager GameObjectManager;
        public WorkerManager WorkerManager;

        private DateTime m_vTime;
        private Client m_vClient;
        private byte m_vAccountPrivileges;
        private byte m_vAccountStatus;
        private string m_vIPAddress;
        private readonly ClientAvatar m_vClientAvatar;

        public Level()
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);
            m_vClientAvatar = new ClientAvatar();
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
            m_vIPAddress = "0.0.0.0";
        }

        public Level(long id, string token)
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);
            m_vClientAvatar = new ClientAvatar(id, token);
            m_vTime = DateTime.UtcNow;
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
            m_vIPAddress = "0.0.0.0";
        }

        public byte GetAccountPrivileges() => m_vAccountPrivileges;

        public bool Banned() => m_vAccountStatus > 99;

        public byte GetAccountStatus() => m_vAccountStatus;

        public Client GetClient() => m_vClient;

        public ComponentManager GetComponentManager() => GameObjectManager.GetComponentManager();

        [Obsolete]
        public ClientAvatar GetHomeOwnerAvatar() => m_vClientAvatar;

        public string GetIPAddress() => m_vIPAddress;

        public ClientAvatar GetPlayerAvatar() => m_vClientAvatar;

        public DateTime GetTime() => m_vTime;

        public bool HasFreeWorkers() => WorkerManager.GetFreeWorkers() > 0;

        public void LoadFromJson(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            GameObjectManager.Load(jsonObject);
        }

        public string SaveToJson() => JsonConvert.SerializeObject(GameObjectManager.Save());

        public void SetAccountPrivileges(byte privileges) => m_vAccountPrivileges = privileges;

        public void SetAccountStatus(byte status) => m_vAccountStatus = status;

        public void SetClient(Client client) => m_vClient = client;

        public void SetHome(string jsonHome)
        {
            var gameObjects = GameObjectManager.GetAllGameObjects();
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Clear();

            GameObjectManager.Load(JObject.Parse(jsonHome));
        }

        public void SetIPAddress(string IP) => m_vIPAddress = IP;

        public void SetTime(DateTime t) => m_vTime = t;

        public void Tick()
        {
            SetTime(DateTime.UtcNow);
            GameObjectManager.Tick();
        }
    }
}
