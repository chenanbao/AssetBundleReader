using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectInfoTable
    {
        private Dictionary<int, ObjectInfo> infoMap = new Dictionary<int, ObjectInfo>();

        public void Read(ByteArray inData)
        {
           
            uint entries = inData.ReadUInt();
            //移动端没有该数据
            Debug.Log("ObjectInfoTable:"+entries);
            for (int i = 0; i < entries; i++)
            {
                int pathId = inData.ReadInt();
                Debug.Log("pathID:" + pathId);
                ObjectInfo info = new ObjectInfo();

                info.Read(inData);
                infoMap.Add(pathId, info);
            }
        }

    }
}
