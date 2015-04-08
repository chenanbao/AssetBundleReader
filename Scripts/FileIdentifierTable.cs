using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class FileIdentifierTable
    {
        private  List<FileIdentifier> fileIDs = new List<FileIdentifier>();

        public void Read(ByteArray inData, AssetHeader assetHeader)
        {
            uint entries = inData.ReadUInt();
            Debug.Log("FileIdentifierTable:" + entries);
            for (int i = 0; i < entries; i++)
            {
                FileIdentifier fileRef = new FileIdentifier();
                fileRef.Read(inData, assetHeader);
                fileIDs.Add(fileRef);
            }
        }
    }
}
