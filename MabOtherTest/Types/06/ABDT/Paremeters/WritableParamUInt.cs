using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class WritableParamUInt : WritableParam
    {
        uint param;
        public WritableParamUInt(uint param)
        {
            this.param = param;
        }
        public override void Write()
        {
            file.WriteTypeBase(param);
        }


    }
}
