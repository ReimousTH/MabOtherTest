using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionABDT : WritableObject
    {
        public SectionABDT() { 
        }
        public SectionABDT(int shift) 
        {
        }
        public SectionABDT(ANode40 root)
        {
            this.Root = root;
        }

        public ANode40 Root = new();

        public override void Write()
        {
            this.point = file.GetPosition();
            file.WriteType(0x41424454);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0x10);// h size
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0x77917325);
            Root.SetOffsetShift(0x14);
            file.WriteType(Root);
        }

        public override void AfterFlush()
        {
            file.PadFile0x10();
            file.WriteTypeAt(point + 4, file.GetLength() - 0x10 - point);
        }

        public override Y Read<T,Y>()
        {
            uint base_offset = file.GetMark<uint>(this, "Offset");
            var point = file.GetPosition();

            var magic = file.ReadType<uint>();
            var size = file.ReadType<uint>();
            var offset_chunk_size = file.ReadType<uint>();
            file.Jump(4, SeekOrigin.Current);
            var version = file.ReadType<uint>();
            
            Root = file.ReadTypeIWritable<ANode40>(base_offset); // :)

            //Node
        
            return (Y)(this as IWritable);
 
        }
        public override void ResetRead()
        {
            base.ResetRead();
            Root.ResetRead();
        }



    }
}
