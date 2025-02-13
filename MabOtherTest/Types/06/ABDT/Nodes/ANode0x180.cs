using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MabOtherTest.Types
{
    //828FBD1C
    public class ANode180_AKLF : ANodeBase
    {

        public ANode180_AKLF()
        {
            this.nodesflag = 8;
        }
        public ANode180_AKLF(int IndexCount) : base(IndexCount)
        {
        }


        public ANode180_AKLF(uint NodesFlag)
        {
            this.nodesflag = NodesFlag;
        }



        //Index-Node-Only
        public List<FXRoot> nodes2 = new();
        public List<ANodeBase> nodes3 = new();
        public List<ANodeBase> nodes4 = new();


        //RootFXNode
        public List<FXRoot> nodes5 = new();
        public List<FXRoot> nodes6 = new();
        public List<FXRoot> nodes7 = new();


        //parameters
        public override uint GetNodeType()
        {
            return 0x180;
        }
        public override void WriteParameters()
        {
            file.WriteType(1); // Flag-To-Index1
            file.WriteType<float>(150f);
            file.WriteType<float>(-1);
            file.WriteType<uint>(0);
            file.WriteType<float>(-1);
            file.WriteType<float>(1.0f);

            file.WriteType<float>(2.0f);
            file.WriteType<float>(100.0f);
            file.WriteType<float>(1.0f);
            file.WriteType<float>(-1.0f);
            file.WriteType<float>(-1.0f);
            file.WriteType<float>(-1.0f);

            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<float>(-1.0f);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<float>(-1.0f);
            file.WriteType<float>(-1.0f);
            file.WriteType<float>(-1.0f);

            file.WriteType<uint>(4);
            file.WriteType<uint>(0);
            file.WriteType<uint>(1);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0);
            file.WriteType<uint>(0xFFFFFFFF);
            file.WriteType<uint>(0);
            file.WriteType<float>(-5.0f);
            file.WriteType<float>(-5.0f);
            file.WriteType<float>(5.0f);
            file.WriteType<float>(5.0f);

        }

        uint u0;
        float u1;
        float u2;
        uint u3;
        float u4;
        float u5;
        float u6;
        float u7;
        float u8;
        float u9;
        float u10;
        float u11;
        uint u12;
        uint u13;
        float u14;
        uint u15;
        uint u16;
        float u17;
        float u18;
        float u19;
        uint u20;
        uint u21;
        uint u22;
        uint u23;
        uint u24;
        uint u25;
        uint u26;
        float u27;
        float u28;
        float u29;
        float u30;
        public override void ReadParemeters()
        {
            u0 = file.ReadType<uint>(); 
            u1 = file.ReadType<float>();
            u2 = file.ReadType<float>();
            u3 = file.ReadType<uint>();
            u4 = file.ReadType<float>();
            u5 = file.ReadType<float>();

            u6 = file.ReadType<float>();
            u7 = file.ReadType<float>();
            u8 = file.ReadType<float>();
            u9 = file.ReadType<float>();
            u10 = file.ReadType<float>();
            u11 = file.ReadType<float>();

            u12 = file.ReadType<uint>();
            u13 = file.ReadType<uint>();
            u14 = file.ReadType<float>();
            u15 = file.ReadType<uint>();
            u16 = file.ReadType<uint>();
            u17 = file.ReadType<float>();
            u18 = file.ReadType<float>();
            u19 = file.ReadType<float>();

            u20 = file.ReadType<uint>();
            u21 = file.ReadType<uint>();
            u22 = file.ReadType<uint>();
            u23 = file.ReadType<uint>();
            u24 = file.ReadType<uint>();
            u25 = file.ReadType<uint>();
            u26 = file.ReadType<uint>();

            u27 = file.ReadType<float>();
            u28 = file.ReadType<float>();
            u29 = file.ReadType<float>();
            u30 = file.ReadType<float>();
        }
        public override void ReadChildren()
        {
            var baseoffs = file.GetMark<uint>(this, "Offset");
            var offset = file.GetPosition();

            var nodes_saved = file.GetMark<(uint, uint, uint)>(this, "ChildCountOffset");

            var nodes2_count = file.ReadType<uint>();   //0x90,0x94
            var nodes2_offset = file.ReadType<uint>();
             
            var nodes3_count = file.ReadType<uint>(); //0x98,0x9C
            var nodes3_offset = file.ReadType<uint>(); 

            var nodes4_count = file.ReadType<uint>();   //0xA0,0xA4
            var nodes4_offset = file.ReadType<uint>();



            //FX -PARTS ( for each ??? )
            var nodes5_count = file.ReadType<uint>();
            var nodes5_offset = file.ReadType<uint>();

            var nodes6_count = file.ReadType<uint>();
            var nodes6_offset = file.ReadType<uint>();


            var nodes7_count = file.ReadType<uint>();
            var nodes7_offset = file.ReadType<uint>();


            uint flags = (uint)new int[] { (int)nodes_saved.Item2,(int)nodes2_count, (int)nodes3_count, (int)nodes4_count }.Select(i => i > 0 ? 1 : 0).ToList().Sum();


            Console.WriteLine($"Node{GetNodeType():x} ChildFlag : {nodes_saved.Item1}");

            var flg = nodes_saved.Item1;
         
            if (flg == 8 ) // 828FADD4 so_faint
            {
                flags = 1;
            }
            else if (flg == 0)
            {
                flags = 2;
            }
            this.index_count_pass = (int)flags;



            if (nodes5_count > 0 || nodes6_count > 0 || nodes7_count > 0)
            {
                //  Console.WriteLine($"Node0x180[FX] : {(nodes5_count + baseoffs):x}:{(nodes6_offset + baseoffs):x}:{(nodes7_offset + baseoffs):x}");
            }
            var base_offset = file.GetMark<uint>(this, "Offset");
      //      this.nodes2 = file.ReadListOfIWriteableAt<FXRoot, FXRoot>(nodes2_offset, base_offset, nodes2_count, new object[] { });
            base.ReadChildren(); //
        }


        public void CheckNode(List<ANodeBase> nodes,uint value)
        {
            foreach (var node in nodes)
            {
                if (node.Indexes.Count != value)
                {
                    Console.WriteLine($"Node{GetNodeType():x} Method CheckNode(), ChildNode : {node.GetNodeType():x} Does Not Mach Index Count");
                }
            }
        }

        public override void WriteNodes()
        {
            base.WriteNodes();

            uint flags = (uint)new int[] { nodes.Count, nodes2.Count, nodes3.Count, nodes4.Count }.Select(i => i > 0 ? 1 : 0).ToList().Sum();
            flags -= 1;
            if (flags <= 0) flags = 0;

            CheckNode(nodes,flags);
      //      CheckNode(nodes2,flags);
            CheckNode(nodes3,flags);
            CheckNode(nodes4,flags);



            file.WriteType(nodes2.Count);
            file.WriteListOfIWritable(nodes2);

            file.WriteType(nodes3.Count);
            file.WriteListOfIWritable(nodes3);

            file.WriteType(nodes4.Count);
            file.WriteListOfIWritable(nodes4);


            file.WriteType(nodes5.Count);
            file.WriteListOfIWritable(nodes5);

            file.WriteType(nodes6.Count);
            file.WriteListOfIWritable(nodes6);

            file.WriteType(nodes7.Count);
            file.WriteListOfIWritable(nodes7);



          //  file.WriteType(0); // ?not sure
        }
 


    }
}
