using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionTest : WritableObject
    {
        SectionTest2 data = new();
        public SectionTest(int shift)
        {
            AddOffsetShift(shift);
        }
        public override void Write()
        {
            file.WriteType(0x54455354);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);
            data.SetOffsetShift(0x20);
//            file.WriteType(data);




        }
    }
}
