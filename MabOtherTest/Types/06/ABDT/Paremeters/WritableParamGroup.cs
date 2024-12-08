using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    public class WritableParamGroup<T> : WritableParam where T : WritableObject
    {
        List<T> GroupParam = new();

        public WritableParamGroup()
        {
        }

        public WritableParamGroup(List<T> gparam)
        {
            this.GroupParam = gparam;
        }
        public override void Write()
        {
            file.WriteType(GroupParam.Count);
            file.WriteListOfIWritable(GroupParam);
        }
    }
}
