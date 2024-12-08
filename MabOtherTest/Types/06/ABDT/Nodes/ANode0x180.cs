using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{
    public class ANode180 : ANodeBase
    {
        public ANode180(uint NodesFlag)
        {
            this.nodesflag = NodesFlag;
        }

        public ANode180()
        {
            this.nodesflag = 8;
        }


        //Index-Node-Only
        public List<ANodeBase> nodes2 = new();
        public List<ANodeBase> nodes3 = new();
        public List<ANodeBase> nodes4 = new();


        //RootFXNode
        public List<FXRoot> nodes5 = new();
        public List<FXRoot> nodes6 = new();
        public List<FXRoot> nodes7 = new();


        //parameters
        public override uint GetType()
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

        public override void WriteNodes()
        {
            base.WriteNodes();

            // 
            if (nodes2.Count > 0)
            {
                nodes.ForEach((node) =>
                {
                    node.Index0 = (uint?)nodes2.Count; 
                });
            }
            if (nodes3.Count > 0)
            {
                nodes.ForEach((node) =>
                {
                    node.Index1 = (uint?)nodes2.Count;
                });
            }
            if (nodes4.Count > 0)
            {
                nodes.ForEach((node) =>
                {
                    node.Index2 = (uint?)nodes2.Count;
                });
            }

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

            file.WriteType(0); // ?not sure
        }
 


    }
}
