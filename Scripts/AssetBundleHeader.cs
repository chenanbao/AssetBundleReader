using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public struct LevelInfo
    {
        public uint PackSize;
        public uint UncompressedSize;
    }

    public class AssetBundleHeader
    {
        public static String SIGNATURE_WEB = "UnityWeb";
        public static String SIGNATURE_RAW = "UnityRaw";

        // UnityWeb or UnityRaw
        private String signature;

        // file version
        // 3 in Unity 3.5 and 4
        // 2 in Unity 2.6 to 3.4
        // 1 in Unity 1 to 2.5
        private int streamVersion;

        // player version string
        // 2.x.x for Unity 2
        // 3.x.x for Unity 3/4
        private String unityVersion;

        // engine version string
        private String unityRevision;

        // minimum number of bytes to read for streamed bundles,
        // equal to completeFileSize for normal bundles
        private long minimumStreamedBytes;

        // offset to the bundle data or size of the bundle header
        private uint headerSize;

        // equal to 1 if it's a streamed bundle, number of levelX + mainData assets
        // otherwise
        private int numberOfLevelsToDownload;

        private List<LevelInfo> levelByteEnd = new List<LevelInfo>();

        // equal to file size, sometimes equal to uncompressed data size without the header
        private long completeFileSize;

        // offset to the first asset file within the data area? equals compressed
        // file size if completeFileSize contains the uncompressed data size
        private long dataHeaderSize;

        public void Read(ByteArray bs)
        {
            signature = bs.ReadStringNull();
            streamVersion = bs.ReadInt();
            unityVersion = bs.ReadStringNull();
            unityRevision = bs.ReadStringNull();
            minimumStreamedBytes = bs.ReadInt();
            headerSize = bs.ReadUInt();

            numberOfLevelsToDownload = bs.ReadInt();
            int numberOfLevels = bs.ReadInt();

            for (int i = 0; i < numberOfLevels; i++)
            {
                levelByteEnd.Add(new LevelInfo() { PackSize = bs.ReadUInt(), UncompressedSize = bs.ReadUInt() });
            }

            if (streamVersion >= 2)
            {
                completeFileSize = bs.ReadUInt();
            }

            if (streamVersion >= 3)
            {
                dataHeaderSize = bs.ReadUInt();
            }

            bs.ReadByte();
        }

        public Boolean HasValidSignature()
        {
            return signature.Equals(SIGNATURE_WEB) || signature.Equals(SIGNATURE_RAW);
        }

        public void SetCompressed(Boolean compressed)
        {
            signature = compressed ? SIGNATURE_WEB : SIGNATURE_RAW;
        }

        public Boolean IsCompressed()
        {
            return signature.Equals(SIGNATURE_WEB);
        }

        public uint GetHeaderSize()
        {
            return headerSize;
        }
    }
}
