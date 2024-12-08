using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class FXNode0x15_MRAD : FXNodeBase
    {


        //rotation probably? (Radian?)

        public FXNode0x15_MRAD():base(7,0xFFFFFFFF) {

            
            parameters = new List<WritableParam>()
            {
                new WritableParamFloat(0.0f),
                new WritableParamFloat(10.0f),
                new WritableParamFloat(0.0f),
                new WritableParamFloat(0.0f),

            };
        }


        public override uint GetType()
        {
            return 0x15;
        }
    
    }
}
