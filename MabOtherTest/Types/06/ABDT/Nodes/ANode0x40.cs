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

        public ANode40(uint NodesFlag)
        {
            this.nodesflag = NodesFlag;
        }

        public ANode40()
        {
            this.nodesflag = 0;
        }


        //parameters
        public override uint GetType()
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

    }
}
