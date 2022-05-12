using Microsoft.Xna.Framework;
using MayLib.Utils.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;

namespace MayLib.Utils.Debug
{
    public static class DebugUtils
    {
        public static void DrawDebugPixel(Vector2 position)
        {
            DrawingUtils.DrawSimpleSprite(position + Main.screenPosition, 0, 1, ModContent.Request<Texture2D>("MayLib/Assets/Textures/DebugPixel").Value, Color.White);
        }
    }
}
