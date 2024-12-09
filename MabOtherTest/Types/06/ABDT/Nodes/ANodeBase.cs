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

 
        public List<uint> Indexes = new();

        /*
        //3-is max
        public uint? Index0 = null;
        public uint? Index1 = null;
        public uint? Index2 = null;

        public bool HaveIndex0 = false;
        public bool HaveIndex1 = false;
        public bool HaveIndex2 = false;

        public bool HaveIndex0_Child = false;
        public bool HaveIndex1_Child = false;
        public bool HaveIndex2_Child = false;
        */

        public ANodeBase()
        {

        }
        public ANodeBase(int IndexCount)
        {
            Indexes = new List<uint>(IndexCount);
        }

        public virtual uint GetNodeType()
        {
            return 0x40;
        }
        public virtual void WriteParameters()
        {

        }
        public virtual void WriteIndex()
        {
            for (int i = 0;  i<Indexes.Count;i++) {
                file.WriteType(Indexes[i]);
            }
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
            file.WriteType(GetNodeType());
            file.WriteType(0xFFFFFFFF);
            WriteParameters();
            WriteNodes();
    
        }

        public virtual void ReadHeadIndex()
        {
            for (int i = 0; i < Indexes.Capacity; i++)
            {
                Indexes[i]=(file.ReadType<uint>());
            }
        }
        public virtual void ReadHead()
        {
            var NodeType = file.ReadType<uint>();
            var NodeFlag = file.ReadType<uint>();
            Console.WriteLine($"Node [{NodeType:x}:{NodeFlag:x}:{file.GetPosition():x}]");
        }
        public override Y Read<T,Y>()
        {
            ReadHeadIndex();
            ReadHead();
            ReadParemeters();
            var ChildrenFlag = file.ReadType<uint>();
            var NodesCount = file.ReadType<uint>();
            var NodesOffset = file.ReadType<uint>();
            file.SetMark<(uint,uint,uint)>(this,"ChildCountOffset",(ChildrenFlag,NodesCount,NodesOffset));
            ReadChildren();

            return (Y)(this as IWritable);

        }
        public virtual void ReadParemeters()
        {

        }
        public virtual void ReadChildren()
        {
            //ReadListOfIWriteable :)
            var data = file.GetMark<(uint, uint, uint)>(this, "ChildCountOffset");
            var base_offset = file.GetMark<uint>(this, "Offset");
            this.nodes = file.ReadListOfIWriteableAt<ANodeReadable,ANodeBase>(data.Item3,base_offset,data.Item2,new object[] {Indexes.Capacity}).OfType<ANodeBase>().ToList();

        }

        public override void ResetRead()
        {
            base.ResetRead();
            for (int i = 0;i<nodes.Count;i++)
            {
                nodes[i].ResetRead();
            }

        }


    }
}
