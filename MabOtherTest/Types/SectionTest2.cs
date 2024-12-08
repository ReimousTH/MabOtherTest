using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class SectionTest2 : WritableObject
    {
        ANode root = new ANode();

        public override void Write()
        {

         
            file.WriteType(0x54455332);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);

            
            root = new ANode()
            {
                nodes = new List<ANode> { new ANode() }
            };

            file.WriteType(root);

        }
    }
}
