using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MayLib.Utils.Drawing
{
    public class AdditiveBlendDrawManager
    {
        public void CallProjAdditiveDraws()
        {
            //Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile p in Main.projectile)
            {
                var modP = p.ModProjectile;
                if(modP is IAdditiveBlendDraw && p.active)
                    (modP as IAdditiveBlendDraw).AdditiveBlendDraw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void CallNPCAdditiveDraws()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (NPC n in Main.npc)
            {
                var modN = n.ModNPC;
                if (modN is IAdditiveBlendDraw && n.active)
                    (modN as IAdditiveBlendDraw).AdditiveBlendDraw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }

    public interface IAdditiveBlendDraw
    {
        public void AdditiveBlendDraw(SpriteBatch spriteBatch);
    }
}
