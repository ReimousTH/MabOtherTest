using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
 
    public class FXRoot : WritableObject
    {
        public List<FXNodeBase> nodes = new();
        uint index;

        public FXRoot() {
            this.index = 0x0;
        }
        public FXRoot(uint index)
        {
            this.index = index;
        }
        public override void Write()
        {
            file.WriteType(0); // ?
            file.WriteType(0); // index 
            file.WriteType(0); // 


            file.WriteType(0);
            file.WriteType(1); // No Idea?
            file.WriteType(0);

            file.WriteType(nodes.Count);
            file.WriteListOfIWritable(nodes); //FXBase (need to make) (0x1C)

        }

        public override void ResetRead()
        {
            base.ResetRead();
            for (int i =  0; i < nodes.Count; i++)
            {
                nodes[i].ResetRead();
            }
        }


    }
}
