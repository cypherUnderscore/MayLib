using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MayLib.Core.Graphics.ParticleSystem
{
    public abstract class EffectParticle
    {
        public Vector2 ParentRelativePosition
        {
            get
            {
                if (parent != null)
                    return pos + parent.Center;
                else
                    return pos;
            }
            set
            {
                if (parent != null)
                    pos = value - parent.Center;
                else
                    pos = value;
            }
        }

        public int lifetime = 120;

        public float extra1;
        public float extra2;

        private Vector2 pos;
        public Vector2 velocity;

        public Entity parent;

        public void Update()
        {
            PreUpdate();
            pos += velocity;
            lifetime--;
            if(lifetime <= 0)
            {
                Destroy();
            }
        }

        public virtual void PreUpdate() { }

        public virtual void Draw(SpriteBatch spritebatch) { }

        public virtual void DrawAdditive(SpriteBatch spritebatch) { }

        public virtual void Spawn() { }

        public void Destroy()
        {
            MayLib.particleManager.particles.Remove(this);
        }
    }
}
