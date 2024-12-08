using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    public class WritableParamPointer<T> : WritableParam where T : WritableObject
    {
        T Param;
        public WritableParamPointer(T param)
        {
            this.Param = param;
        }
        public override void Write()
        {
            file.WritePointerType(Param);
        }
    }
}
