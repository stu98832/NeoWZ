﻿using NeoWZ.Serialize.Canvas;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.Shape2D;
using NeoWZ.Serialize.Sound;
using NeoWZ.Serialize.UOL;

namespace NeoWZ.Serialize
{
    internal class WzSerializer : ComSerializer
    {
        public WzSerializer(byte[] iv = null) : base(iv) {
        }

        protected override IComObject GetUnknown(string className) {
            switch (className) {
                case "Property": 
                    return new WzProperty();
                case "Canvas": 
                    return new WzCanvas();
                case "Shape2D#Vector2D": 
                    return new WzVector2D();
                case "Shape2D#Convex2D": 
                    return new WzConvex2D();
                case "UOL": 
                    return new WzUOL();
                case "Sound_DX8": 
                    return new WzSound();
                default:
                    throw new NotSupportedException($"Unknown class {className}");
            }
        }
    }
}
