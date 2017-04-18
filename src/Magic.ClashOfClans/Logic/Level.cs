using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Magic.Logic.Manager;
using Magic.PacketProcessing;

namespace Magic.Logic
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
        private readonly ClientAvatar m_vAvatar;

        public Level()
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);
            m_vAvatar = new ClientAvatar();
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
            m_vIPAddress = "0.0.0.0";
        }

        public Level(long id, string token)
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);

            m_vAvatar = new ClientAvatar(id, token);
            m_vTime = DateTime.UtcNow;
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
            m_vIPAddress = "0.0.0.0";
        }


        public byte AccountPrivileges
        {
            get
            {
                return m_vAccountPrivileges;
            }

            set
            {
                m_vAccountPrivileges = value;
            }
        }

        public bool Banned => m_vAccountStatus > 99;


        public byte AccountStatus
        {
            get
            {
                return m_vAccountStatus;
            }

            set
            {
                m_vAccountStatus = value;
            }
        }


        public Client Client
        {
            get
            {
                return m_vClient;
            }

            set
            {
                m_vClient = value;
            }
        }

        public ComponentManager GetComponentManager() => GameObjectManager.GetComponentManager();

        [Obsolete]
        public ClientAvatar GetHomeOwnerAvatar() => m_vAvatar;


        public string IPAddress
        {
            get
            {
                return m_vIPAddress;
            }

            set
            {
                m_vIPAddress = value;
            }
        }

        public ClientAvatar Avatar => m_vAvatar;


        public DateTime Time
        {
            get
            {
                return m_vTime;
            }

            set
            {
                m_vTime = value;
            }
        }

        public bool HasFreeWorkers() => WorkerManager.GetFreeWorkers() > 0;

        public void LoadFromJson(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            GameObjectManager.Load(jsonObject);
        }

        public string SaveToJson()
        {
            return JsonConvert.SerializeObject(GameObjectManager.Save());
        }

        public void SetHome(string jsonHome)
        {
            var gameObjects = GameObjectManager.GetAllGameObjects();
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Clear();

            GameObjectManager.Load(JObject.Parse(jsonHome));
        }

        public void Tick()
        {
            Time = DateTime.UtcNow;
            GameObjectManager.Tick();
        }
    }
}
