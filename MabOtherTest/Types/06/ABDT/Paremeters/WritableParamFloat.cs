using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class WritableParamFloat : WritableParam
    {
        float param;
        public WritableParamFloat(float param)
        {
            this.param = param;
        }
        public override void Write()
        {
            file.WriteTypeBase(param);
        }

        public static explicit operator WritableParamFloat(float param)
        {
            return new WritableParamFloat(param);
        }


    }
}
