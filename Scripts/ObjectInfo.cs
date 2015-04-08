using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectInfo
    {
        // Object data offset
        private long offset;

        // Object data size
        private long length;

        // Type ID, equal to classID if it's not a MonoBehaviour
        private int typeID;

        // Class ID, probably something else in asset format <=5
        private int classID;

        // set to 1 if destroyed object instances are stored?
        private short isDestroyed;


        public void Read(ByteArray inData)
        {
            offset = inData.ReadUInt();
            length = inData.ReadUInt();
            typeID = inData.ReadInt();
            classID = inData.ReadShort();
            isDestroyed = inData.ReadShort();

            Debug.Log(string.Format("offset={0},length={1},typeID={2},classID={3},isDestroyed={4}", offset, length, typeID, classID, isDestroyed));
        }
    }
}
