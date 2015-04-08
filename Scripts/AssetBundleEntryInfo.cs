using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AssetBundleEntryInfo
    {
        private String name;
        private long offset;
        private long size;

        public void Read(ByteArray inData)
        {
            name = inData.ReadStringNull();
            offset = inData.ReadUInt();
            size = inData.ReadUInt();
            Debug.Log(string.Format("name={0},offset={1},size={2}",name,offset,size));
            //36+4+4=44
        }

        public String GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public long GetOffset()
        {
            return offset;
        }

        public void SetOffset(long offset)
        {
            this.offset = offset;
        }

        public long GetSize()
        {
            return size;
        }

        public void SetSize(long size)
        {
            this.size = size;
        }
    }
}
