using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode105 : ANodeBase
    {
  

        public ANode105(uint NodesFlag)
        {
            this.nodesflag = NodesFlag;
        }

        public ANode105() {
            this.nodesflag = 1;
        }
        //parameters
        public override uint GetType()
        {
            return 0x105;
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
            file.WriteType<uint>(1);

            file.WriteType<uint>(0xFFFFFFFF);
            file.WriteType<uint>(0xFFFFFFFF);

            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
        }

    }
}
