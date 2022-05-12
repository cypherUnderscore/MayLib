using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MayLib.Utils.Misc
{
    public static class LightingUtils
    {
        public static Color GetLightColourAtPos(Vector2 pos)
        {
            return Lighting.GetColor((int)(pos.X / 16), (int)(pos.Y / 16));
        }

        public static void AddLightFromColour(Vector2 position, Color colour, float brightness = 1)
        {
            Lighting.AddLight(position, (new Vector3(colour.R, colour.B, colour.G) / 255) * brightness);
        }
    }
}
