using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MayLib.Utils.Content
{
    public static class EntityUtils
    {
        public static void WaveMovement(this Projectile p, float waveTime, float freqMultiplier, float ampMultiplier, bool invert = false)
        {
            p.position += Vector2.Normalize(p.velocity).RotatedBy(MathHelper.PiOver2) * (float)Math.Sin(waveTime * freqMultiplier) * ampMultiplier * (invert ? 1 : -1);
        }

        public static void FaceVelocity(this Projectile p, float offset = MathHelper.PiOver2)
        {
            p.rotation = p.velocity.ToRotation() + offset;
        }

        public static void FacePositionDelta(this Projectile p, float offset = MathHelper.PiOver2)
        {
            p.rotation = (p.position - p.oldPosition).ToRotation() + offset;
        }

        public static void FaceVelocity(this NPC n, float offset = MathHelper.PiOver2)
        {
            n.rotation = n.velocity.ToRotation() + offset;
        }

        public static void FacePositionDelta(this NPC n, float offset = MathHelper.PiOver2)
        {
            n.rotation = (n.position - n.oldPosition).ToRotation() + offset;
        }

        public delegate void OnManaFail();

        public static void TakeMana(this Player player, int mana, OnManaFail onFail)
        {
            if(player.statMana >= mana)
            {
                player.statMana -= mana;
            }
            else if (player.manaFlower && player.QuickMana_GetItemToUse() != null)
            {
                player.QuickMana();
                player.statMana -= mana;
            }
            else
            {
                onFail();
            }
        }

        public static bool TakeMana(this Player player, int mana)
        {
            if (player.statMana >= mana)
            {
                player.statMana -= mana;
            }
            else if (player.manaFlower && player.QuickMana_GetItemToUse() != null)
            {
                player.QuickMana();
                player.statMana -= mana;
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
