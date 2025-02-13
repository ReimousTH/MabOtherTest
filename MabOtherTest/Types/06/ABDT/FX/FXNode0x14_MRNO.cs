using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class FXNode0x14_MSNC : FXNodeBase
    {


        //rotation probably?
        //base-params idek
        public FXNode0x14_MSNC():base(7,0xFFFFFFFF) {

            
            parameters = new List<WritableParam>()
            {
                new WritableParamFloat(0),
                new WritableParamFloat(0),
                new WritableParamFloat(90.0f),

            };
        }


        public override uint GetType()
        {
            return 0x14;
        }
        public override Y Read<T, Y>()
        {
            return base.Read<T, Y>();
        }

    }
}
