namespace Magic.Network.Messages.Client
{
    /// <summary>
    /// Message that is sent by the client to the server to request
    /// for a login.
    /// </summary>
    public class LoginRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequestMessage"/> class.
        /// </summary>
        public LoginRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// User ID needed to login in a specific account.
        /// </summary>
        public long UserId;
        /// <summary>
        /// User token needed to login in a specific account.
        /// </summary>
        public string UserToken;
        /// <summary>
        /// Client major version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int MajorVersion;
        /// <summary>
        /// Client minor version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int MinorVersion;
        /// <summary>
        /// Client content version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int BuildVersion;
        /// <summary>
        /// MasterHash of 'fingerprint.json'. If the fingerprint master hash is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public string MasterHash;
        /// <summary>
        /// UDID.
        /// </summary>
        public string Udid;
        /// <summary>
        /// Open UDID.
        /// </summary>
        public string OpenUdid;
        /// <summary>
        /// MAC address of the device.
        /// </summary>
        public string MacAddress;
        /// <summary>
        /// Model of the device.
        /// </summary>
        public string DeviceModel;
        /// <summary>
        /// Advertising GUID.
        /// </summary>
        public string AdvertisingGuid;
        /// <summary>
        /// Operating system version.
        /// </summary>
        public string OSVersion;
        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public bool IsAndroid;

        /// <summary>
        /// Unknown string 3.
        /// </summary>
        public string Unnkown1;

        /// <summary>
        /// Android device ID.
        /// </summary>
        public string AndroidDeviceId;
        /// <summary>
        /// Language of the device.
        /// </summary>
        public string DeviceLanguage;

        /// <summary>
        /// 
        /// </summary>
        public byte Unknown2;
        /// <summary>
        /// 
        /// </summary>
        public byte Language;
        /// <summary>
        /// Facebook distribution ID.
        /// </summary>
        public string FacebookDistributionID;
        /// <summary>
        /// Is advertising tracking enabled.
        /// </summary>
        public bool IsAdvertisingTrackingEnabled;
        /// <summary>
        /// 
        /// </summary>
        public string AppleVendorId;
        public int AppleStore;
        public string KunlunSSO;
        public string KunlunUid;
        public string Unknown3;
        public string Unknown4;
        public byte Unknown5;

        /// <summary>
        ///  Gets the ID of the <see cref="LoginRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 10101; } }

        /// <summary>
        /// Reads the <see cref="LoginRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();
            UserToken = reader.ReadString();
            MajorVersion = reader.ReadRRInt32();
            MinorVersion = reader.ReadRRInt32();
            BuildVersion = reader.ReadRRInt32();
            MasterHash = reader.ReadString();
            Udid = reader.ReadString();
            OpenUdid = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            AdvertisingGuid = reader.ReadString();
            OSVersion = reader.ReadString();
            IsAndroid = reader.ReadBoolean();

            Unnkown1 = reader.ReadString();

            AndroidDeviceId = reader.ReadString();
            DeviceLanguage = reader.ReadString();

            Unknown2 = reader.ReadByte();

            Language = reader.ReadByte();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBoolean();
            AppleVendorId = reader.ReadString();
            AppleStore = reader.ReadRRInt32();
            KunlunSSO = reader.ReadString();
            KunlunUid = reader.ReadString();

            Unknown3 = reader.ReadString();
            Unknown4 = reader.ReadString();
            Unknown5 = reader.ReadByte();
        }

        /// <summary>
        /// Writes the <see cref="LoginRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
        }
    }
}
