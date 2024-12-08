using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionEOFC : WritableObject
    {

        public SectionEOFC(int shift)
        {
            AddOffsetShift(shift);
        }
        public override void Write()
        {

            file.WriteType(0x454F4643);
            file.WriteType(0);
            file.WriteType(0x10);
            file.WriteType(0x0);

        }
    }
}
