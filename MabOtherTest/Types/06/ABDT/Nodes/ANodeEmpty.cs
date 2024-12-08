using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANodeEmpty : ANodeBase
    {
        //parameters
        public ANodeEmpty() {
        }

     
        public override void Write()
        {
            WriteIndex();
        }
   

    }
}
