using System;

namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzBool : WzVariant
    {
        /// <summary> </summary>
        public const ushort True = 0xFFFF;

        /// <summary> </summary>
        public const ushort False = 0x0000;

        /// <summary> </summary>
        public ushort Value { get; set; }

        /// <summary> </summary>
        public WzBool(string name, bool value = false) :
            base(WzVariantType.Boolean, name) {
            this.Value = value ? True : False;
        }

        /// <summary> </summary>
        public override string ToString() {
            return this.Value == True ? "true" :
                   this.Value == False ? "false" :
                   "invalid(" + this.Value + ")";
        }

        /// <summary> </summary>
        public override bool ToBool(bool def = false) {
            ValidateValue();
            return this.Value == True;
        }

        /// <summary> </summary>
        public override sbyte ToSByte(sbyte def = 0) => (sbyte)this.Value;

        /// <summary> </summary>
        public override byte ToByte(byte def = 0) => (byte)this.Value;

        /// <summary> </summary>
        public override short ToShort(short def = 0) => (short)this.Value;

        /// <summary> </summary>
        public override ushort ToUShort(ushort def = 0) => this.Value;

        /// <summary> </summary>
        public override int ToInt(int def = 0) => this.Value;

        /// <summary> </summary>
        public override uint ToUInt(uint def = 0) => this.Value;

        /// <summary> </summary>
        public override long ToLong(long def = 0) => this.Value;

        /// <summary> </summary>
        public override ulong ToULong(ulong def = 0) => this.Value;

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) {
            return this.Value == (obj as WzBool).Value;
        }

        /// <summary> </summary>
        public override WzVariant Clone() {
            WzBool clone = new WzBool(this.Name);
            clone.Value = this.Value;

            return clone;
        }

        internal override void Read(IWzFileStream fs) {
            this.Value = fs.ReadUInt16();
            ValidateValue();
        }

        internal override void Write(IWzFileStream fs) {
            ValidateValue();
            base.Write(fs);
            fs.WriteUInt16(this.Value);
        }

        private void ValidateValue() {
            if (this.Value != True && this.Value != False) {
                throw new ArgumentException();
            }
        }
    }
}
