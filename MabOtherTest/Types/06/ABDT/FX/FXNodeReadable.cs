using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    //why, WritableParam, because some parameters can contain FXNode also O_O
    public class FXNodeReadable : FXNodeBase
    {
        public override Y Read<T, Y>()
        {
            var point = file.GetPosition();
            var base_offset = file.GetMark<uint>(this, "Offset");
            var Flag1 = file.ReadType<uint>();
            var Flag2 = file.ReadType<uint>();
            var _Type = file.ReadType<uint>();
            FXNodeBase fxbase = new FXNodeBase();
            switch (_Type)
            {
                case 0x28:
                    fxbase = new FXNode0x28_MSNO();
                    break;
                case 0x14:
                    fxbase = new FXNode0x14_MSNC();
                    break;
                case 0x15:
                    fxbase = new FXNode0x15_MRAD();
                    break;
                case 0x1F4:
                    fxbase = new FXNode0x1F4_MMTS();
                    break;
                default:
                    Console.WriteLine($"FXNODE UNK : {_Type:x},point:{point:x}");
                    break;
            }
    
            file.Jump(point);
            fxbase.GetFFile().OpenFile(this.file);
            fxbase.GetFFile().SetMark(fxbase, "Offset", base_offset); //:)
            var result = (Y)(fxbase.Read<IWritable, IWritable>() as IWritable);
            return (Y)(result as IWritable);
        }
    }
}
