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

    public class ConstCharPtr: WritableObject
    {
        private string _value;
        public ConstCharPtr()
        {

        }
        public override void Write()
        {
            file.WriteByteArray((byte[])this);
           
        }

        public static implicit operator ConstCharPtr(string v)
        {
            return new ConstCharPtr() { _value = v.PadRight(0x30,'\0') };

        }
        public static explicit operator string(ConstCharPtr v)
        {
            return v._value;

        }
        public static explicit operator byte[](ConstCharPtr v)
        {
            return System.Text.ASCIIEncoding.ASCII.GetBytes(v._value);
        }


    }

    public class ABRS_NODE_FILE : WritableObject
    {
        string Name;
        string FileType; // 4

        public override void Write()
        {
            file.WriteByteArray(System.Text.ASCIIEncoding.ASCII.GetBytes(FileType));
            file.WriteType(Name.Length);
            file.WriteType(0x10);
            file.WriteType(0x00400000);
            file.WriteByteArray(System.Text.ASCIIEncoding.ASCII.GetBytes(Name));
            file.PadFile0x10();
        }
        public ABRS_NODE_FILE()
        {

        }
        public ABRS_NODE_FILE(string Name)
        {
            this.Name = Name.GetPadded(0x10);
            this.FileType = "DDS ";
        }
        public ABRS_NODE_FILE(string Name,string Type):this(Name)
        {
            this.FileType = Type;
        }
        public override Y Read<T, Y>()
        {
            var Base = file.GetMark<uint>(this, "Offset");
            var point = file.GetPosition();
            this.FileType = System.Text.ASCIIEncoding.ASCII.GetString(file.ReadBytes(4));
            this.Name = System.Text.ASCIIEncoding.ASCII.GetString( file.ReadBytesAt(point+0x10,file.ReadType<uint>()) );

            return (Y)(this as IWritable);
        }
        public override void ResetRead()
        {
            base.ResetRead();
        }



    }
    public class ABRS_NODE:WritableObject
    {
        ABRS_NODE_FILE Alfa;
        uint Flag;
     
        public ABRS_NODE()
        {

        }
        public ABRS_NODE(string Name,uint Flag,string NameType) {
            this.Alfa = new(Name,NameType);
            this.Flag = Flag;   
        }
        public ABRS_NODE(string Name):this(Name,0xFFFFFFFF,"DDS ")
        {
        }
        public ABRS_NODE(string Name,string NameType) : this(Name, 0xFFFFFFFF, NameType)
        {
        }

        public override void Write()
        {
            file.WritePointerType(Alfa);
            file.WriteType(Flag);
        }
        public override Y Read<T,Y>()
        {
            var Base = file.GetMark<uint>(this, "Offset");
            Alfa = file.ReadTypePointer<ABRS_NODE_FILE, ABRS_NODE_FILE>(Base);
            Flag = file.ReadType<uint>();
            
            return (Y)(this as IWritable);
        }
        public override void ResetRead()
        {
            base.ResetRead();
            Alfa.ResetRead();
        }
    }
    public class SectionABRS : SectionPOFO
    {
        public SectionABRS():base(0)
        {

        }

        public List<ABRS_NODE> Nodes = new();
        
        private uint ABRS_OFFSET_TABLE_SIZE;
        private uint ABRS_OFFSET_DATA_SIZE;
        public SectionABRS(int shift):base(shift)
        {
            AddOffsetShift(shift);
           
        }

        public override void Write()
        {
            file.WriteType(0x41425253);
            file.WriteType(0xFFFFFFFF);
            file.WriteType(0xFFFFFFFF);// h size
            file.WriteType(0);


            file.WriteType(0x77917325); //versi
            file.WriteType(Nodes.Count);
            file.WriteType(0);

            file.WriteArrayType(Nodes);

  
            file.PadFile0x10();

            ABRS_OFFSET_TABLE_SIZE = file.GetLength();
            file.WriteTypeAt<uint>(0x8, file.GetLength());


        }

        public override void AfterPOFOWrite()
        {
            var POFO_SIZE = file.GetLength() - ABRS_OFFSET_TABLE_SIZE;
            file.WriteTypeAt<uint>(0x4, POFO_SIZE);
        }
        public override void AfterFlush()
        {
            file.PadFile0x10();
            file.WriteTypeAt<uint>(0x18, file.GetLength());
            base.Write();
        }


        public override Y Read<T, Y>()
        {
            uint base_offset = file.GetMark<uint>(this, "Offset");
            var point = file.GetPosition();

            var magic = file.ReadType<uint>();
            var size = file.ReadType<uint>();
            var offset_chunk_size = file.ReadType<uint>();
            file.Jump(4, SeekOrigin.Current);
            var version = file.ReadType<uint>();
            var count = file.ReadType<uint>();
            file.Jump(4, SeekOrigin.Current); // size skip

            Nodes = file.ReadArrayType<ABRS_NODE,ABRS_NODE>(point,count);


            return (Y)(this as IWritable);
        }
        public override void ResetRead()
        {
            base.ResetRead();
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].ResetRead();
            }
        }
    }
}
