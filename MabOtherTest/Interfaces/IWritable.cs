using MabOtherTest.BaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Interfaces
{
    public interface IWritable
    {
        public void Write();
        public void Flush();
        public void SetOffsetShift(int offset);
        public void AddOffsetShift(int offset);
        public int GetOffsetShift();


        public FFile GetFFile();

        public List<uint> GetFlushedOffsets();

        public byte[] GetBytes();




        public Y Read<T, Y>() where T : IWritable where Y : IWritable;
        public T Read<T>() where T : IWritable;

        public void ResetRead(); //Reset All Read Mostly file = new FFile()

        public void OnMarkSet();

    }
}
