using NeoMS.Wz.Utils;
using System;
using System.Text;

namespace NeoMS.Wz.Com
{
    /// <summary> wzLua腳本 </summary>
    public class WzLua : IDisposable
    {
        /// <summary> 腳本名稱 </summary>
        public string Name { get; set; }

        /// <summary> 腳本內容 </summary>
        public string Script { get; set; }

        /// <summary> 從指定資料流讀取腳本資料 </summary>
        public bool Read(IWzFileStream stream)
        {
            stream.Seek(0, true);
            byte flag = stream.ReadByte();
            switch (flag)
            {
                case 1:
                    int len = stream.ReadInt32(true);
                    this.Script = stream.ReadString(len, Encoding.UTF8, true);
                    break;
                default:
                    throw new NotSupportedException("Not supported flag : " + flag);
            }
            return true;
        }

        /// <summary> 將腳本資料寫入指定資料流 </summary>
        public bool Write(IWzFileStream stream)
        {
            byte[] data = Encoding.UTF8.GetBytes(this.Script);

            stream.WriteByte(1);
            stream.WriteInt32(data.Length, true);
            stream.Write(data, data.Length, true);
            return true;
        }

        /// <summary> 釋放<see cref="WzLua"/>所使用的資源 </summary>
        public void Dispose()
        {
            this.Name = null;
            this.Script = null;

            GC.SuppressFinalize(this);

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        /// <summary> 使用指定的加密金鑰，將加密過的<see cref="WzLua"/>資料寫入<see cref="WzArchive"/>中 </summary>
        /// <param name="name"> <see cref="WzArchive"/>的名子 </param>
        /// <param name="key"> 加密金鑰 </param>
        public WzArchive ToWzFile(string name, byte[] iv = null)
        {
            WzArchive file = new WzArchive(name, iv);

            WzFileStream stream = new WzFileStream(file.Stream, iv);

            this.Write(stream);
            file.Size = (int)stream.Length;
            stream.Dispose(false);

            return file;
        }

        /// <summary> 從<see cref="WzArchive"/>中讀取資料並建立<see cref="WzLua"/>實體 </summary>
        public static WzLua FromWzFile(IWzArchive file)
        {
            WzLua lua = new WzLua();

            WzFileStream fs = new WzFileStream(file.Stream, WzDefaultKey.KMSOld);
            fs.BaseOffset = file.Offset;
            lua.Name = file.Name;
            lua.Read(fs);

            return lua;
        }
    }
}
