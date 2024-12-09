using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode40 : ANodeBase
    {

        public ANode40()
        {
            this.nodesflag = 0;
        }
        public ANode40(int IndexCount) : base(IndexCount)
        {
        }


        public ANode40(uint NodesFlag)
        {
            this.nodesflag = NodesFlag;
        }

    


        //parameters
        public override uint GetNodeType()
        {
            return 0x40;
        }
        public override void WriteParameters()
        {
            file.WriteType<float>(-1);
            file.WriteType<float>(-1);
            file.WriteType<uint>(0);
            file.WriteType<float>(45.0f);
            file.WriteType<uint>(0);
        }


        //
        float u0 = -1;
        float u1 = -1;
        uint u2  = 0;
        float u3 = 45.0f;
        uint u4 = 0;
        

        public override void ReadParemeters()
        {
            u0 = file.ReadType<float>();
            u1 = file.ReadType<float>();
            u2 = file.ReadType<uint>();
            u3 = file.ReadType<float>();
            u4 = file.ReadType<uint>();
        }
  

    }
}
