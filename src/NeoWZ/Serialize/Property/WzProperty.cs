using NeoWZ.Extensions;
using NeoWZ.Serialize.Attributes;
using System.Collections;

namespace NeoWZ.Serialize.Property
{
    [ComClass("Property")]
    public class WzProperty : WzComBase, IEnumerable<WzVariant>
    {
        private List<WzVariant> mItems = new();

        public ushort Unknown { get; set; }
        public WzVariant this[int index] => this.mItems[index];
        public WzVariant this[string path] {
            get {
                var section = path.Replace('\\', '/').Trim('/').Trim().Split('/', 2);
                if (section[0] == "") {
                    return new WzDispatch(this.Name, this);
                } else if (section[0] == "..") {
                    return this.Parent?.To<WzProperty>()?[section.Length == 1 ? "" : section[1]] ?? WzVariant.Null;
                }
                var item = this.mItems.FirstOrDefault(x => x.Name == section[0], WzVariant.Null);
                return section.Length == 1 ? item : item.ToCom<WzProperty>()?[section[1]] ?? WzVariant.Null;
            }
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteUInt16(this.Unknown);
            stream.WriteCompressedInt32(this.Count);
            foreach (var v in this.mItems) {
                this.WriteVariant(stream, v, serializer);
            }
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            this.mItems.Clear();

            this.Unknown = stream.ReadUInt16();
            var count = stream.ReadCompressedInt32();
            for (int i=0;i<count;++i) {
                var variant = this.ReadVariant(stream, serializer);
                this.mItems.Add(variant);
            }
        }

        public int Count => this.mItems.Count;
        public void Add(WzVariant item) => this.mItems.Add(item);
        public void Clear() => this.mItems.Clear();
        public bool Contains(WzVariant item) => this.mItems.Contains(item);
        public bool Remove(WzVariant item) => this.mItems.Remove(item);

        private WzVariant ReadVariant(WzStream stream, ComSerializer serializer) {
            // VT_TYPE
            var name = stream.StringPool.Read(0, 1);
            var type = stream.ReadByte();

            switch (type) {
                case 0: return new WzEmpty(name);
                case 1: return new WzNull(name);
                case 2: return new WzShort(name, stream.ReadInt16());
                case 3: return new WzInt(name, stream.ReadCompressedInt32());
                case 4: return new WzFloat(name, stream.ReadCompressedFloat());
                case 5: return new WzDouble(name, stream.ReadDouble());
                case 8: return new WzString(name, stream.StringPool.Read(0, 1));
                case 9: {
                    var size = stream.ReadInt32();
                    var off = stream.Position;
                    var serialable = serializer.Deserialize<WzComBase>(stream);

                    serialable.Name = name;
                    serialable.Parent = this;

                    // prevent incomplete deserialize
                    stream.Seek(off + size, SeekOrigin.Begin);

                    return new WzDispatch(name, serialable);
                }
                case 11: {
                    var value = stream.ReadUInt16();
                    if (value != 0xFFFF && value != 0x0000) {
                        throw new InvalidDataException("Invalid VT_BOOL");
                    }
                    return new WzBool(name, value == 0xFFFF);
                }
                case 19: return new WzUInt(name, (uint)stream.ReadCompressedInt32());
                case 20: return new WzLong(name, stream.ReadCompressedInt64());
                default: throw new NotImplementedException($"Variant {type} not implemented");
            }
        }

        private void WriteVariant(WzStream stream, WzVariant variant, ComSerializer serializer) {
            var writeHeader = (byte type) => {
                stream.StringPool.Write(variant.Name, 0, 1);
                stream.WriteByte(type);
            };
            if (variant is WzEmpty) {
                writeHeader(0);
            } else if (variant is WzNull) {
                writeHeader(1);
            } else if (variant is WzShort) {
                writeHeader(2);
                stream.WriteInt16(variant.ToInt16());
            } else if (variant is WzInt) {
                writeHeader(3);
                stream.WriteCompressedInt32(variant.ToInt32());
            } else if (variant is WzFloat) {
                writeHeader(4);
                stream.WriteCompressedFloat(variant.ToFloat());
            } else if (variant is WzDouble) {
                writeHeader(5);
                stream.WriteDouble(variant.ToDouble());
            } else if (variant is WzString) {
                writeHeader(8);
                stream.StringPool.Write(variant.ToText(), 0, 1);
            } else if (variant is WzDispatch) {
                writeHeader(9);
                stream.WriteInt32(0);
                var off = stream.Base.Position;
                serializer.Serialize(stream, variant.ToCom<WzComBase>());
                var size = stream.Base.Position - off;
                stream.Base.Seek(off - 4, SeekOrigin.Begin);
                stream.WriteInt32((int)size);
                stream.Base.Seek(off + size, SeekOrigin.Begin);
            } else if (variant is WzBool) {
                writeHeader(11);
                stream.WriteUInt16(variant.ToBool() ? (ushort)0xFFFF : (ushort)0x0000);
            } else if (variant is WzUInt) {
                writeHeader(19);
                stream.WriteCompressedInt32(variant.ToInt32());
            } else if (variant is WzLong) {
                writeHeader(20);
                stream.WriteCompressedInt64(variant.ToInt64());
            }
        }

        // WzSerializable
        public override WzComBase Clone() {
            var prop = new WzProperty() { Name = this.Name };

            prop.Unknown = this.Unknown;

            foreach (WzVariant v in this.mItems) {
                prop.Add(v.Clone());
            }

            return prop;
        }

        // IEnumerable
        public IEnumerator<WzVariant> GetEnumerator() => this.mItems.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.mItems.GetEnumerator();
    }
}
