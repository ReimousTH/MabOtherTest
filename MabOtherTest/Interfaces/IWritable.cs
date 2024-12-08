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
    }
}
