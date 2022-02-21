using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MayLib.Utils.Maths
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
    }
}
