using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MayLib.Core.Systems
{
    public class DrawDelegates : ModSystem
    {
        public delegate void AdditiveDraw(SpriteBatch addsb);

        public static List<AdditiveDraw> AdditivePreDrawProj = new();

        public static List<AdditiveDraw> AdditivePostDrawProj = new();

        public static List<AdditiveDraw> AdditiveDrawAbovePlayer = new();

        public static List<Action> DrawAbovePlayer = new();

        public override void Load()
        {
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawNPC += Main_DrawNPC;
            On.Terraria.Main.DrawDust += Main_DrawDust;
        }

        public override void Unload()
        {
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.DrawNPC -= Main_DrawNPC;
            On.Terraria.Main.DrawDust -= Main_DrawDust;

            AdditivePreDrawProj.Clear();
            AdditivePreDrawProj = null;
            AdditivePostDrawProj.Clear();
            AdditivePostDrawProj = null;
            AdditiveDrawAbovePlayer.Clear();
            AdditiveDrawAbovePlayer = null;

            DrawAbovePlayer.Clear();
            DrawAbovePlayer = null;
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (AdditiveDraw func in AdditivePreDrawProj) func(Main.spriteBatch);

            Main.spriteBatch.End();

            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (AdditiveDraw func in AdditivePostDrawProj) func(Main.spriteBatch);

            Main.spriteBatch.End();
        }

        private void Main_DrawNPC(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
        {
            orig(self, iNPCIndex, behindTiles);
        }

        private void Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (AdditiveDraw func in AdditiveDrawAbovePlayer) func(Main.spriteBatch);

            Main.spriteBatch.End();

            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Action func in DrawAbovePlayer) func();

            Main.spriteBatch.End();
        }
    }
}
