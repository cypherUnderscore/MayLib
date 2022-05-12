/*using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MayLib.Utils
{
    public static class MainUtils
    {
        public static void PointToVelocity(this Projectile p, float rotationOffset = MathHelper.PiOver2)
        {
            p.rotation = p.velocity.ToRotation() + rotationOffset;
        }

        public static void PointToVelocity(this NPC n, float rotationOffset = MathHelper.PiOver2)
        {
            n.rotation = n.velocity.ToRotation() + rotationOffset;
        }

        /// <summary>
        /// Returns a vector between two points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point</param>
        /// <param name="multiplier">The value to multiply the returned vector by</param>
        /// <param name="normalize">Whether or not the returned vector is normalized or not</param>
        /// <returns></returns>
        public static Vector2 GetVectorTowards(Vector2 point1, Vector2 point2, float multiplier = 1f, bool normalize = true)
        {
            float distToX = point2.X - point1.X;
            float distToY = point2.Y - point1.Y;
            if (normalize) return Vector2.Normalize(new Vector2(distToX, distToY)) * multiplier;
            else return new Vector2(distToX, distToY) * multiplier;
        }

        /// <summary>
        /// Used to limit the velocity of an entity. Returns the limited velocity.
        /// </summary>
        /// <param name="vel">The velocity to limit</param>
        /// <param name="maxSpeed">The maximum speed</param>
        /// <returns></returns>
        public static Vector2 SoftLimitVelocity(this Vector2 vel, float maxSpeed)
        {
            if (vel.Length() > maxSpeed) return vel * (maxSpeed / vel.Length());
            else return vel;
        }

        /// <summary>
        /// Spawns a ring of dust with the desired parameters.
        /// </summary>
        /// <param name="origin">The origin of the ring</param>
        /// <param name="type">The dust ID</param>
        /// <param name="amount">The number of dust particles to spawn</param>
        /// <param name="speed">The outward speed the dust particles have when spawning</param>
        /// <param name="randomised">Whether or not the dusts are positioned randomly or evenly</param>
        /// <param name="radius">The distance the dust particles spawn from the origin point</param>
        /// <param name="scale">The scale of the dust particles</param>
        /// <param name="noLight">noLight dust boolean</param>
        /// <param name="noGravity">noGravity dust boolean</param>
        /// <param name="dustColor">The colour of the dust particles</param>
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

        /// <summary>
        /// Spawns an even spread of projectiles with a designated angle and direction.
        /// </summary>
        /// <param name="source">The projectile source</param>
        /// <param name="position">The origin point for the spread</param>
        /// <param name="direction">The direction the spread should point towards</param>
        /// <param name="type">The projectile ID</param>
        /// <param name="damage">The damage of the projectiles</param>
        /// <param name="knockback">The knockback of the projectiles</param>
        /// <param name="owner">The identity of the projectile's owner</param>
        /// <param name="amount">The number of projectiles in the spread</param>
        /// <param name="spreadDegrees">The angle of the spread</param>
        /// <param name="speedMultiplier">The speed of the projectiles</param>
        /// <param name="ai0"></param>
        /// <param name="ai1"></param>
        public static void SpawnEvenProjectileSpread(IEntitySource source, Vector2 position, Vector2 direction, int type, int damage, float knockback, int owner, int amount, int spreadDegrees, float speedMultiplier = 1, float ai0 = 0, float ai1 = 0)
        {
            for (int p = 0; p < amount; p++)
            {
                Vector2 direction2 = Vector2.Normalize(direction).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-spreadDegrees / 2, spreadDegrees / 2, (float)p / (amount - 1)))) * speedMultiplier;
                Projectile.NewProjectile(source, position, direction2, type, damage, knockback, owner, ai0, ai1);
            }
        }

        /// <summary>
        /// Spawns an even ring of projectiles.
        /// </summary>
        /// <param name="source">The projectile source</param>
        /// <param name="position">The origin point for the ring</param>
        /// <param name="amount">The number of projectiles in the ring</param>
        /// <param name="damage">The damage of the projectiles</param>
        /// <param name="type">The projectile ID</param>
        /// <param name="speed">The speed of the projectiles</param>
        /// <param name="knockback">The knockback of the projectiles</param>
        /// <param name="owner">The identity of the projectile's owner</param>
        /// <param name="ai0"></param>
        /// <param name="ai1"></param>
        /// <param name="rotationOffset">The offset of the projectile ring's rotation</param>
        public static void SpawnProjectileRing(IEntitySource source, Vector2 position, int amount, int damage, int type, float speed, float knockback, int owner, float ai0 = 0, float ai1 = 0, float rotationOffset = 0)
        {
            for (int p = 0; p < amount; p++)
            {
                Vector2 velocity = new Vector2(0, 1).RotatedBy(((p - (amount / 2 - 1)) * 6.28318548f / amount) + MathHelper.ToRadians(rotationOffset)) * speed;
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, owner, ai0, ai1);
            }
        }

        /// <summary>
        /// Returns the closest NPC to a position.
        /// </summary>
        /// <param name="position">The position from which to search from</param>
        /// <returns></returns>
        // todo: make more efficient??? somehow??? 
        public static NPC ClosestNPC(Vector2 position)
        {
            int closestIdentity = 0;
            float closestDistance = 9999;
            float[] distances = new float[200];
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                distances[n] = Vector2.Distance(Main.npc[n].Center, position);
            }
            for (int n = 0; n < distances.Length; n++)
            {
                if (distances[n] < closestDistance && Main.npc[n].active && !Main.npc[n].friendly && !Main.npc[n].immortal && !Main.npc[n].dontTakeDamage)
                {
                    closestDistance = distances[n];
                    closestIdentity = n;
                }
            }
            return Main.npc[closestIdentity];
        }

        //needs testing
        public static NPC ClosestNPC2(Vector2 pos)
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

        public static Color SetA(this Color colour, int newAlpha)
        {
            return new Color(colour.R, colour.G, colour.B, newAlpha);
        }*/

        /*/// <summary>
        /// Spawns generic smoke cloud gore effects.
        /// </summary>
        /// <param name="position">The origin for the gores</param>
        public static void SpawnExplosionGores(Vector2 position)
        {
            for (int i = 0; i > 4; i++)
            {
                Gore g = Main.gore[Gore.NewGore(position, default, Main.rand.Next(61, 64), 1f)];
                g.velocity *= 0.4f;
                g.velocity += new Vector2(1, 1).RotatedBy(MathHelper.PiOver2 * i);
            }
        }*/
        /*
        public static bool CanBeFriendlyHomingTarget(NPC target, Projectile attacker, bool lineOfSight = true) //decides whether or not the specified projectile can home in on the specified NPC
        {
            if (!target.friendly && target.lifeMax > 5 && target.CanBeChasedBy(attacker, false) && target.active && target.life > 0 && (Collision.CanHit(attacker.Center, 1, 1, target.Center, 1, 1) || !lineOfSight)) return true;
            else return false;
        }

        public static void ProjectileFriendlyHoming(this Projectile p, float maxSpeed, float range, float acceleration = 1, bool lineOfSight = true) //extension methods ftw
        {
            if (Vector2.Distance(p.Center, ClosestNPC(p.Center).Center) < range && CanBeFriendlyHomingTarget(ClosestNPC(p.Center), p, lineOfSight))
            {
                Vector2 desiredVelocity = Vector2.Normalize(ClosestNPC(p.Center).Center - p.Center);
                p.velocity += desiredVelocity * acceleration;
                p.velocity = p.velocity.SoftLimitVelocity(maxSpeed);
            }
        }

        public static Player OwnerPlayer(this Projectile p)
        {
            return Main.player[p.owner];
        }

        public static Vector2 VectorInDirection(float length, float rotation)
        {
            return new Vector2(length, 0).RotatedBy(rotation);
        }

        public static T[] ShiftArray<T>(this T[] array, T newFirst)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                array[i] = array[i - 1];
            }

            array[0] = newFirst;

            return array;
        }

        public static T[] FillArray<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }

            return array;
        }
    }
}*/
