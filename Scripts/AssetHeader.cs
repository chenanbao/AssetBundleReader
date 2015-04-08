using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AssetHeader
    {
        // size of the structure data
        private long metadataSize;

        // size of the whole asset file
        private long fileSize;

        private long assetVersion;//assetVersion

        // offset to the serialized data
        private long dataOffset;

        // byte order of the serialized data?
        private byte endianness;

        // unused
        private byte[] reserved = new byte[3];

        public void Read(ByteArray inData)
        {
            metadataSize = inData.ReadInt();
            fileSize = inData.ReadUInt();
            assetVersion = inData.ReadInt();
            dataOffset = inData.ReadUInt();
            if (assetVersion >= 9)
            {
                endianness = inData.ReadByte();
                inData.ReadBytes(reserved, 0, 3);
            }
            Debug.Log(string.Format("metadataSize={0},fileSize={1},versionInfo={2},dataOffset={3}", metadataSize, fileSize, assetVersion, dataOffset));
            //4*5=20
        }

        public long GetMetadataSize()
        {
            return metadataSize;
        }

        public void SetMetadataSize(long metadataSize)
        {
            this.metadataSize = metadataSize;
        }

        public long GetFileSize()
        {
            return fileSize;
        }

        public void SetFileSize(long fileSize)
        {
            this.fileSize = fileSize;
        }

        public int GetVersion()
        {
            return (int)assetVersion;
        }

        public long GetDataOffset()
        {
            return dataOffset;
        }

        public void SetDataOffset(long dataOffset)
        {
            this.dataOffset = dataOffset;
        }

        public byte GetEndianness()
        {
            return endianness;
        }

        public void SetEndianness(byte endianness)
        {
            this.endianness = endianness;
        }
    }
}
