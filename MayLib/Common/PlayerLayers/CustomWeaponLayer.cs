using MayLib.Utils.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MayLib.Common.PlayerLayers
{
    public class CustomWeaponLayer : PlayerDrawLayer
    {
        public Position zPos;

        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.HeldItem, PlayerDrawLayers.ArmOverItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => active;

        public Texture2D tex;
        public Texture2D glowTex = null;

        public SpriteEffects effects = SpriteEffects.None;

        public Rectangle source;

        public Color drawColour = Color.White;
        public Color glowmaskDrawColour = Color.White;

        public Vector2 drawPos;

        public float rotation = 0;
        public float scale = 1;

        public bool active = false;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            var position = drawPos - Main.screenPosition;
            //position = position.IntCoords();

            drawInfo.DrawDataCache.Add(new DrawData(
                tex,
                position,
                source == default ? null : source,
                drawColour,
                rotation,
                tex.Size() * 0.5f,
                scale,
                effects,
                0
            ));

            if (glowTex != null)
            {
                drawInfo.DrawDataCache.Add(new DrawData(
                    glowTex,
                    position,
                    source == default ? null : source,
                    glowmaskDrawColour,
                    rotation,
                    tex.Size() * 0.5f,
                    scale,
                    effects,
                    0
                ));
            }
        }
    }
}
