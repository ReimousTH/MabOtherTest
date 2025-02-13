using MabOtherTest.Interfaces;
using MabOtherTest.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        Dictionary<IWritable, Dictionary<string,object>> readmarks = new();

        int shift = 0;
        #endregion

        #region Constructor
        public FFile()
        {
            stream = new MemoryStream();
        }
        public void OpenFile(string path)
        {
            stream = new MemoryStream(File.ReadAllBytes(path));
        }
        public void OpenFile(FFile ffile)
        {
            this.stream = ffile.stream;
        }
        public FFile(string path){
            stream = new MemoryStream(File.ReadAllBytes(path)); 
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

        public void Jump(int jump, SeekOrigin seekOrigin = SeekOrigin.Begin)
        {
            stream.Position = seekOrigin == SeekOrigin.Begin ? jump : jump + stream.Position;
        }


        public void MergeData(FFile fFile)
        {
            var pre_len = GetLength();
            WriteByteArray(fFile.GetBytes());
            _flush_offsets.AddRange(fFile._flush_offsets.Select(zx => new KeyValuePair<uint, PointerIWritable>((uint)(zx.Key + pre_len), zx.Value)));
        }

        public void SetMark<T>(IWritable key,string mark_key,T mark_data) where T : struct
        {
            if (readmarks.TryGetValue(key, out var value))
            {
                this.readmarks[key][mark_key] = mark_data;
            }
            else
            {

                this.readmarks.Add(key, new());
                this.readmarks[key][mark_key] = mark_data;
            }     
        }
        public T GetMark<T>(IWritable key,string mark_key) where T : struct
        {
            return (T)this.readmarks[key][mark_key];
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
        private T ExecWithPosition<T>(int position, Func<T> action)
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
            byte[] buffer = new byte[(int)count];
            stream.ReadExactly(buffer, 0, (int)count);
            return buffer;
        }
        public byte[] ReadBytesAt(uint position, uint count)
        {
            return ExecWithPosition(position, () => ReadBytes(count));
        }

        public T ReadBytesAt<T>(uint position)
        {
            return ExecWithPosition<T>(position, () => ReadType<T>());
        }

        public T ReadType<T>()
        {
            var _type_info = typeof(T);
            if (_type_info.IsPrimitive)
            {
                return ReadTypeBase<T>();

            }
            else if (typeof(WritableObject).IsAssignableFrom(_type_info))
            {
                // (obj as WritableObject).AddOffsetShift(GetOffsetShift());
                //   WriteTypeIWritable(obj as WritableObject);
                return (T)ReadTypeIWritable<IWritable>();
            }
            else
            {
                return (T)new Object();
            }
        }
        public unsafe T ReadTypeBase<T>()
        {
            var type = typeof(T);
            var size = sizeof(T);
            var bytes_readed = ReadBytes((uint)size).Reverse().ToArray();
            fixed (byte* conv = bytes_readed)
            {
                return *(T*)conv;
            }    
        }
        public unsafe T ReadTypeBaseAt<T>(uint position)
        {
            return ExecWithPosition<T>(position, () => ReadTypeBase<T>());
        }
        public unsafe T ReadTypeBaseAt<T>(int position)
        {
            return ExecWithPosition<T>(position, () => ReadTypeBase<T>());
        }
        public Y ReadTypeIWritable<T,Y>(uint BaseOffset = 0) where T : IWritable where Y :IWritable
        {
            var type = typeof(T);
            T obj = (T)Activator.CreateInstance(type);
            obj.GetFFile().OpenFile(this);
            obj.GetFFile().SetMark(obj, "Offset", BaseOffset); //:)
            obj.Read<T,Y>();
            return (Y)(obj as IWritable);
        }
        public T ReadTypeIWritable<T>(uint BaseOffset = 0) where T : IWritable
        {
            return ReadTypeIWritable<T,T>(BaseOffset);
        }

        public Y ReadTypePointer<T,Y>(uint BaseOffset, object[] constructor_param = null) where T : IWritable where Y : IWritable
        {
            var type = typeof(T);
            var typeY = typeof(Y);
            var pointer = ReadTypeBase<uint>();
            Y obj = (Y)Activator.CreateInstance(type,constructor_param);
            var or = GetPosition();
            obj.GetFFile().OpenFile(this);
            obj.GetFFile().Jump(BaseOffset + pointer, SeekOrigin.Begin);
            obj.GetFFile().SetMark(obj, "Offset", BaseOffset); //:)
            obj = obj.Read<T,Y>();
            Jump(or, SeekOrigin.Begin);


            return obj;
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

        public Y Read<T,Y>() where T : IWritable where Y :IWritable { return (Y)(this as IWritable); }

        public void OnMarkSet() { }

        public FFile GetFFile() => this;

        //offset exclude baseoffset
        public List<Y> ReadListOfIWriteableAt<T,Y>(uint offset,uint BaseOffset,uint item3, object[] constructor_param = null) where T : IWritable where Y :IWritable
        {
            var p = GetPosition();
            List<Y> list = new List<Y>();
            for (int i = 0;i < item3; i++) {
                uint offset_i = ((uint)((offset + BaseOffset) + (i * 4))); //&offset[table]
                Jump(offset_i);        
                list.Add(ReadTypePointer<T,Y>(BaseOffset,constructor_param));
            }
            Jump(p);
            return list;
        }

        public T Read<T>() where T : IWritable
        {
            return Read<T, T>();
        }

        public List<Y> ReadArrayType<T, Y>(uint BaseOffset, uint item3, object[] constructor_param = null) where T : IWritable where Y : IWritable
        {
            var p = GetPosition();
            List<Y> list = new List<Y>();
            for (int i = 0; i < item3; i++)
            {
                var element = ReadTypeIWritable<T, Y>(BaseOffset);
                list.Add(element);
            }
            Jump(p);
            return list;
        }

        public void ResetRead()
        {
            stream = new MemoryStream();
        }

        #endregion

    }
}
