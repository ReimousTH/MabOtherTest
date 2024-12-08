using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionMRAB : SectionBBINA
    {

        public SectionABDA abda = new(0);
        public SectionABRS abrs = new(0);
        public SectionMRAB(int shift):base()
        {
            AddOffsetShift(shift);
  
        }
        public override void Write()
        {

            file.AddOffsetShift(-0x30);
            file.WriteType(0x4D524142);
            file.WriteType(0x10);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0);

            base.Write();
            file.WritePointerTypeParam(abda, BaseFile.PointerIWritablePrm.DoNotKeepParentOffsets);
            file.WritePointerTypeParam(abrs, BaseFile.PointerIWritablePrm.DoNotKeepParentOffsets);
            file.WriteType(0); file.WriteType(0);
            file.WriteType(0); file.WriteType(0); file.WriteType(0); file.WriteType(0);


        }
        public override void AfterFlush()
        {
            base.AfterFlush();
            file.WriteTypeAt(0x8, file.GetLength() - 0x10);

        }
    }
}
