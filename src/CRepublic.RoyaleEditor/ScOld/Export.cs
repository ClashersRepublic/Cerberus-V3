﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace CR.Assets.Editor.ScOld
{
    public class Export : ScObject
    {
        #region Constructors
        public Export(ScFile scFile)
        {
            _scFile = scFile;
        }
        #endregion

        #region Fields & Properties
        private ushort _exportId;
        private string _exportName;
        private MovieClip _dataObject;
        private ScFile _scFile;

        public override ushort Id => _exportId;
        public override List<ScObject> Children => _dataObject.Children;
        #endregion

        #region Methods
        public override ScObject GetDataObject()
        {
            return _dataObject;
        }

        public override int GetDataType()
        {
            return 7;
        }

        public override string GetDataTypeName()
        {
            return "Exports";
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("/!\\ Experimental Rendering");
            sb.AppendLine("");
            sb.AppendLine("ExportId: " + _exportId);
            sb.AppendLine("Polygons: " + Children.Count);   
            return sb.ToString();
        }

        public override string GetName()
        {
            return _exportName;
        }

        public override void Rename(string name) => _exportName = name;

        public override void Write(FileStream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            byte[] file = new byte[input.Length];
            input.Read(file, 0, file.Length);

            int cursor = (int)_scFile.GetStartExportsOffset();

            input.Seek(_scFile.GetStartExportsOffset(), SeekOrigin.Begin);

            ushort exportCount = BitConverter.ToUInt16(file, cursor);
            input.Write(BitConverter.GetBytes((ushort)(exportCount + 1)), 0, 2);
            cursor += 2;

            input.Seek(exportCount * 2, SeekOrigin.Current);
            cursor += exportCount * 2;
            input.Write(BitConverter.GetBytes(_exportId), 0, 2);

            for (int i = 0; i < exportCount; i++)
            {
                byte nameLength = file[cursor];
                cursor++;
                byte[] exportName = new byte[nameLength];
                Array.Copy(file, cursor, exportName, 0, nameLength);
                input.WriteByte(nameLength);
                input.Write(exportName, 0, nameLength);
                cursor += nameLength;
            }

            input.WriteByte((byte)_exportName.Length);
            input.Write(Encoding.UTF8.GetBytes(_exportName), 0, (byte)_exportName.Length);

            while (cursor < file.Length)
            {
                input.WriteByte(file[cursor]);
                cursor++;
            }

            //refresh all offsets
            foreach (Texture t in _scFile.GetTextures())
            {
                long offset = t.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                t.SetOffset(offset);
            }
            foreach (Shape s in _scFile.GetShapes())
            {
                long offset = s.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                s.SetOffset(offset);
                foreach (ShapeChunk sc in s.GetChunks())
                {
                    long chunkOffset = sc.GetOffset();
                    if (chunkOffset > 0)
                        chunkOffset += 2 + 1 + _exportName.Length;
                    else
                        chunkOffset = chunkOffset - 2 - 1 - _exportName.Length;
                    sc.SetOffset(chunkOffset);
                }
            }
            foreach (MovieClip mc in _scFile.GetMovieClips())
            {
                long offset = mc.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                mc.SetOffset(offset);
            }
            _scFile.SetEofOffset(_scFile.GetEofOffset() + 2 + 1 + _exportName.Length);
            //ne pas oublier eofoffset
        }

        public void SetDataObject(MovieClip sd)
        {
            _dataObject = sd;
        }

        public void SetId(ushort id)
        {
            _exportId = id;
        }

        public void SetExportName(string name)
        {
            _exportName = name;
        }
        public override Bitmap Render(RenderingOptions options)
        {
            if (Children != null && Children.Count > 0)
            {
                List<PointF> A = new List<PointF>();
                foreach (Shape s in Children)
                {
                    PointF[] pointsXY = s.Children.SelectMany(chunk => ((ShapeChunk)chunk).XY).ToArray();
                    A.AddRange(pointsXY.ToArray());
                }
                foreach (PointF p in  A)
                {
                    Console.WriteLine("x: " + p.X + ", y: " + p.Y);
                }

                using (var xyPath = new GraphicsPath())
                {
                    xyPath.AddPolygon(A.ToArray());

                    var xyBound = Rectangle.Round(xyPath.GetBounds());

                    var width = xyBound.Width;
                    width = width > 0 ? width : 1;

                    var height = xyBound.Height;
                    height = height > 0 ? height : 1;

                    var x = xyBound.X;
                    var y = xyBound.Y;

                    var finalShape = new Bitmap(width, height);
                    Console.WriteLine($"Rendering export: W:{finalShape.Width} H:{finalShape.Height}\n");

                    foreach (Shape shape in Children)
                    {
                        foreach (ShapeChunk chunk in shape.Children)
                        {
                            var texture = (Texture)_scFile.GetTextures()[chunk.GetTextureId()];
                            if (texture != null)
                            {
                                Bitmap bitmap = texture.Bitmap;
                                using (var gpuv = new GraphicsPath())
                                {
                                    gpuv.AddPolygon(chunk.UV.ToArray());

                                    var gxyBound = Rectangle.Round(gpuv.GetBounds());

                                    int gpuvWidth = gxyBound.Width;
                                    gpuvWidth = gpuvWidth > 0 ? gpuvWidth : 1;

                                    int gpuvHeight = gxyBound.Height;
                                    gpuvHeight = gpuvHeight > 0 ? gpuvHeight : 1;

                                    var shapeChunk = new Bitmap(gpuvWidth, gpuvHeight);

                                    var chunkX = gxyBound.X;
                                    var chunkY = gxyBound.Y;

                                    using (var g = Graphics.FromImage(shapeChunk))
                                    {
                                        gpuv.Transform(new Matrix(1, 0, 0, 1, -chunkX, -chunkY));
                                        g.SetClip(gpuv);
                                        g.DrawImage(bitmap, -chunkX, -chunkY);
                                    }

                                    GraphicsPath gp = new GraphicsPath();
                                    gp.AddPolygon(new[] { new Point(0, 0), new Point(gpuvWidth, 0), new Point(0, gpuvHeight) });

                                    //Calculate transformation Matrix of UV
                                    //double[,] matrixArrayUV = { { polygonUV[0].X, polygonUV[1].X, polygonUV[2].X }, { polygonUV[0].Y, polygonUV[1].Y, polygonUV[2].Y }, { 1, 1, 1 } };
                                    double[,] matrixArrayUV =
                                    {
                            {
                                gpuv.PathPoints[0].X, gpuv.PathPoints[1].X, gpuv.PathPoints[2].X
                            },
                            {
                                gpuv.PathPoints[0].Y, gpuv.PathPoints[1].Y, gpuv.PathPoints[2].Y
                            },
                            {
                                1, 1, 1
                            }
                        };
                                    double[,] matrixArrayXY =
                                    {
                            {
                                chunk.XY[0].X, chunk.XY[1].X, chunk.XY[2].X
                            },
                            {
                                chunk.XY[0].Y, chunk.XY[1].Y, chunk.XY[2].Y
                            },
                            {
                                1, 1, 1
                            }
                        };

                                    var matrixUV = Matrix<double>.Build.DenseOfArray(matrixArrayUV);
                                    var matrixXY = Matrix<double>.Build.DenseOfArray(matrixArrayXY);
                                    var inverseMatrixUV = matrixUV.Inverse();
                                    var transformMatrix = matrixXY * inverseMatrixUV;
                                    var m = new Matrix((float)transformMatrix[0, 0], (float)transformMatrix[1, 0], (float)transformMatrix[0, 1], (float)transformMatrix[1, 1], (float)transformMatrix[0, 2], (float)transformMatrix[1, 2]);
                                    //m = new Matrix((float)transformMatrix[0, 0], (float)transformMatrix[1, 0], (float)transformMatrix[0, 1], (float)transformMatrix[1, 1], (float)Math.Round(transformMatrix[0, 2]), (float)Math.Round(transformMatrix[1, 2]));

                                    //Perform transformations
                                    gp.Transform(m);

                                    using (Graphics g = Graphics.FromImage(finalShape))
                                    {
                                        //g.InterpolationMode = InterpolationMode.NearestNeighbor;
                                        //g.PixelOffsetMode = PixelOffsetMode.None;

                                        //Set origin
                                        Matrix originTransform = new Matrix();
                                        originTransform.Translate(-x, -y);
                                        g.Transform = originTransform;

                                        g.DrawImage(shapeChunk, gp.PathPoints, gpuv.GetBounds(), GraphicsUnit.Pixel);

                                        if (options.ViewPolygons)
                                        {
                                            gpuv.Transform(m);
                                            g.DrawPath(new Pen(Color.DeepSkyBlue, 1), gpuv);
                                        }
                                        g.Flush();
                                    }
                                }
                            }
                        }
                    }

                    return finalShape;
                }
            }
            return null;
        }
    }
    #endregion
}