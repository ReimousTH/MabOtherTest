using MabOtherTest.Interfaces;

namespace MabOtherTest.BaseFile
{

    struct PointerIWritable
    {
        private PointerIWritablePrm Prm;
        private IWritable obj;

        public PointerIWritablePrm GetParameter()
        {
            return Prm;
        }
        public IWritable GetIWritable()
        {
            return obj;
        }
        public PointerIWritable(IWritable writable)
        {
            Prm = PointerIWritablePrm.None;
            obj = writable;
        }
        public PointerIWritable(IWritable writable, PointerIWritablePrm prm = PointerIWritablePrm.None) : this(writable)
        {
            Prm = prm;
        }

    }
}
