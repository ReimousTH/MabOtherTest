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



    }
}
