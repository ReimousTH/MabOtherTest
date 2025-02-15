﻿using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode01 : ANodeBase
    {
        public ANode01()
        {

        }
        public ANode01(int IndexCount) : base(IndexCount)
        {
        }


        //parameters
        public override uint GetNodeType()
        {
            return 0x1;
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
