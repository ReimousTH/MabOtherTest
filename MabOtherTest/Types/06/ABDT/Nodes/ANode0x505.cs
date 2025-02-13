using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode505 : ANodeBase
    {

        public ANode505()
        {
      
        }
        public ANode505(int IndexCount):base(IndexCount)
        {
        }


        //parameters
        public override uint GetNodeType()
        {
            return 0x505;
        }
        public override void WriteParameters()
        {
            file.WriteType<float>(-1);
            file.WriteType<float>(-1);
            file.WriteType<uint>(0);
            file.WriteType<float>(-1);

            file.WriteType<uint>(1);
            file.WriteType<uint>(0);

            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0);

            file.WriteType<uint>(0xFFFFFFFF);
            file.WriteType<uint>(0xFFFFFFFF);

            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
        }

        float u0;
        float u1;
        uint u2;
        float u3;
        uint u4;
        uint u5;
        uint u6;
        uint u7;
        uint u8;
        uint u9;
        uint u10;
        uint u11;
        uint u12;
        uint u13;

        public override void ReadParemeters()
        {

            u0 = file.ReadType<float>();
            u1 = file.ReadType<float>();
            u2 = file.ReadType<uint>();
            u3 = file.ReadType<float>();

            u4 = file.ReadType<uint>();
            u5 = file.ReadType<uint>();

            u6 = file.ReadType<uint>();
            u7 = file.ReadType<uint>();
            u8 = file.ReadType<uint>();
           
            u9 = file.ReadType<uint>();
            u10 = file.ReadType<uint>();
       
            u11 = file.ReadType<uint>();
            u12 = file.ReadType<uint>();
            u13 = file.ReadType<uint>();

        }
        public override void ResetRead()
        {
            base.ResetRead();
        }

    }
}
