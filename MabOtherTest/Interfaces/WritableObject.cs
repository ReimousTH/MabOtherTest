using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabOtherTest.BaseFile;


namespace MabOtherTest.Interfaces
{
    public abstract class WritableObject : IWritable
    {
        public FFile file = new();
        public uint point = 0;

        public abstract void Write();
        public void Flush()
        {
            file.FlushOffsets();
            AfterFlush();
        }
        public virtual void AfterFlush()
        {
        }


        public void SetOffsetShift(int offset)
        {
            file.SetShift(offset);
        }
        public void AddOffsetShift(int offset)
        {
            file.SetShift(file.GetShift() + offset);
        }
        public int GetOffsetShift()
        {
            return file.GetShift();
        }

        public byte[] GetBytes()
        {
            return (byte[])this.file;
        }

        public List<uint> GetFlushedOffsets()
        {
            return file.GetFlushedOffsets();
        }

        public FFile GetFFile()
        {
            return file;
        }

        public WritableObject()
        {
         
        }

        public static explicit operator byte[](WritableObject value)
        {
            return value.file.ReadBytesAt(0, value.file.GetLength());
        }

    }
}
