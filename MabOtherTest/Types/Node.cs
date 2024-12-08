using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode : WritableObject
    {
        public List<ANode> nodes = new();
        public override void Write()
        {
            file.WriteType(0x4E4F4445);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);
            file.WriteListOfIWritable(nodes);


        }


    }
}
