using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    public class WritableParamArray<T> : WritableParam where T : WritableObject
    {
        List<T> Param = new();

        public WritableParamArray()
        {
        }

        public WritableParamArray(List<T> param)
        {
            this.Param = param;
        }
        public override void Write()
        {
            file.WriteType(Param.Count);
            file.WriteListOfIWritableSinglePointer(Param);
        }
    }
}
