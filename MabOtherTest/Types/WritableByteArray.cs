using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.Types
{
    internal class WritableByteArray : WritableObject
    {
        byte[] data;
        public WritableByteArray(byte[] data)
        {
            this.data = data; ;
        }
        public override void Write()
        {
           file.WriteByteArray(data);
        }
    }
}
