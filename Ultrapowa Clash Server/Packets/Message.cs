using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UCS.Core;
using UCS.Core.Crypto;
using UCS.Core.Crypto.Blake2b;
using UCS.Core.Crypto.CustomNaCl;
using UCS.Core.Settings;
using UCS.Helpers;
using UCS.Logic;
using UCS.Packets.Messages.Server;
using static System.Console;
using static UCS.PacketProcessing.Client;

namespace UCS.PacketProcessing
{
    internal class Message
    {

        byte[] m_vData;
        int m_vLength;
        ushort m_vMessageVersion;
        ushort m_vType;

        public Message()
        {
        }

        public Message(Client c)
        {
            Client = c;
            m_vType = 0;
            m_vLength = -1;
            m_vMessageVersion = 0;
            m_vData = null;
        }

        public Message(Client c, PacketReader br)
        {
            Client = c;
            //m_vType = br.ReadUInt16WithEndian();
            //byte[] tempLength = br.ReadBytes(3);
            //m_vLength = (0x00 << 24) | (tempLength[0] << 16) | (tempLength[1] << 8) | tempLength[2];
            //m_vMessageVersion = br.ReadUInt16WithEndian();
            //m_vData = br.ReadBytes(m_vLength);
        }

        private static readonly object s_sync = new object();

        public int Broadcasting { get; set; }

        public Client Client { get; set; }

        public virtual void Decode()
        {

        }

        public void Decrypt()
        {
            try
            {
                if (Constants.IsRc4)
                {
                    lock (s_sync)
                    {
                        Client.Decrypt(m_vData);
                        if (m_vType == 10101)
                            Client.State = ClientState.Login;

                        // No need since the decryption occurs on same buffer.
                        //SetData(m_vData);
                    }
                }
                else
                {
                    if (m_vType == 10101)
                    {
                        byte[] cipherText = m_vData;
                        Client.CPublicKey = cipherText.Take(32).ToArray();
                        Hasher b = Blake2B.Create(new Blake2BConfig
                        {
                            OutputSizeInBytes = 24
                        });
                        b.Init();
                        b.Update(Client.CPublicKey);
                        b.Update(Key.Crypto.PublicKey);
                        Client.CRNonce = b.Finish();
                        cipherText = CustomNaCl.OpenPublicBox(cipherText.Skip(32).ToArray(), Client.CRNonce, Key.Crypto.PrivateKey, Client.CPublicKey);
                        Client.CSharedKey = Client.CPublicKey;
                        Client.CSessionKey = cipherText.Take(24).ToArray();
                        Client.CSNonce = cipherText.Skip(24).Take(24).ToArray();
                        Client.State = ClientState.Login;
                        SetData(cipherText.Skip(48).ToArray());
                    }
                    else
                    {
                        if (m_vType != 10100)
                            if (Client.State == ClientState.LoginSuccess)
                            {
                                Client.CSNonce.Increment();
                                SetData(CustomNaCl.OpenSecretBox(new byte[16].Concat(m_vData).ToArray(), Client.CSNonce, Client.CSharedKey));
                            }
                    }
                }
            }
            catch
            {
                Client.State = ClientState.Exception;
            }
        }

        public virtual void Encode()
        {

        }

        public void Encrypt(byte[] plainText)
        {
            try
            {
                if (Constants.IsRc4)
                {
                    Client.Encrypt(plainText);
                    if (m_vType == 20104)
                        Client.State = Client.ClientState.LoginSuccess;

                    SetData(plainText);
                }
                else
                {
                    if (m_vType == 20104 || m_vType == 20103)
                    {
                        Hasher b = Blake2B.Create(new Blake2BConfig
                        {
                            OutputSizeInBytes = 24
                        });
                        b.Init();
                        b.Update(Client.CSNonce);
                        b.Update(Client.CPublicKey);
                        b.Update(Key.Crypto.PublicKey);
                        SetData(CustomNaCl.CreatePublicBox(Client.CRNonce.Concat(Client.CSharedKey).Concat(plainText).ToArray(), b.Finish(), Key.Crypto.PrivateKey, Client.CPublicKey));
                        if (m_vType == 20104)
                            Client.State = Client.ClientState.LoginSuccess;
                    }
                    else
                    {
                        Client.CRNonce.Increment();
                        SetData(CustomNaCl.CreateSecretBox(plainText, Client.CRNonce, Client.CSharedKey).Skip(16).ToArray());
                    }
                }
            }
            catch (Exception)
            {
                Client.State = ClientState.Exception;
            }
        }

        public byte[] GetData() => m_vData;

        public int GetLength() => m_vLength;

        public ushort GetMessageType() => m_vType;

        public ushort GetMessageVersion() => m_vMessageVersion;

        public byte[] GetRawData()
        {
            var encodedMessage = new List<byte>();
            encodedMessage.AddUInt16(m_vType);
            encodedMessage.AddInt32WithSkip(m_vLength, 1);
            encodedMessage.AddUInt16(m_vMessageVersion);
            encodedMessage.AddRange(m_vData);
            return encodedMessage.ToArray();
        }

        public virtual void Process(Level level)
        {

        }

        public void SetData(byte[] data)
        {
            m_vData = data;
            m_vLength = data.Length;
        }

        public void SetMessageType(ushort type)
        {
            m_vType = type;
            Logger.Write("Server Message " + type + " was sent");
        }

        public void SetMessageVersion(ushort v)
        {
            m_vMessageVersion = v;
        }

        public string ToHexString()
        {
            var hex = BitConverter.ToString(m_vData);
            return hex.Replace("-", " ");
        }

        //public override string ToString() => Encoding.UTF8.GetString(m_vData, 0, m_vLength);
    }
}
