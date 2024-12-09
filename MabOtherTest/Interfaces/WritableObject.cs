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


        public virtual Y Read<T, Y>() where T : IWritable where Y : IWritable
        {
            return (Y)(this as IWritable); 
        }
        public virtual void OnMarkSet()
        {
            this.file.SetMark<uint>(this, "Offset", file.GetPosition());
        }

        public T Read<T>() where T : IWritable
        {
            return Read<T, T>();
        }
        public virtual void ResetRead()
        {
            file = new FFile();
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
