using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AssetBundleReader
    {
        private AssetBundleHeader header = new AssetBundleHeader();
        private  List<AssetBundleEntryInfo> entryInfos = new List<AssetBundleEntryInfo>();
        private ByteArray inData;

        public AssetBundleReader(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            BufferedStream bfs = new BufferedStream(fs);
            byte[] bf = new byte[(int)fs.Length];
            fs.Read(bf, 0, (int)fs.Length);
            inData = new ByteArray(bf);
            
            header.Read(inData);
            
            if (!header.HasValidSignature())
            {
                Debug.LogError("Invalid signature");
                return;
            }

            if (header.IsCompressed())
            {

                SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();

                uint length = (uint) (fs.Length - header.GetHeaderSize());
                byte[] inBytes = new byte[length];
                inData.ReadBytes(inBytes, 0, length);
                MemoryStream input = new MemoryStream(inBytes);
                MemoryStream output = new MemoryStream();

                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);
                output.Position = 0;

                inData = new ByteArray(output);

                Debug.Log("fileLength:" + fileLength);
            }
            
            uint files = inData.ReadUInt();
            Debug.Log(files);
            for (int i = 0; i < files; i++)
            {
                AssetBundleEntryInfo entryInfo = new AssetBundleEntryInfo();
                entryInfo.Read(inData);
                entryInfos.Add(entryInfo);
                Debug.Log("AssetBundleEntryInfo");

                inData.Postion = (int)entryInfo.GetOffset();
                AssetHeader assetHeader = new AssetHeader();
                assetHeader.Read(inData);
                Debug.Log("AssetHeader");

                MetadataInfo metadataInfo = new MetadataInfo();
                metadataInfo.Read(inData, assetHeader);
                Debug.Log("MetadataInfo");

                //后面的都是小端读取
                inData.IsBigEndian = false;

                ObjectInfoTable objectInfoTable = new ObjectInfoTable();
                objectInfoTable.Read(inData);
                Debug.Log("ObjectInfoTable");

                FileIdentifierTable fileIdentifierTable = new FileIdentifierTable();
                fileIdentifierTable.Read(inData, assetHeader);
                Debug.Log("FileIdentifierTable");
            }

            Debug.Log(inData.Postion);
        }
    }

}
