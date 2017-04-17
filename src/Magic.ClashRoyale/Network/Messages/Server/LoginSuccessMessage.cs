using System;

namespace Magic.Network.Messages.Server
{
    /// <summary>
    /// Message that is sent by the server to the client when
    /// <see cref="LoginRequestMessage"/> was successful.
    /// </summary>
    public class LoginSuccessMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginSuccessMessage"/> class.
        /// </summary>
        public LoginSuccessMessage()
        {
            // Space
        }

        /// <summary>
        /// User ID of the client.
        /// </summary>
        public long UserId;
        /// <summary>
        /// User ID of the client. Should be same as <see cref="UserId"/>.
        /// </summary>
        public long HomeId;
        /// <summary>
        /// User token of the client.
        /// </summary>
        public string UserToken;
        /// <summary>
        /// Facebook ID.
        /// </summary>
        public string FacebookId;
        /// <summary>
        /// Game center ID.
        /// </summary>
        public string GameCenterId;
        /// <summary>
        /// Major version.
        /// </summary>
        public int MajorVersion;
        /// <summary>
        /// Minor version.
        /// </summary>
        public int BuildVersion;
        /// <summary>
        /// Revision version.
        /// </summary>
        public int BuildVersion2;
        public int ContentVersion;
        /// <summary>
        /// Environment of the server.
        /// </summary>
        public string Environment; // Could implement an Enum here.
        /// <summary>
        /// Number of times logged in.
        /// </summary>
        public int LoginCount;
        /// <summary>
        /// Amount of time logged in.
        /// </summary>
        public TimeSpan PlayTime;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int DaysSinceStartedPlaying;

        /// <summary>
        /// Facebook application ID.
        /// </summary>
        public string FacebookAppId;
        /// <summary>
        /// Date last logged in.
        /// </summary>
        public DateTime DateLastSave;
        /// <summary>
        /// Date joined the server.
        /// </summary>
        public DateTime DateCreated;

        /// <summary>
        /// Google plus ID.
        /// </summary>
        public string GooglePlusId;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown1;

        public string Unknown2;
        public string Unknown3;

        public string AssetsUrl;
        /// <summary>
        /// Country code.
        /// </summary>
        public string CountryCode;
        /// <summary>
        /// Unknown string 3.
        /// </summary>
        public string EventAssetsUrl; // -1
        public byte Unknown4;


        /// <summary>
        ///  Gets the ID of the <see cref="LoginSuccessMessage"/>.
        /// </summary>
        public override ushort Id { get { return 20104; } }

        /// <summary>
        /// Reads the <see cref="LoginSuccessMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginSuccessMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();
            HomeId = reader.ReadInt64();
            UserToken = reader.ReadString();
            FacebookId = reader.ReadString();
            GameCenterId = reader.ReadString();
            MajorVersion = reader.ReadRRInt32();
            BuildVersion = reader.ReadRRInt32();
            BuildVersion2 = reader.ReadRRInt32();
            ContentVersion = reader.ReadRRInt32();
            Environment = reader.ReadString();
            LoginCount = reader.ReadRRInt32();
            PlayTime = TimeSpan.FromSeconds(reader.ReadRRInt32());

            DaysSinceStartedPlaying = reader.ReadRRInt32();

            FacebookAppId = reader.ReadString();
            DateLastSave = TimeUtils.FromJavaTimestamp(double.Parse(reader.ReadString()));
            DateCreated = TimeUtils.FromJavaTimestamp(double.Parse(reader.ReadString()));

            Unknown1 = reader.ReadRRInt32();

            GooglePlusId = reader.ReadString();

            Unknown2 = reader.ReadString();
            Unknown3 = reader.ReadString();

            CountryCode = reader.ReadString();
            AssetsUrl = reader.ReadString();
            EventAssetsUrl = reader.ReadString();

            Unknown4 = reader.ReadByte();
        }

        /// <summary>
        /// Writes the <see cref="LoginSuccessMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginSuccessMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(UserId);
            writer.Write(HomeId);
            writer.Write(UserToken);
            writer.Write(FacebookId);
            writer.Write(GameCenterId);
            writer.Write(MajorVersion, true);
            writer.Write(BuildVersion, true);
            writer.Write(BuildVersion2, true);
            writer.Write(ContentVersion, true);
            writer.Write(Environment);
            writer.Write(LoginCount, true);
            writer.Write((int)PlayTime.TotalSeconds, true);
            writer.Write(DaysSinceStartedPlaying, true);
            writer.Write(FacebookAppId);
            writer.Write(TimeUtils.ToJavaTimestamp(DateLastSave).ToString());
            writer.Write(TimeUtils.ToJavaTimestamp(DateCreated).ToString());

            writer.Write(Unknown1, true);

            writer.Write(GooglePlusId);

            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(CountryCode);
            writer.Write(AssetsUrl);
            writer.Write(EventAssetsUrl);

            writer.Write(Unknown4);
        }
    }
}
