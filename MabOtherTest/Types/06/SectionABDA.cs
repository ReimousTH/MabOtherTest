using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MabOtherTest.Types
{
    public class SectionABDA : SectionPOFO
    {

        public SectionABDT aBDT = new(0);
        public SectionABDA(int shift):base(shift)
        {
            AddOffsetShift(shift);
        }
        public override void Write()
        {
            this.point = file.GetPosition();
            file.WriteType(0x41424441);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0x30);// h size
            file.WriteType(0x0);

            file.WriteType(0x77917325);
            file.WriteType(1); //Count of abdt i guess may be something else?, implement later
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0);


            //bbina 
            file.WritePointerType(aBDT);


            file.WriteType(0);
            file.WriteType(0);
            file.WriteType(0);

        

           
        }

        public override void AfterFlush()
        {
            var what = this.GetFlushedOffsets();
            var pre_len = file.GetLength();


            file.WriteTypeAt(point + 0x18, file.GetLength());
            base.Write();
            file.WriteTypeAt(point + 0x4, pre_len); // ?? makes no sense at all 
        }
    }
}
