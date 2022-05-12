using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MayLib.Utils
{
    public static class MathUtils
    {
        public static Vector2 IntCoords(this Vector2 vector)
        {
            return new Vector2((int)vector.X, (int)vector.Y);
        }

        public static Vector2 TileCoords(this Vector2 vector)
        {
            return vector / 16;
        }

        public static Vector2 Limit(this Vector2 v, float max)
        {
            Vector2 newV = v;
            if (v.Length() > max) newV = Vector2.Normalize(v) * max;
            return newV;
        }

        public static Vector2 GetNormalTowards(Vector2 point1, Vector2 point2)
        {
            return Vector2.Normalize(point2 - point1);        
        }
    }
}
