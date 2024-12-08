using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class FXNode0x28_MSNO : FXNodeBase
    {


        //base-params idek
        public FXNode0x28_MSNO():base(3,0xFFFFFFFF) {

            
            parameters = new List<WritableParam>()
            {
                new WritableParamFloat(1.0f),
                new WritableParamFloat(1.2f),
                new WritableParamFloat(1.0f),

            };
        }


        public override uint GetType()
        {
            return 0x28;
        }
    
    }
}
