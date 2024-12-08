using MabOtherTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.Types
{

    public class FXNode0x1F4_MMTS : FXNodeBase
    {


        //base-params idek
        public FXNode0x1F4_MMTS() : base(3, 0xFFFFFFFF)
        {

            parameters = new List<WritableParam>()
            {
                new WritableParamUInt(0),
                new WritableParamUInt(0),
                new WritableParamUInt(0),
                new WritableParamUInt(0),
                new WritableParamFloat(1f),
                new WritableParamPointer<WritableParam>( new WritableParamGroup<WritableParam>(new List<WritableParam>()
                {
                    new WritableParamArray<WritableParam>(new List<WritableParam>()
                    {
                        new WritableParamUInt(0),
                        new WritableParamFloat(1),
                        new WritableParamFloat(-1),
                        new WritableParamFloat(-1),
                        new WritableParamUInt(0xFFFFFFFF),
                        new WritableParamUInt(2),
                        new WritableParamUInt(0),
                        new WritableParamUInt(0),
                        new WritableParamUInt(0),
                        new WritableParamUInt(0),
                        new WritableParamUInt(0)
                    })
                })),

                new WritableParamUInt(0),
                new WritableParamUInt(0),
                new WritableParamUInt(0),
            };
        }


        public override uint GetType()
        {
            return 0x1F4;
        }

    }
}
