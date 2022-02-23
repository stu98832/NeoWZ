﻿namespace NeoMS.Wz.Com.Variant
{
    // 原始資料結構為 Union VARIANT; 含在Wtype.h檔中
    // 這邊另外實做一個

    /// <summary> Variant for wz, corresponding to VARIANT in oaidl.h </summary>
    public abstract class WzVariant
    {
        /// <summary> </summary>
        public readonly static WzVariant Null = new WzNull("");

        /// <summary> </summary>
        public readonly static WzVariant Empty = new WzEmpty("");

        /// <summary> </summary>
        public virtual IWzProperty Parent {
            get => this.mParent;
            set => this.mParent = value;
        }

        /// <summary> </summary>
        public string Name { get; set; }

        /// <summary> </summary>
        public WzVariantType Type { get; private set; }

        /// <summary> </summary>
        public WzVariant(WzVariantType type, string name) {
            this.Type = type;
            this.Name = name;
            this.Parent = null;
        }

        /// <summary> </summary>
        public string GetImagePath() {
            if (this.Parent == null) {
                return this.Name;
            }

            string prop_path = this.Parent.GetImagePath();

            return (prop_path == "" ? "" : (prop_path + "/")) + this.Name;
        }

        /// <summary> </summary>
        public abstract bool Equals(WzVariant obj);

        /// <summary> </summary>
        public abstract WzVariant Clone();

        /// <summary> </summary>
        public virtual bool ToBool(bool def = false) => def;

        /// <summary> </summary>
        public virtual sbyte ToSByte(sbyte def = 0) => def;

        /// <summary> </summary>
        public virtual byte ToByte(byte def = 0) => def;

        /// <summary> </summary>
        public virtual short ToShort(short def = 0) => def;

        /// <summary> </summary>
        public virtual ushort ToUShort(ushort def = 0) => def;

        /// <summary> </summary>
        public virtual int ToInt(int def = 0) => def;

        /// <summary> </summary>
        public virtual uint ToUInt(uint def = 0) => def;

        /// <summary> </summary>
        public virtual long ToLong(long def = 0) => def;

        /// <summary> </summary>
        public virtual ulong ToULong(ulong def = 0) => def;

        /// <summary> </summary>
        public virtual float ToFloat(float def = 0) => def;

        /// <summary> </summary>
        public virtual double ToDouble(double def = 0) => def;

        /// <summary> </summary>
        public virtual IWzSerialize ToDispatch(IWzSerialize def = null) => def;

        /// <summary> </summary>
        public override string ToString() => string.Format("{0}[{1}]", this.Type.ToString(), this.GetHashCode());

        /// <summary> 判斷指定的 <see cref="object"/> 和目前的 <see cref="object"/> 是否相等。 </summary>
        /// <param name="obj"> <see cref="object"/>，要與目前的 <see cref="object"/> 比較。 </param>
        public override bool Equals(object obj) {
            if (obj is WzVariant && this.Type == (obj as WzVariant).Type) {
                return this.Equals((WzVariant)obj);
            }
            return base.Equals(obj);
        }

        /// <summary> </summary>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary> </summary>
        public static bool operator ==(WzVariant left, WzVariant right)
            => object.Equals(left, right) || left.Equals((object)right);

        /// <summary> </summary>
        public static bool operator !=(WzVariant left, WzVariant right)
            => !object.Equals(left, right) || !left.Equals((object)right);

        internal virtual void Read(IWzFileStream fs) {
        }

        internal virtual void Write(IWzFileStream stream) {
            stream.WriteSerializeString(this.Name, 0, 1);
            stream.WriteByte((byte)this.Type);
        }

        /// <summary> </summary>
        public static WzVariant Create(string name, WzVariantType type) {
            switch (type) {
                case WzVariantType.Empty:
                    return new WzEmpty(name);
                case WzVariantType.Null:
                    return new WzNull(name);
                case WzVariantType.Short:
                    return new WzShort(name);
                case WzVariantType.Int:
                    return new WzInt(name);
                case WzVariantType.Float:
                    return new WzFloat(name);
                case WzVariantType.Double:
                    return new WzDouble(name);
                case WzVariantType.String:
                    return new WzString(name);
                case WzVariantType.Boolean: // (0xFFFF = 1, 0x0000 = 0)
                    return new WzBool(name);
                case WzVariantType.UInt:
                    return new WzUInt(name);
                case WzVariantType.Long:
                    return new WzLong(name);
                case WzVariantType.Dispatch:
                    return new WzDispatch(name);
                default:
                    return null;
            }
        }

        /// <summary> </summary>
        protected IWzProperty mParent;
    }
}
