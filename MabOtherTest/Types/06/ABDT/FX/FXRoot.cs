using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    //same as FXNodeBase, but 0x180,0x80 gives one extra index (int) value soo thats why :)
    public class FXRoot : WritableObject
    {
        public List<FXNodeBase> nodes = new();
        uint index;
        uint Flag = 0;
        uint Flag2 = 0;

        public FXRoot() {
            this.index = 0x0;
        }
        public FXRoot(uint index)
        {
            this.index = index;
        }
        public override void Write()
        {
            file.WriteType(0); // ?
            file.WriteType(0); // index 
            file.WriteType(0); // 


            file.WriteType(0);
            file.WriteType(1); // No Idea? (Root Flag)
            file.WriteType(0);

            file.WriteType(nodes.Count);
            file.WriteListOfIWritable(nodes); //FXBase (need to make) (0x1C)

        }

        public override void ResetRead()
        {
            base.ResetRead();
            for (int i =  0; i < nodes.Count; i++)
            {
                nodes[i].ResetRead();
            }
        }
        public override Y Read<T, Y>()
        {
            var point = file.GetPosition();
            var base_offset = file.GetMark<uint>(this, "Offset");

            this.index = file.ReadType<uint>();
            this.Flag = file.ReadType<uint>();
            var _Type = file.ReadType<uint>();
            this.Flag2 = file.ReadType<uint>();
            var count = file.ReadType<uint>();
            var offset = file.ReadType<uint>();

            this.nodes = file.ReadListOfIWriteableAt<FXNodeReadable, FXNodeBase>(count, offset, base_offset);

         
  
            return (Y)(this as IWritable);
        }

    }
}
