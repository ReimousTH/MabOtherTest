using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode80 : ANode180_AKLF
    {
        public ANode80(int i):base(i) { }

        public override uint GetNodeType()
        {
            return 0x80;
        }
    }
}
