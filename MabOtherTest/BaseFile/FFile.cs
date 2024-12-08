using MabOtherTest.Interfaces;
using MabOtherTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MabOtherTest.BaseFile
{
    public partial class FFile : IWritable
    {

        #region Fields
        Stream stream = new MemoryStream();
        List<KeyValuePair<uint, PointerIWritable>> _flush_offsets = new List<KeyValuePair<uint, PointerIWritable>>();


        List<uint> _flushed_offsets = new();
        int shift = 0;
        #endregion

        #region Constructor
        public FFile()
        {

        }
        #endregion


        #region Operators

        public static explicit operator byte[](FFile file)
        {
            return file.ReadBytesAt(0, file.GetLength());
        }

        #endregion


        #region CommonShiftMethods

        //SHIFT
        public void SetShift(int shift)
        {
            this.shift = shift;
        }
        public int GetShift()
        {
            return shift;
        }

        #endregion


        #region CommonFFileMethods
        public uint GetLength()
        {
            return (uint)stream.Length;
        }

        public uint GetLengthWithShift()
        {
            return (uint)stream.Length + (uint)GetShift();
        }
        public uint GetLengthP0x10()
        {
            if ((GetLength() & ~0xF) != GetLength())
            {
                return (uint)(GetLength() + 0xF & ~0xF);
            }
            return GetLength();
        }

        public uint GetLengthP0x10WithShift()
        {
            if ((GetLength() + GetShift() & ~0xF) != GetLength() + GetShift())
            {
                return (uint)(GetLength() + GetShift() + 0xF & ~0xF);
            }
            return GetLength();
        }

        public void PadFile0x10()
        {
            long currentLength = GetLength();
            long paddedLength = currentLength & ~0xF;
            if (paddedLength != currentLength)
            {
                paddedLength = (currentLength + 0xF) & ~0xF;
                Jump((uint)paddedLength - 1);
                WriteTypeBase<byte>(0);
            }
        }
        public uint GetPosition()
        {
            return (uint)stream.Position;
        }
        public void Jump(uint jump, SeekOrigin seekOrigin = SeekOrigin.Begin)
        {
            stream.Position = seekOrigin == SeekOrigin.Begin ? jump : jump + stream.Position;
        }

        public void MergeData(FFile fFile)
        {
            var pre_len = GetLength();
            WriteByteArray(fFile.GetBytes());
            _flush_offsets.AddRange(fFile._flush_offsets.Select(zx => new KeyValuePair<uint, PointerIWritable>((uint)(zx.Key + pre_len), zx.Value)));
        }

        #endregion


        #region WriteReadAtMacro
        private T ExecWithPosition<T>(uint position, Func<T> action)
        {
            var originalPosition = GetPosition();
            Jump(position);
            T result = action();
            Jump(originalPosition);
            return result;
        }
        private void ExecWithPositionWrite<T>(uint position, Action action)
        {
            var originalPosition = GetPosition();
            Jump(position);
            action();
            Jump(originalPosition);
        }

        #endregion


        #region Readers
        public byte[] ReadBytes(uint count)
        {
            byte[] buffer = new byte[(int)GetLength()];
            stream.ReadExactly(buffer, 0, (int)count);
            return buffer;
        }
        public byte[] ReadBytesAt(uint position, uint count)
        {
            return ExecWithPosition(position, () => ReadBytes(count));
        }
        #endregion


        #region Writers

        #region Common
        public void WriteByteArray(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
        public void WriteType<T>(T obj)
        {
            var _type_info = typeof(T);
            if (_type_info.IsPrimitive)
            {
                WriteTypeBase(obj);

            }
            else if (typeof(WritableObject).IsAssignableFrom(_type_info))
            {
                (obj as WritableObject).AddOffsetShift(GetOffsetShift());
                WriteTypeIWritable(obj as WritableObject);
            }
            else
            {

            }
        }
        public unsafe void WriteTypeBase<T>(T value)
        {
            var t = sizeof(T);
            var bt = new byte[t];
            if (t != null && t > 0)
            {
                T _vl = value;

                var s = (byte*)&_vl;
                for (int i = 0; i < t; i++)
                {
                    bt[i] = *(s + (t - 1 - i));
                }
            }
            WriteByteArray(bt);
        }
        public void WriteTypeIWritable<T>(T obj, PointerIWritablePrm prm = PointerIWritablePrm.None) where T : IWritable
        {
            obj.Write();
            obj.Flush();
            var point_maybe = GetPosition();
            WriteByteArray(obj.GetBytes());
            if (!prm.HasFlag(PointerIWritablePrm.DoNotKeepParentOffsets))
            {
                var pair_flush = obj.GetFlushedOffsets().Select(zx => zx + point_maybe).ToList();
                _flushed_offsets.AddRange(pair_flush);
            }


            Console.WriteLine($"FFILE : SHIFT {obj.GetOffsetShift():X}");
        }

        /// <summary>
        /// Write To File a Pointer Array, Offset->[Offset1->value,Offset2->value,Offset3->value,...], Supports only IWritable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void WriteListOfIWritable<T>(List<T> value) where T : WritableObject
        {
            var p = GetPosition();
            WriteTypeBase<uint>(0);

            FFile new_file = new FFile();

            for (int i = 0; i < value.Count; i++)
            {
                new_file.WritePointerType(value[i]);
            }


            if (value.Count > 0) _flush_offsets.Add(new KeyValuePair<uint, PointerIWritable>(p, new PointerIWritable(new_file)));
        }

        //Try work with that :(
        public void WriteListOfIWritableSinglePointer<T>(List<T> value) where T : WritableObject
        {
            var p = GetPosition();
            var shift = GetShift();
            var len = GetLength();
            WriteTypeBase<uint>(0); // pointer
            FFile new_file = new FFile();
            for (int i = 0; i < value.Count; i++)
            {
                value[i].AddOffsetShift((int)((int)p + shift + len));
                value[i].Write();
                new_file.MergeData(value[i].GetFFile());
            }
            if (value.Count > 0) _flush_offsets.Add(new KeyValuePair<uint, PointerIWritable>(p, new PointerIWritable(new_file)));
        }

        #endregion

        #region At
        public void WriteByteArrayAt(uint position, byte[] bytes)
        {
            ExecWithPositionWrite<byte[]>(position, () => WriteByteArray(bytes));
        }
        public void WriteTypeAt<T>(uint position, T obj)
        {
            ExecWithPositionWrite<T>(position, () => WriteType(obj));
        }
        #endregion

        #region Pointer
        /// <summary>
        /// Write To File a Pointer Object Type, Supports only IWritable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void WritePointerType<T>(T value) where T : IWritable
        {
            var p = GetPosition();
            WriteTypeBase<uint>(0);
            _flush_offsets.Add(new KeyValuePair<uint, PointerIWritable>(p, new PointerIWritable(value)));
        }

        public void WriteArrayType<T>(IEnumerable<T> values) where T : IWritable
        {
            foreach (var v in values)
            {
                v.Write();
                MergeData(v.GetFFile());
            }

        }


        public void WritePointerTypeParam<T>(T value, PointerIWritablePrm prm) where T : IWritable
        {
            var p = GetPosition();
            WriteTypeBase<uint>(0);
            _flush_offsets.Add(new KeyValuePair<uint, PointerIWritable>(p, new PointerIWritable(value, prm)));
        }




        public void FlushOffsets()
        {
            //test
            _flush_offsets=_flush_offsets.OrderBy(pair => pair.Key).ToList();
            //

            var _offsets_after_flush = new List<uint>();
            foreach (var pair in _flush_offsets)
            {
                var point = pair.Key;
                var FreeSpacePoint = GetLength();

                _flushed_offsets.Add(point);
                if (pair.Value.GetIWritable() is FFile file)
                {
                    //  file.SetShift(GetLength());
                }

                int shift_test = shift;

                if (pair.Value.GetParameter().HasFlag(PointerIWritablePrm.None))
                {
                    pair.Value.GetIWritable().AddOffsetShift((int)(GetLength() + GetShift()));

                }
                if (pair.Value.GetParameter().HasFlag(PointerIWritablePrm.ResetShiftThisOffset))
                {
                    shift_test = 0;
                }


                WriteTypeAt(point, (uint)(shift_test + (int)FreeSpacePoint)); Jump(FreeSpacePoint);
                WriteTypeIWritable(pair.Value.GetIWritable(), pair.Value.GetParameter());
            }




            _flush_offsets.Clear();
        }
        #endregion


        #endregion



        public List<uint> GetFlushedOffsets()
        {
            return _flushed_offsets;
        }


        #region ExternalDrive

        /// <summary>
        /// Save To File System
        /// </summary>
        /// <param name="path">Path To Save</param>
        public void WriteToExternalDrive(string path)
        {
            File.WriteAllBytes(path, ReadBytesAt(0, GetLength()));
        }


        #endregion

        #region IWritableImplementation

        /// <summary>
        /// IWritable Interface
        /// </summary>
        public void Write() { }

        public void Flush() => FlushOffsets();

        public void SetOffsetShift(int offset) => SetShift(offset);

        public void AddOffsetShift(int offset) => shift += offset;
        public int GetOffsetShift() => GetShift();

        public byte[] GetBytes() => (byte[])this;

        public FFile GetFFile() => this;

        #endregion

    }
}
