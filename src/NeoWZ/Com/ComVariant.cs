﻿using NeoWZ.Serialize;

namespace NeoWZ.Com
{
    public abstract class ComVariant
    {
        public abstract VariantType Type { get; }

        /// <summary>
        /// <para>Convert variant data to <see cref="bool"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="bool"/> value of this variant, return default value when convert failed</returns>
        public virtual bool ToBool(bool def = false) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="byte"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="byte"/> value of this variant, return default value when convert failed</returns>
        public virtual byte ToByte(byte def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="sbyte"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="sbyte"/> value of this variant, return default value when convert failed</returns>
        public virtual sbyte ToSByte(sbyte def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="short"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="short"/> value of this variant, return default value when convert failed</returns>
        public virtual short ToInt16(short def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="ushort"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="ushort"/> value of this variant, return default value when convert failed</returns>
        public virtual ushort ToUInt16(ushort def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="int"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="int"/> value of this variant, return default value when convert failed</returns>
        public virtual int ToInt32(int def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="uint"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="uint"/> value of this variant, return default value when convert failed</returns>
        public virtual uint ToUInt32(uint def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="long"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="long"/> value of this variant, return default value when convert failed</returns>
        public virtual long ToInt64(long def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="ulong"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="ulong"/> value of this variant, return default value when convert failed</returns>
        public virtual ulong ToUInt64(ulong def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="string"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="string"/> value of this variant, return default value when convert failed</returns>
        public virtual string ToText(string def = null) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="float"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="float"/> value of this variant, return default value when convert failed</returns>
        public virtual float ToFloat(float def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="double"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="double"/> value of this variant, return default value when convert failed</returns>
        public virtual double ToDouble(double def = 0) => def;

        /// <summary>
        /// <para>Convert variant data to <see cref="IComSerializable"/></para>
        /// </summary>
        /// <param name="def">defaut return</param>
        /// <returns><see cref="IComSerializable"/> value of this variant, return default value when convert failed</returns>
        public virtual T ToCom<T>(T def = null) where T : class, IComSerializable => def;

        /// <summary>
        /// Convert variant to other derive type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T To<T>() where T : ComVariant => this as T;

        /// <summary>
        /// Check specific variant is equal to current variant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><see langword="true"/> if two variant has same type, name and value; otherwise, <see langword="false"/></returns>
        public abstract bool Equals(ComVariant obj);

        public override bool Equals(object obj) =>
            obj is ComVariant
            ? this.GetType() == obj.GetType()
                && this.Type == (obj as ComVariant).Type
                && this.Equals(obj as ComVariant)
            : base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }
}
