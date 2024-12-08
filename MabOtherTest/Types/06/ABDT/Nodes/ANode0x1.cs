using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode01 : ANodeBase
    {
  
        //parameters
        public override uint GetType()
        {
            return 0x1;
        }

        public override void Write()
        {
            WriteIndex();
            file.WriteTypeBase(GetType());
        }
        public override void WriteParameters()
        {
            
        }

    }
}
