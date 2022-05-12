using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using MayLib.Utils;
	
namespace MayLib.Utils.Content
{
	public abstract class DynamicHoldoutAnimDEPRECATED : ModProjectile
	{
		/// <summary>
		/// The time (in ticks) that the weapon has been actively held for.
		/// </summary>
		public int timeHeld = 0;

		/// <summary>
		/// The item's damage, set in the item's ModItem class. Used to avoid ranged weapons dealing damage from the weapon itself rather than the projectile.
		/// </summary>
		public int shotDmg = 0;

		/// <summary>
		/// The time for the weapon to remain held out after the player releases the attack button.
		/// </summary>
		public int remainTime = 0;

		/// <summary>
		/// Whether or not the item acts as a melee weapon.
		/// </summary>
		public bool isMelee = false;

		/// <summary>
		/// Whether or not the weapon is being held actively. True until the player releases the attack button, and will remain false until the remainTime has elapsed, at which point the projectile is killed.
		/// </summary>
		public bool heldActive = true;

		/// <summary>
		/// Allows you to set all of your projectile's properties. (safe version to avoid overriding dynamic holdout defaults)
		/// </summary>	
		public virtual void SafeSetDefaults() { }

		/// <summary>
		/// Allows you to determine how this projectile behaves (safe version to avoid overriding dynamic holdout behaviour)
		/// </summary>	
		public virtual void SafeAI() { }

		/// <summary>
		/// Called once when the player releases the attack button. Use this to do things such as fire a projectile or add other special behaviour.
		/// </summary>
		public virtual void OnRelease() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ignoreWater = true;
		}

		public sealed override void AI()
		{
			if (timeHeld == 0 && !isMelee)
			{
				shotDmg = Projectile.damage;
				Projectile.damage = 0;
			}
			SafeAI();
			Player projOwner = Main.player[Projectile.owner];
			Vector2 pRRP = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.position = projOwner.MountedCenter;
			float projDirRot;
			Vector2 mouseDir = Vector2.Normalize(Main.MouseWorld - pRRP);
			Projectile.spriteDirection = Projectile.direction;
			if (Projectile.spriteDirection == 1) projDirRot = 0;
			else projDirRot = MathHelper.Pi;
			Projectile.velocity = mouseDir * 20f;
			Projectile.position = pRRP - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + projDirRot;
			projOwner.ChangeDir(Projectile.direction);
			projOwner.heldProj = Projectile.whoAmI;
			projOwner.itemRotation = (float)Math.Atan2((Projectile.velocity.Y * Projectile.direction), (Projectile.velocity.X * Projectile.direction));
			if (projOwner.channel && !projOwner.noItems && !projOwner.CCed && heldActive) //while held out
			{
				heldActive = true;
				timeHeld++;
				projOwner.itemTime = 2;
				projOwner.itemAnimation = 2;
				Projectile.timeLeft = 3;
			}
			else //while not held out
			{
				if (heldActive)
				{
					heldActive = false;
					projOwner.itemTime = remainTime;
					projOwner.itemAnimation = remainTime;
					Projectile.timeLeft = remainTime;
					OnRelease();
				}
			}
		}

		public void ForceRelease()
		{
			if (!heldActive) return;
			Player projOwner = Main.player[Projectile.owner];
			heldActive = false;
			projOwner.itemTime = remainTime;
			projOwner.itemAnimation = remainTime;
			Projectile.timeLeft = remainTime;
			OnRelease();
		}

		public Vector2 AimDirection()
		{
			Player projOwner = Main.player[Projectile.owner];
			return MathUtils.GetNormalTowards(projOwner.MountedCenter, Main.MouseWorld);
		}

		public static void DynamicHoldoutItemDefaults(ModItem item)
		{
			item.Item.channel = true;
			item.Item.noMelee = true;
			item.Item.autoReuse = false;
			item.Item.noUseGraphic = true;
		}
	}
}
