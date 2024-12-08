using MabOtherTest.Interfaces;
using MabOtherTest.Types;

namespace MabOtherTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            SectionMRAB MRRAB = new SectionMRAB(0);
            MRRAB.abda.aBDT.Root = new ANode40()
            {
                nodes = new List<ANodeBase>() { new ANode105(1) {
                    nodes = new List<ANodeBase>() { new ANode180(0)
                    {
                    nodes = new List<ANodeBase>()
                    {
                        new ANode01(){Index0 = 1} // here preffered
                    },
                    //Index Nodes (Empty)
                    nodes2 = new List<ANodeBase>()
                    {
                        new ANodeEmpty(){Index0 = 0}
                    },
                    nodes7 = new List<FXRoot>()
                    {
                        new FXRoot(0)
                        {
                            nodes = new List<FXNodeBase>()
                            {
                                new FXNode0x1F4_MMTS(),
                                new FXNode0x28_MSNO(),
                                new FXNode0x14_MSNC(),
                                new FXNode0x15_MRAD()
                            }
                        }
                    }
                    } }
            } }
            };
            MRRAB.abrs.Nodes = new List<ABRS_NODE>() {
                new("particle/xno_model/ef_circle000_a_x_00.xno", "NXIF"),
                new("particle/xno_model/ef_circle000_a_x_00.xnv", "NXIF"),
            };

            MRRAB.Write();
            MRRAB.Flush();
            //var offsets = MRRAB.GetFlushedOffsets();

            /*
            var MRRAB = new FXRoot()
            {
                nodes = new List<FXNodeBase>()
                {
                    new FXNode0x1F4_MMTS()
                    {

                    }
                }
            };
            MRRAB.Write();
            MRRAB.Flush();
            */
            // D:\Games\X360\Sonic06\NoArcsVersion\Sonic06NoArc\Game\xenon\effect\player\sonic
            MRRAB.file.WriteToExternalDrive("D:\\Games\\X360\\Sonic06\\NoArcsVersion\\Sonic06NoArc\\Game\\xenon\\effect\\player\\sonic\\so_spincharge.mab");
        }
    }
}