﻿using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    //why, WritableParam, because some parameters can contain FXNode also O_O
    public class FXNodeBase : WritableParam
    {
        //Writable-Param, 
        public List<WritableParam> parameters = new();
        uint Flag; // no idea for what 
        uint Flag2; 

        public FXNodeBase()
        {

        }
        public FXNodeBase(uint  Flag, uint Flag2)
        {
            this.Flag = Flag;
            this.Flag2 = Flag2;
        }


        public virtual uint GetType()
        {
            return 0x0;
        }
     
        public override void Write()
        {
            file.WriteType(Flag); // ?? 
            file.WriteType(GetType()); // index 
            file.WriteType(Flag2); // index 

            file.WriteType(0);
            file.WriteType<float>(-1f);

            file.WriteType(parameters.Count);
            file.WriteListOfIWritableSinglePointer(parameters); //FXBase (need to make)
        }


    }
}
