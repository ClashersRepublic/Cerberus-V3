﻿using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System;

namespace CR.Assets.Editor.ScOld
{
    public class ScObject : IDisposable
    {
        #region Constructor
        public ScObject()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        
        public virtual ushort Id { get; protected set; }

        public virtual Bitmap Bitmap => null;

        public virtual List<ScObject> Children => new List<ScObject>();
        #endregion

        #region Methods
        public virtual int GetDataType()
        {
            return -1;
        }

        public virtual string GetDataTypeName()
        {
            return null;
        }

        public virtual string GetInfo()
        {
            return string.Empty;
        }

        public virtual string GetName()
        {
            return Id.ToString();
        }

        public virtual ScObject GetDataObject()
        {
            return null;
        }
        public virtual void Dispose()
        {
        }

        public virtual void Rename(string s)
        {
            
        }

        public virtual bool IsImage()
        {
            return false;
        }

        public virtual Bitmap Render(RenderingOptions options) => null;

        public virtual void Read(BinaryReader br , string id)
        {
            // Space
        }


        public virtual void Write(FileStream input)
        {
            // Space
        }
        
        #endregion
    }
}
