using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MayLib.Utils.Drawing
{
    public class EffectParticleManager
    {
        public List<EffectParticle> particles = new List<EffectParticle>();
        public void DrawParticles()
        {
            UpdateParticles();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (EffectParticle p in particles.ToArray())
            {
                p.Draw(Main.spriteBatch);
            }
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (EffectParticle p in particles.ToArray())
            {
                p.DrawAdditive(Main.spriteBatch);
            }
            Main.spriteBatch.End();
        }

        private void UpdateParticles()
        {
            foreach (EffectParticle p in particles.ToArray())
            {
                p.Update();
            }
        }

        public EffectParticle NewParticle(EffectParticle p, Vector2 position, Vector2 velocity, int lifetime, Entity parent = null, float extra1 = 0, float extra2 = 0)
        {
            EffectParticle particle = CreateParticle(p, position, velocity, lifetime, parent, extra1, extra2);
            particles.Add(particle);
            particle.Spawn();

            return particle;
        }

        public EffectParticle CreateParticle(EffectParticle p, Vector2 position, Vector2 velocity, int lifetime, Entity parent = null, float extra1 = 0, float extra2 = 0)
        {
            p.ParentRelativePosition = position;
            p.velocity = velocity;
            p.parent = parent;
            p.lifetime = lifetime;
            p.extra1 = extra1;
            p.extra2 = extra2;
            return p;
        }

        public void AddParticle(EffectParticle p)
        {
            particles.Add(p);
        }
    }
}
