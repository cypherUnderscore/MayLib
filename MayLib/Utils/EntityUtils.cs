using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MayLib.Utils;

namespace MayLib.Utils
{
    public static class EntityUtils
    {
        public static Player OwnerPlayer(this Projectile p) => Main.player[p.owner];

        public static Player TargetPlayer(this NPC n) => Main.player[n.target];

        public static void HomeToPosition(this Entity e, Vector2 target, float maxSpeed, float range, float acceleration = 1, bool lineOfSight = true)
        {
            Vector2 normalTo = Vector2.Normalize(target - e.Center);
            if (Vector2.Distance(e.Center, target) <= range && (Collision.CanHit(e.Center, 0, 0, target, 0, 0) || !lineOfSight)) e.velocity = (e.velocity + (normalTo * acceleration)).Limit(maxSpeed);
        }

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

        public static void SpawnEvenProjectileSpread(IEntitySource source, Vector2 position, Vector2 direction, int type, int damage, float knockback, int owner, int amount, int spreadDegrees, float speedMultiplier = 1, float ai0 = 0, float ai1 = 0)
        {
            for (int p = 0; p < amount; p++)
            {
                Vector2 direction2 = Vector2.Normalize(direction).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-spreadDegrees / 2, spreadDegrees / 2, (float)p / (amount - 1)))) * speedMultiplier;
                Projectile.NewProjectile(source, position, direction2, type, damage, knockback, owner, ai0, ai1);
            }
        }

        public static void SpawnProjectileRing(IEntitySource source, Vector2 position, int amount, int damage, int type, float speed, float knockback, int owner, float ai0 = 0, float ai1 = 0, float rotationOffset = 0)
        {
            for (int p = 0; p < amount; p++)
            {
                Vector2 velocity = new Vector2(0, 1).RotatedBy(((p - (amount / 2 - 1)) * 6.28318548f / amount) + MathHelper.ToRadians(rotationOffset)) * speed;
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, owner, ai0, ai1);
            }
        }

        public static void FaceVelocity(this NPC n, float offset = MathHelper.PiOver2)
        {
            n.rotation = n.velocity.ToRotation() + offset;
        }

        public static void FacePositionDelta(this NPC n, float offset = MathHelper.PiOver2)
        {
            n.rotation = (n.position - n.oldPosition).ToRotation() + offset;
        }

        public static NPC ClosestNPC(Vector2 pos)
        {
            float closestDist = 99999;
            NPC closest = Main.npc[0];
            foreach (NPC n in Main.npc)
            {
                float currentDist = Vector2.Distance(pos, n.Center);
                if (currentDist < closestDist && n.active && !n.friendly && !n.immortal && !n.dontTakeDamage)
                    closestDist = currentDist;
                closest = n;
            }
            return closest;
        }

        public static void TakeMana(this Player player, int mana, Action onFail)
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
