using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{


    public class ByteArray
    {
        private MemoryStream m_Stream = new MemoryStream();
        private BinaryReader m_Reader = null;
        private BinaryWriter m_Writer = null;
        public bool IsBigEndian = true;

        public byte[] Bytes
        {
            get
            {
                return m_Stream.ToArray();
            }
        }
        public ByteArray()
        {
            Init();
        }

        public ByteArray(MemoryStream ms)
        {
            m_Stream = ms;
            Init();
        }

        public ByteArray(byte[] buffer)
        {
            m_Stream = new MemoryStream(buffer);
            Init();
        }

        private void Init()
        {
            m_Writer = new BinaryWriter(m_Stream);
            m_Reader = new BinaryReader(m_Stream);
        }

        public int Length
        {
            get { return (int)m_Stream.Length; }
        }

        public int Postion
        {
            get { return (int)m_Stream.Position; }
            set { m_Stream.Position = value; }
        }

        public byte[] Buffer
        {
            get { return m_Stream.GetBuffer(); }
        }

        internal MemoryStream MemoryStream { get { return m_Stream; } }

        public bool ReadBoolean()
        {
            try
            {
                return m_Reader.ReadBoolean();
            }
            catch
            {
                Debug.Log("ReadBoolean Error");
                return false;
            }
        }

        public byte ReadByte()
        {
            return m_Reader.ReadByte();
        }

        public void ReadBytes(byte[] bytes, uint offset, uint length)
        {
            byte[] tmp = m_Reader.ReadBytes((int)length);
            for (int i = 0; i < tmp.Length; i++)
                bytes[i + offset] = tmp[i];
           
        }

     
        public float ReadFloat()
        {
            byte[] bytes = m_Reader.ReadBytes(4);
            byte[] invertedBytes = new byte[4];
            //Grab the bytes in reverse order from the backwards index
            for (int i = 3, j = 0; i >= 0; i--, j++)
            {
                invertedBytes[j] = bytes[i];
            }
            float value = BitConverter.ToSingle(invertedBytes, 0);
            return value;

            // return m_Reader.ReadFloat();
        }

        public int ReadInt()
        {
            try
            {
                int v = m_Reader.ReadInt32();
                if (IsBigEndian)
                {
                    v = Endian.SwapInt32(v);
                }
                return v;
            }
            catch
            {
                Debug.LogWarning(" ReadInt Error , End of Stream .");
                return 0;
            }
        }
        public long ReadLong()
        {
            try
            {
                long v = m_Reader.ReadInt64();
                if (IsBigEndian)
                {
                    v = Endian.SwapInt64(v);
                }
                return v;
            }
            catch
            {
                Debug.LogWarning(" ReadLong Error , End of Stream .");
                return 0;
            }
        }
        public short ReadShort()
        {
            try
            {
                short v = m_Reader.ReadInt16();
                if (IsBigEndian)
                {
                    v = Endian.SwapInt16(v);
                }
                return v;
            }
            catch
            {
                Debug.LogWarning(" ReadShort Error , End of Stream .");
                return 0;
            }
        }

        public byte ReadUByte()
        {
            return m_Reader.ReadByte();
        }

        public uint ReadUInt()
        {
            uint v = m_Reader.ReadUInt32();
            if (IsBigEndian)
            {
                v = Endian.SwapUInt32(v);
            }
            return v;
        }

        public ushort ReadUShort()
        {
            ushort v = m_Reader.ReadUInt16();
            if (IsBigEndian)
            {
                v = Endian.SwapUInt16(v);
            }
            return v;
        }


        public string ReadStringNull()
        {
            byte num;
            string text = String.Empty;
            System.Collections.Generic.List<byte> temp = new System.Collections.Generic.List<byte>();

            while ((num = m_Reader.ReadByte()) != 0)
                temp.Add(num);

            text = Encoding.UTF8.GetString(temp.ToArray());

            return text;
        }

        public string ReadUTFBytes(uint length)
        {
            if (length == 0)
                return string.Empty;
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = m_Reader.ReadBytes((int)length);
            string decodedString = utf8.GetString(encodedBytes, 0, encodedBytes.Length);
            return decodedString;
        }

        public void WriteBoolean(bool value)
        {
            m_Writer.BaseStream.WriteByte(value ? ((byte)1) : ((byte)0));
        }

        public void WriteByte(byte value)
        {
            m_Writer.BaseStream.WriteByte(value);
        }

        public void WriteBytes(byte[] buffer)
        {
            for (int i = 0; buffer != null && i < buffer.Length; i++)
                m_Writer.BaseStream.WriteByte(buffer[i]);
        }

        public void WriteBytes(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
                m_Writer.BaseStream.WriteByte(bytes[i]);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteBigEndian(bytes);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteBigEndian(bytes);
        }

        private void WriteBigEndian(byte[] bytes)
        {
            if (bytes == null)
                return;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                m_Writer.BaseStream.WriteByte(bytes[i]);
            }
        }

        public void WriteInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteBigEndian(bytes);
        }

        public void WriteInt(int value)
        {
            WriteInt32(value);
        }

        public void WriteShort(int value)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)value);
            WriteBigEndian(bytes);
        }

        public void WriteUnsignedInt(uint value)
        {
            WriteInt32((int)value);
        }

        public void WriteLong(long value)
        {
            if (IsBigEndian)
            {
                value = Endian.SwapInt64(value);
            }
            byte[] bytes = BitConverter.GetBytes(value);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += bytes[i].ToString() + "  ";
                try
                {
                    m_Writer.BaseStream.WriteByte(bytes[i]);
                }
                catch
                {
                    Debug.Log("bytes[" + i + "]->" + str);
                }
            }
        }
        public void WriteUTF(string value)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            int byteCount = utf8Encoding.GetByteCount(value);
            byte[] buffer = utf8Encoding.GetBytes(value);
            WriteShort(byteCount);
            if (buffer.Length > 0)
                m_Writer.Write(buffer);
        }

        public void WriteUTFBytes(string value)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            byte[] buffer = utf8Encoding.GetBytes(value);
            if (buffer.Length > 0)
                m_Writer.Write(buffer);
        }
    }

    class Endian
    {
        public static short SwapInt16(short v)
        {
            return (short)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }
        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }
        public static int SwapInt32(int v)
        {
            return (int)(((SwapInt16((short)v) & 0xffff) << 0x10) |
                    (SwapInt16((short)(v >> 0x10)) & 0xffff));
        }
        public static uint SwapUInt32(uint v)
        {
            return (uint)(((SwapUInt16((ushort)v) & 0xffff) << 0x10) |
                    (SwapUInt16((ushort)(v >> 0x10)) & 0xffff));
        }
        public static long SwapInt64(long v)
        {
            return (long)(((SwapInt32((int)v) & 0xffffffffL) << 0x20) |
                    (SwapInt32((int)(v >> 0x20)) & 0xffffffffL));
        }
        public static ulong SwapUInt64(ulong v)
        {
            return (ulong)(((SwapUInt32((uint)v) & 0xffffffffL) << 0x20) |
                    (SwapUInt32((uint)(v >> 0x20)) & 0xffffffffL));
        }
    }
}
