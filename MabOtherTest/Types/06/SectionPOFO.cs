using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionPOFO : SectionEOFC
    {

        public SectionPOFO(int shift):base(shift) 
        {
            AddOffsetShift(shift);
        }
        public override void Write()
        {
            var point = file.GetPosition();
            file.WriteType(0x504F4630);
            file.WriteType(0xFFFFFFF1);
            file.WriteType(0x10);// h size
            file.WriteType(0);


            long offset_start = 0; // This will keep track of the cumulative offset
            var tried = GetFlushedOffsets().Order();
            var _array_ = GetFlushedOffsets().Order().Select(offset =>
            {
                Console.WriteLine($"offset : {offset:x}");
                var offset_i = (offset - offset_start) / 4; // Divide by 4 as per your original logic
                FFile file = new FFile();

                if (offset_i <= 63)
                {
                    // For offsets between 0 and 63
                    byte value = (byte)((offset_i & 0x3F) | 0x40);
                    file.WriteType<byte>(value);
                }
                else if (offset_i <= 16383)
                {
                    // For offsets between 64 and 16383
                    short value = (short)((offset_i & 0x3FFF) | 0x8000); // Use the upper bit to indicate this range
                    file.WriteType<short>(value);
                }
                else if (offset_i <= 4194303)
                {
                    // For offsets between 16384 and 4194303
                    int value = (int)((offset_i & 0x3FFFFF) | 0xC00000); // Use the upper bits to indicate this range
                    file.WriteType<int>(value);
                }
                offset_start = offset;

                return file.GetBytes();
            }).SelectMany(b => b).ToList();

            //DO PADDING LATER THIS IS REQUIRED TO WORK PROPERLY 
            _array_.Add(0);
            _array_.Add(0);
            _array_.Add(0);
            _array_.Add(0);


            file.WriteType(_array_.Count);
            file.WriteByteArray(_array_.ToArray());
    

            file.PadFile0x10();
            file.WriteTypeAt(point + 4, file.GetLength()-point - 0x10); //SIZE


            AfterPOFOWrite();
            base.Write();

        }
        public virtual void AfterPOFOWrite()
        {

        }
    }
}
