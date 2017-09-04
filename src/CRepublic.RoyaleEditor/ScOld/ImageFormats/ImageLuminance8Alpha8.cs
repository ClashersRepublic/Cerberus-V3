﻿using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using CR.Assets.Editor.Helpers;

namespace CR.Assets.Editor.ScOld.ImageFormats
{
    internal class ImageLuminance8Alpha8 : ScImage
    {
        public ImageLuminance8Alpha8()
        {
            
        }
        public override string GetImageTypeName()
        {
            return "Luminance8 Alpha8";
        }
        public override void ReadImage(uint packetID, uint packetSize, BinaryReader br)
        {
            base.ReadImage(packetID, packetSize, br);
            var sw = Stopwatch.StartNew();
            bool Is32x32 = false; // true;
            switch (packetID)
            {
                case 27: //1b
                case 28: //1c
                case 29: //1d (boom beach: in-game_tex.sc & defences_tex.sc)
                    Is32x32 = true;
                    break;
            }

            Console.WriteLine(@"packetID: " + packetID);
            Console.WriteLine(@"packetSize: " + packetSize);
            Console.WriteLine(@"texPixelFormat: " + _imageType);
            Console.WriteLine(@"texWidth: " + _width);
            Console.WriteLine(@"texHeight: " + _height);
            Console.WriteLine(@"Is32x32: " + Is32x32);

            _bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);


            Color[,] pixelArray = new Color[_height, _width];
            for (int row = 0; row < pixelArray.GetLength(0); row++)
            {
                for (int col = 0; col < pixelArray.GetLength(1); col++)
                {
                    ushort color = br.ReadUInt16();
                    int alpha = (int)(color & 0xFF);
                    int rgb = (int)(color >> 8);//>>8 : Shift 8 bits to the right
                    int red = rgb;
                    int green = rgb;
                    int blue = rgb;
                    pixelArray[row, col] = Color.FromArgb(alpha, red, green, blue);
                }

            }
            if (Is32x32)
                pixelArray = Utils.Solve32X32Blocks(_width, _height, pixelArray);

            for (int row = 0; row < pixelArray.GetLength(0); row++)
            {
                for (int col = 0; col < pixelArray.GetLength(1); col++)
                {
                    //Color pxColor = Color.Red;
                    Color pxColor = pixelArray[row, col];
                    _bitmap.SetPixel(col, row, pxColor);
                }
            }

            sw.Stop();
            Console.WriteLine("ImageLuminance8Alpha8.ReadImage finished in {0}ms", sw.Elapsed.TotalMilliseconds);
        }

        public override void Print()
        {
            base.Print();
        }

        public override void WriteImage(FileStream input)
        {
            base.WriteImage(input);

            for (int column = 0; column < _bitmap.Height; column++)
            {
                for (int row = 0; row < _bitmap.Width; row++)
                {
                    Color cc = _bitmap.GetPixel(row, column);
                    var a = cc.A;
                    var r = cc.R;
                    if (a == 0) 
                        r = 0;

                    input.WriteByte(a);
                    input.WriteByte(r);
                }
            }
        }
    }
}
