﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRepublic.LZMAManager
{
    internal class Program
    {

        internal static void Main(string[] args)
        {
            Console.SetOut(new Prefixed());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"_________ .__                .__                          __________                   ___.   .__  .__        ");
            Console.WriteLine(@"\_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  ");
            Console.WriteLine(@"/    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ ");
            Console.WriteLine(@"\     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ ");
            Console.WriteLine(@" \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >");
            Console.WriteLine(@"        \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ ");
            Console.WriteLine();
            Console.ResetColor();

            //args[0]: source directory
            //args[1]: target directory
            //args[2]: filename (ex: *.csv)

            //args[3] file type : -sc to use sc file header
            //args[4] (optional) : "-d" for decompress

            if (args.Length == 4)
            {
                if (args[3] == "-d")
                    if (args[2] == "*.sc")
                        Decompress(new[] { args[0], args[1], args[2] }, true);
                    else
                        Decompress(new[] { args[0], args[1], args[2] });
            }
            else
            {
                if (args[2] == "*.sc")
                    Compress(new string[] { args[0], args[1], args[2] }, true);
                else
                    Compress(new string[] { args[0], args[1], args[2] });
            }
        }

        internal static void Compress(string[] args, bool header = false)
        {
            var encoder = new SevenZip.Compression.LZMA.Encoder();
            var filePaths = Directory.GetFiles(args[0], args[2]);
            Directory.CreateDirectory(args[1]);

            foreach (string filePath in filePaths)
            {
                byte[] hash;
                using (MD5 md5 = MD5.Create())
                {
                    hash = md5.ComputeHash(File.ReadAllBytes(filePath));
                }

                using (FileStream input = new FileStream(filePath, FileMode.Open))
                {
                    using (
                        FileStream output = new FileStream(Path.Combine(args[1], Path.GetFileName(filePath)),
                            FileMode.Create, FileAccess.Write))
                    {
                        SevenZip.CoderPropID[] propIDs =
                        {
                            SevenZip.CoderPropID.DictionarySize,
                            SevenZip.CoderPropID.PosStateBits,
                            SevenZip.CoderPropID.LitContextBits,
                            SevenZip.CoderPropID.LitPosBits,
                            SevenZip.CoderPropID.Algorithm,
                            SevenZip.CoderPropID.NumFastBytes,
                            SevenZip.CoderPropID.MatchFinder,
                            SevenZip.CoderPropID.EndMarker
                        };

                        Int32 dictionary = 1 << 24;
                        Int32 posStateBits = 2;
                        Int32 litContextBits = 3; // for normal files
                        // UInt32 litContextBits = 0; // for 32-bit data
                        Int32 litPosBits = 0;
                        // UInt32 litPosBits = 2; // for 32-bit data
                        Int32 algorithm = 2;
                        Int32 numFastBytes = 32;
                        string mf = "bt4";
                        bool eos = false;

                        if (header)
                        {
                            litContextBits = 4; // for normal files
                            output.Write(Encoding.UTF8.GetBytes("SC"), 0, 2);
                            output.Write(BitConverter.GetBytes(1), 0, 4);
                            output.Write(BitConverter.GetBytes(hash.Length), 0, 4);
                            output.Write(hash, 0, 16);
                        }
                        object[] properties =
                        {
                            dictionary,
                            posStateBits,
                            litContextBits,
                            litPosBits,
                            algorithm,
                            numFastBytes,
                            mf,
                            eos
                        };

                        encoder.SetCoderProperties(propIDs, properties);
                        encoder.WriteCoderProperties(output);
                        output.Write(BitConverter.GetBytes(input.Length), 0, 4);

                        encoder.Code(input, output, input.Length, -1, null);
                        output.Flush();
                        output.Dispose();
                    }
                    input.Dispose();
                }
            }
        }

        internal static void Decompress(string[] args, bool header = false)
        {
            var decoder = new SevenZip.Compression.LZMA.Decoder();
            var filePaths = Directory.GetFiles(args[0], args[2]);
            Directory.CreateDirectory(args[1]);
            foreach (string filePath in filePaths)
            {
                using (var input = new FileStream(filePath, FileMode.Open))
                {
                    using (
                        var output = new FileStream(Path.Combine(args[1], Path.GetFileName(filePath)),
                            FileMode.Create))
                    {
                        if (header)
                        {
                            byte[] sc = new byte[2];
                            input.Read(sc, 0, 2);

                            byte[] version = new byte[4];
                            input.Read(version, 0, 4);

                            byte[] md5Length = new byte[4];
                            input.Read(md5Length, 0, 4);

                            byte[] md5 = new byte[16];
                            input.Read(md5, 0, 16);
                        }

                        // Read the decoder properties
                        byte[] properties = new byte[5];
                        input.Read(properties, 0, 5);

                        // Read in the decompress file size.
                        byte[] fileLengthBytes = new byte[4];
                        input.Read(fileLengthBytes, 0, 4);
                        int fileLength = BitConverter.ToInt32(fileLengthBytes, 0);

                        decoder.SetDecoderProperties(properties);
                        decoder.Code(input, output, input.Length, fileLength, null);
                        output.Flush();
                        output.Dispose();
                    }
                    input.Dispose();
                }
            }
        }
    }
}