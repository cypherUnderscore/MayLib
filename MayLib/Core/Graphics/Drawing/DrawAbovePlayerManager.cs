using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MayLib.Core.Graphics.Drawing
{
    public class DrawAbovePlayerManager
    {
        public void CallProjectileDraws()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile p in Main.projectile)
            {
                var modP = p.ModProjectile;

                if (modP is IDrawAbovePlayer && p.active)
                    (modP as IDrawAbovePlayer).DrawAbovePlayer(Main.spriteBatch);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile p in Main.projectile)
            {
                var modP = p.ModProjectile;

                if (modP is IDrawAdditiveAbovePlayer && p.active)
                    (modP as IDrawAdditiveAbovePlayer).DrawAdditiveAbovePlayer(Main.spriteBatch);
            }

            Main.spriteBatch.End();
        }
    }

    public interface IDrawAbovePlayer
    {
        public void DrawAbovePlayer(SpriteBatch sb) { }
    }

    public interface IDrawAdditiveAbovePlayer
    {
        public void DrawAdditiveAbovePlayer(SpriteBatch sb) { }
    }
}
