using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    public class SectionBBINA : WritableObject
    {

        public SectionBBINA():base()
        {


        }
        public override void Write()
        {
            point = file.GetPosition();
            file.WriteType(0); file.WriteType(0); file.WriteType(0); file.WriteType(0); ;
            file.WriteType(0); file.WriteType(0x00003142); file.WriteType(0x42494E41); file.WriteType(0); ;

        }
        public override void AfterFlush()
        {
            long offset_last = 0;
            var _array_ = GetFlushedOffsets().Select(offset =>
            {
                //why 0x20 because it bbina size :)
                var offset_i = ((offset-offset_last) - 0x20 - point) / 4;
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
                offset_last = offset_i;
                return file.GetBytes();
            }).SelectMany(b => b).ToArray();

            var point_2 = file.GetPosition();
            file.WriteTypeAt(point + 8, _array_.Length);
            file.WriteByteArray(_array_);
            file.PadFile0x10();
            file.WriteTypeAt(point,file.GetLength());
            file.WriteTypeAt(point + 4, point_2-point-0x20);

        }
        public virtual void AfterBBINAWrite()
        {

        }

        public override Y Read<T,Y>()
        {
            var point = file.GetPosition();
            var BBINA_SIZE = file.ReadType<uint>();
            var BBINA_DATA_SIZE = file.ReadType<uint>();
            var BBINA_BINA_SIZE = file.ReadType<uint>();
            file.Jump(4, SeekOrigin.Current);
            file.Jump(4, SeekOrigin.Current);
            var BBINA_VERSION = file.ReadType<uint>();
            var BBINA_MAGIC = file.ReadType<uint>();
            file.Jump(4, SeekOrigin.Current);

        



            return (Y)(this as IWritable);

        }
    }
}
