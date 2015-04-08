using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MetadataInfo
    {
        private int attributes;
        private Dictionary<int, FieldTypeNode> typeMap = new Dictionary<int, FieldTypeNode>();

        public void Read(ByteArray inData, AssetHeader assetHeader)
        {
            if (assetHeader.GetVersion() >= 7)
            {
                inData.ReadStringNull();
                attributes = inData.ReadInt();
            }

            int numBaseClasses = inData.ReadInt();
            Debug.Log("numBaseClasses:" + numBaseClasses);
            for (int i = 0; i < numBaseClasses; i++)
            {
                int classID = inData.ReadInt();

                FieldTypeNode node = new FieldTypeNode();
                node.Read(inData);

                typeMap.Add(classID, node);
            }

            // padding
            if (assetHeader.GetVersion() >= 7)
            {
                inData.ReadInt();
            }
        }
    }
}
