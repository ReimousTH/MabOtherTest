using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    //ABDA_READ_FX_ROOT(828FB2E8)
    //Node 0x180......
    //0x88-0x8C (Children Count, Children Pointers->)

    //0xA0-0xA4 (Children Count, Children Pointers->)
    //0x90-0x94 (Children Count, Children Pointers->)
    //0x98-0x9C (Children Count, Children Pointers->)



    //828F6560(Here why ,  a2 = *(_DWORD *)(**(_DWORD **)(*(_DWORD *)(a1 + 0x1C) + 0xBC) + 4);)
    //but 0xA8-0xAC, 0xB0-0xB4 not defined here but i guess space allocated for nodes also
    //0xA8-0xAC (Children Count, Children Pointers->) Other Node Types I Think
    //0xB0-0xB4 (Children Count, Children Pointers->) Other Node Types I Think
    //0xB8-0xBC (Children Count, Children Pointers->[RootFXNode->FXNode])





    public class ANodeBase : WritableObject
    {
        public List<ANodeBase> nodes = new();
        public uint nodesflag = 0x8;

        //3-is max
        public uint? Index0 = null;
        public uint? Index1 = null;
        public uint? Index2 = null;
        public virtual uint GetType()
        {
            return 0x40;
        }
        public virtual void WriteParameters()
        {

        }
        public virtual void WriteIndex()
        {
            if (this.Index0 != null) file.WriteType((uint)this.Index0);
            if (this.Index1 != null) file.WriteType((uint)this.Index1);
            if (this.Index2 != null) file.WriteType((uint)this.Index2);
        }

        public virtual void WriteNodes()
        {
            file.WriteType(nodesflag);
            file.WriteType(nodes.Count);
            file.WriteListOfIWritable(nodes);
        }
        public override void Write()
        {
            WriteIndex();
            file.WriteType(GetType());
            file.WriteType(0xFFFFFFFF);
            WriteParameters();
            WriteNodes();
    
        }


    }
}
