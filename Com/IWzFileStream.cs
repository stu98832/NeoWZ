using System;
using System.IO;
using System.Text;

namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public interface IWzFileStream : IDisposable
    {
        /// <summary> </summary>
        Stream BaseStream { get; }

        /// <summary> </summary>
        byte[] IV { get; }

        /// <summary> </summary>
        long BaseOffset { get; set; }

        /// <summary> </summary>
        long Length { get; }

        /// <summary> </summary>
        long Position { get; set; }
        bool DynamicRead { get; }

        /// <summary> </summary>
        void Seek(long offset, bool usebase = false);

        /// <summary> </summary>
        void Skip(long size);

        /// <summary> </summary>
        long Tell(bool usebase = false);

        /// <summary> </summary>
        void Flush();

        /// <summary> </summary>
        byte[] Read(int length, bool decrypt = false);

        /// <summary> </summary>
        void Read(byte[] buffer, int length, bool decrypt = false);

        /// <summary> </summary>
        void Write(byte[] buffer, bool enerypt = false);

        /// <summary> </summary>
        void Write(byte[] buffer, int length, bool encrypt = false);

        /// <summary> </summary>
        bool ReadBool();

        /// <summary> </summary>
        void WriteBool(bool value);

        /// <summary> </summary>
        byte ReadByte();

        /// <summary> </summary>
        void WriteByte(byte value);

        /// <summary> </summary>
        ushort ReadUInt16();

        /// <summary> </summary>
        void WriteUInt16(ushort value);

        /// <summary> </summary>
        uint ReadUInt32();
        /// <summary> </summary>
        /// 
        void WriteUInt32(uint value);

        /// <summary> </summary>
        ulong ReadUInt64();

        /// <summary> </summary>
        void WriteUInt64(ulong value);

        /// <summary> </summary>
        float ReadFloat(bool compressed = false);

        /// <summary> </summary>
        void WriteFloat(float value, bool compressed = false);

        /// <summary> </summary>
        double ReadDouble();

        /// <summary> </summary>
        void WriteDouble(double value);

        /// <summary> </summary>
        sbyte ReadSByte();

        /// <summary> </summary>
        void WriteSByte(sbyte value);

        /// <summary> </summary>
        short ReadInt16();

        /// <summary> </summary>
        void WriteInt16(short value);

        /// <summary> </summary>
        int ReadInt32(bool compressed = false);

        /// <summary> </summary>
        void WriteInt32(int value, bool compressed = false);

        /// <summary> </summary>
        long ReadInt64(bool compressed = false);

        /// <summary> </summary>
        void WriteInt64(long value, bool compressed = false);

        /// <summary> </summary>
        string ReadSerializeString();

        /// <summary> </summary>
        void WriteSerializeString(string str, byte readsign, byte refsign);

        /// <summary> </summary>
        string ReadDirectoryString(out WzPackageItemType type, WzPackageItemType reftype);

        /// <summary> </summary>
        void WriteDirectoryString(string str, WzPackageItemType type, WzPackageItemType reftype);

        void Dispose(bool closeBase);

        /// <summary> </summary>
        string ReadString(int len, Encoding codepage = null, bool decrypt = false);

        void WriteString(string str, Encoding codepage = null, bool encrypt = false);

        void ClearStringPool();

        void WriteDataFromStream(Stream source, long offset, int size);
    }
}
