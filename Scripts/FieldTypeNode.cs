using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class FieldTypeNode
    {
        private List<FieldTypeNode> list = new List<FieldTypeNode>();
        private FieldType type = new FieldType();

        public FieldType GetFileType()
        {
            return type;
        }

        public void SetFileType(FieldType field)
        {
            this.type = field;
        }

        

        public void Read(ByteArray inData)
        {
            type.Read(inData);

            int numChildren = inData.ReadInt();
            for (int i = 0; i < numChildren; i++)
            {
                FieldTypeNode child = new FieldTypeNode();
                child.Read(inData);
                list.Add(child);
            }
        }
    }
}
