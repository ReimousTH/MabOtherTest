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
    public class ANode00 : ANodeBase
    {
        public ANode00()
        {

        }
        public ANode00(int IndexCount) : base(IndexCount)
        {
        }
   

        //parameters
        public override uint GetNodeType()
        {
            return 0x0;
        }

        public override void Write()
        {
            WriteIndex();
            file.WriteTypeBase(GetNodeType());
        }
        public override void WriteParameters()
        {
            
        }

        public override Y Read<T,Y>()
        {
            ReadHeadIndex();
         
            return (Y)(this as IWritable);
        }

    }
}
