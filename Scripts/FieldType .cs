using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class FieldType
    {
        public static int FLAG_FORCE_ALIGN = 0x4000;

        // field type string
        private String type;

        // field name string
        private String name;

        // size of the field value in bytes or -1 if the field contains sub-fields only
        private int size;

        // field index for the associated parent field
        private int index;

        // set to 1 if "type" is "Array" or "TypelessData"
        private int isArray;

        // type version, starts with 1 and is incremented when the type
        // information is updated in a new Unity release
        //
        // equal to serializedVersion in YAML format files
        private int version;

        // field flags
        // observed values:
        // 0x1
        // 0x10
        // 0x800
        // 0x4000
        // 0x8000
        // 0x200000
        // 0x400000
        private int metaFlag;

        public Boolean IsForceAlign()
        {
            return (metaFlag & FLAG_FORCE_ALIGN) != 0;
        }

        public void SetForceAlign(Boolean forceAlign)
        {
            if (forceAlign)
            {
                metaFlag |= FLAG_FORCE_ALIGN;
            }
            else
            {
                metaFlag &= ~FLAG_FORCE_ALIGN;
            }
        }

        public String GetTypeName()
        {
            return type;
        }

        public void SetTypeName(String type)
        {
            this.type = type;
        }

        public String GetFieldName()
        {
            return name;
        }

        public void SetFieldName(String name)
        {
            this.name = name;
        }

        public int getSize()
        {
            return size;
        }

        public void SetSize(int size)
        {
            this.size = size;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public Boolean GetIsArray()
        {
            return isArray == 1;
        }

        public void SetIsArray(Boolean isArray)
        {
            this.isArray = isArray ? 1 : 0;
        }

        public int GetVersion()
        {
            return version;
        }

        public void SetVersion(int flags1)
        {
            this.version = flags1;
        }

        public int GetMetaFlag()
        {
            return metaFlag;
        }

        public void SetMetaFlag(int flags2)
        {
            this.metaFlag = flags2;
        }

        public void Read(ByteArray inData)
        {
            type = inData.ReadStringNull();
            name = inData.ReadStringNull();
            size = inData.ReadInt();
            index = inData.ReadInt();
            isArray = inData.ReadInt();
            version = inData.ReadInt();
            metaFlag = inData.ReadInt();
        }
    }
}
