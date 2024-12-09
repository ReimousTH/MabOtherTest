using MabOtherTest.BaseFile;
using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    public class ANodeReadable : ANodeBase
    {
        public ANodeReadable()
        {

        }
        public ANodeReadable(int IndexCount) : base(IndexCount) { }
        public unsafe override void ReadHead()
        {
         
            var NodeType = file.ReadType<uint>();
            var NodeFlag = file.ReadType<uint>();
            file.Jump(-0x8, SeekOrigin.Current);

            file.SetMark(this, "NodeType", NodeType);
            file.SetMark(this, "NodeFlag", NodeType);

            // Get a pointer to this instance

        }


        public override Y Read<T,Y>()
        {
            var yf = typeof(Y);
            var tf = typeof(T);
            var point = file.GetPosition();
            var baseoffset = file.GetMark<uint>(this, "Offset");
            ReadHeadIndex();
            ReadHead();
            var NT = file.GetMark<uint>(this, "NodeType");
            var NF = file.GetMark<uint>(this, "NodeFlag");
            ANodeBase basenoode = new ANodeBase();
            switch (NT)
            {
                case 0x105:
                    basenoode = new ANode105();
                    break;
                case 0x180:
                    basenoode = new ANode180();
                    break;
                case 0x80:
                    basenoode = new ANode80();
                    break;
                case 0x505:
                    basenoode = new ANode505();
                    break;
                case 0x0:
                    basenoode = new ANode00();
                    break;
                case 0x1:
                    basenoode = new ANode01();
                    break;
                default:
                    Console.WriteLine($"MISSING TYPE : {NT:x}");
                    break;


            }
            file.Jump(point);

            basenoode.GetFFile().OpenFile(this.file);
            basenoode.GetFFile().SetMark(basenoode, "Offset", baseoffset); //:)

            var result = (Y)(basenoode.Read<IWritable, IWritable>() as IWritable);
           
            return result;

        }
    }
}