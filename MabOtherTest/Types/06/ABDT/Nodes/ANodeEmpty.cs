using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANodeEmpty : ANodeBase
    {
        //parameters
        public ANodeEmpty() {
        }


        public override uint GetNodeType()
        {
            return 0xFFFFFFFF;
        }
        public override void Write()
        {
            WriteIndex();
        }
        public override Y Read<T,Y>()
        {

            return (Y)(this as IWritable);
        }
        public override void ResetRead()
        {
            base.ResetRead();
            
        }


    }
}
