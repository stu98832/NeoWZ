using NeoWZ.Serialize;
using NeoWZ.Serialize.Canvas;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.Property.Variant;
using NeoWZ.Serialize.Shape2D;
using NeoWZ.Serialize.Sound;
using NeoWZ.Serialize.UOL;
using System.Text.Json;

namespace NeoWZ.Json
{
    /// <summary> Provide methods for <see cref="WzComBase"/> to convert to json. </summary>
    public static class WzJsonExtension
    {
        /// <summary> convert <seealso cref="WzComBase"/> to json string without image/sound data </summary>
        public static string ToJson(this WzComBase com) {
            return JsonSerializer.Serialize(JsonObject(com), new JsonSerializerOptions {
                WriteIndented = true
            });
        }

        private static object JsonObject(WzComBase com) {
            if (com is WzProperty) {
                return JsonObject(com as WzProperty);
            } else if (com is WzVector2D) {
                return JsonObject(com as WzVector2D);
            } else if (com is WzConvex2D) {
                return JsonObject(com as WzConvex2D);
            } else if (com is WzUOL) {
                return JsonObject(com as WzUOL);
            } else if (com is WzCanvas) {
                return JsonObject(com as WzCanvas);
            } else if (com is WzSound) {
                return JsonObject(com as WzSound);
            }
            return null;
        }

        private static object JsonObject(WzProperty property) {
            var dict = new Dictionary<string, object>();
            foreach (var variant in property) {
                dict.Add(variant.Name, JsonObject(variant));
            }
            return dict;
        }

        private static object JsonObject(WzVariant variant) {
            if (variant is WzBool) {
                return (variant as WzBool).Value;
            } else if (variant is WzShort) {
                return (variant as WzShort).Value;
            } else if (variant is WzInt) {
                return  (variant as WzInt).Value;
            } else if (variant is WzUInt) {
                return (variant as WzUInt).Value;
            } else if (variant is WzLong) {
                return (variant as WzLong).Value;
            } else if (variant is WzFloat) {
                return (variant as WzFloat).Value;
            } else if (variant is WzDouble) {
                return (variant as WzDouble).Value;
            } else if (variant is WzString) {
                return (variant as WzString).Value;
            } else if (variant is WzDispatch) {
                return JsonObject((variant as WzDispatch).Value);
            } else if (variant is WzNull) {
                return null;
            } else if (variant is WzEmpty) {
                return null;
            }
            throw new ArgumentException("inconvertible variant");
        }

        private static object JsonObject(WzVector2D vector) {
            return new {
                x = vector.X,
                y = vector.Y
            };
        }

        private static object JsonObject(WzConvex2D convex) {
            return convex.Select(v => new { 
                x = v.X, 
                y = v.Y 
            });
        }

        private static object JsonObject(WzUOL uol) {
            return new {
                path = uol.Path
            };
        }

        private static object JsonObject(WzCanvas canvas) {
            return new {
                height = canvas.Height,
                width = canvas.Width,
                scale = canvas.Scale,
                format = canvas.Format,
                properties = JsonObject(canvas.Property)
            };
        }

        private static object JsonObject(WzSound sound) {
            return new {
                duration = sound.Duration
            };
        }
    }
}
