using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MayLib.Utils
{
    public static class DustUtils
    {
        public static Dust[] SpawnDustRing(Vector2 origin, int type, int amount, float speed, bool randomised = false, float radius = 0f, float scale = 1f, bool noLight = false, bool noGravity = false, Color dustColor = default) //creates a ring of evenly spaced dust
        {
            Dust[] dusts = new Dust[amount];
            int numDusts = amount;
            for (int k = 0; k < numDusts; k++)
            {
                Vector2 velocity;
                if (randomised) velocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(0, 6.28318548f));
                else velocity = new Vector2(0, 1).RotatedBy((int)(k - (numDusts / 2 - 1)) * 6.28318548f / numDusts);
                int d = Dust.NewDust(origin, 0, 0, type, velocity.X, velocity.Y, 0, dustColor, scale);
                Main.dust[d].noGravity = noGravity;
                Main.dust[d].noLight = noLight;
                Main.dust[d].position = origin + (Vector2.Normalize(velocity) * radius);
                Main.dust[d].velocity = Vector2.Normalize(velocity) * speed;
                dusts[k] = Main.dust[d];
            }
            return dusts;
        }
    }
}
