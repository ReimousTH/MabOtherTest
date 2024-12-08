using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabOtherTest.BaseFile
{
    public static class Padding
    {
        public static string GetPadded(this string str,int pad)
        {
            return str.PadRight(GetPadding(str.Length, pad) + str.Length,'\0');
        }
        public static int GetPadding(int value,int pad)
        {
            if ( (value &~(pad-1)) != value)
            {
                return ((value + pad - 1) & ~(pad - 1)) - value;
            }
            else
            {
                return 0;
            }
        }
        public static int GetPaddedValue(int value,int pad)
        {
            if ((value & ~(pad - 1)) != value)
            {
                return ((value + pad - 1) & ~(pad - 1));
            }
            else
            {
                return value;
            }
        }

    }
}
