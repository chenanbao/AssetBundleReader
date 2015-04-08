using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class FileIdentifier
    {
        private String assetPath;

        // Globally unique identifier of the referred asset. Unity displays these
        // as simple 16 byte hex strings with each byte swapped, but they can also
        // be represented according to the UUID standard.
        private long guid_high;
        private long guid_low;

        // Path to the asset file. Only used if "type" is 0.
        private String filePath;

        // Reference type. Possible values are probably 0 to 3.
        private int type;

        public void Read(ByteArray inData, AssetHeader assetHeader)
        {
            if (assetHeader.GetVersion() > 5)
            {
                assetPath = inData.ReadStringNull();
            }
            guid_high = inData.ReadLong();
            guid_low = inData.ReadLong();
            type = inData.ReadInt();
            filePath = inData.ReadStringNull();

            Debug.Log(string.Format("assetPath={0},filePath={1},type={3}",assetPath,filePath,type));
        }
    }
}
