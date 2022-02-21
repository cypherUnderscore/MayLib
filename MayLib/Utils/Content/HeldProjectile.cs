using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;

using MayLib.Common.PlayerLayers;
using MayLib.Utils.Maths;
using Terraria.ID;

namespace MayLib.Utils.Content
{
    public abstract class HeldProjectile : ModProjectile
    {
        public enum HoldAnimType
        {
            Shoot = 0
        }

        public override bool CloneNewInstances => true;

        public bool CanBeHeld => Main.player[Projectile.owner].channel && !Main.player[Projectile.owner].noItems && !Main.player[Projectile.owner].CCed;

        public CustomWeaponLayer drawLayer = null;

        public Player owner;

        public Vector2 holdoutOrigin = Vector2.Zero;
        public Vector2 aimNormal;

        public float holdoutOffset = 0;
        public float aimAngle;

        public HoldAnimType holdAnim;

        public int activeTime;
        public int remainTime;
        public int aimDirection;
        public int ammoType;

        public bool heldActive = true;
        public bool firstFrame = true;
        public bool isMelee = false;
        public bool lockAngleAfterRelease = false;

        public sealed override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            SafeSetDefaults();
        }

        public sealed override bool PreAI()
        {
            owner = Main.player[Projectile.owner];

            PreUpdateAI();
            if (firstFrame)
            {
                FirstFrame();
                drawLayer = ModContent.GetInstance<CustomWeaponLayer>();
                Projectile.originalDamage = Projectile.damage;
                if (!isMelee) Projectile.damage = 0;
            }

            if (heldActive || !lockAngleAfterRelease)
            {
                aimNormal = MainUtils.GetVectorTowards(owner.MountedCenter, Main.MouseWorld);
                aimAngle = aimNormal.ToRotation();
                aimDirection = Main.MouseWorld.X > owner.MountedCenter.X ? 1 : -1;
            }

            if (heldActive && CanBeHeld)
            {
                activeTime++;
                owner.itemAnimation = 2;
                owner.itemTime = 2;
                Projectile.timeLeft = 2;
            }
            else if (heldActive)
            {
                heldActive = false;

                Projectile.timeLeft = remainTime;
                owner.itemAnimation = remainTime;
                owner.itemTime = remainTime;

                OnRelease();
            }

            return true;
        }

        public sealed override void AI()
        {
            UpdateAI();

            switch (holdAnim)
            {
                case HoldAnimType.Shoot:
                    Projectile.Center = owner.MountedCenter + holdoutOrigin + aimNormal * holdoutOffset;
                    Projectile.rotation = aimAngle;

                    if (heldActive || !lockAngleAfterRelease)
                    {
                        if (StrictSpriteFlip)
                        {
                            Projectile.direction = aimDirection;
                            Projectile.spriteDirection = aimDirection;
                        }

                        if (StrictPlayerControl)
                        {
                            owner.ChangeDir(aimDirection);
                            owner.itemRotation = HeldProjectileUtils.GetItemRotation(aimNormal, aimDirection);
                        }
                    }
                    break;
            }
        }

        public sealed override void PostAI()
        {
            PostUpdateAi();

            if (firstFrame) firstFrame = false;
        }

        public sealed override bool PreDraw(ref Color lightColor)
        {
            ManageDrawLayer(lightColor);
            return false;
        }

        public void ManageDrawLayer(Color lightColour)
        {
            if (drawLayer == null)
            {
                throw new Exception("Projectile draw layer cannot be null!");
            }

            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            bool draw = PreManageDrawLayers();

            drawLayer.active = true;

            if (!draw) return;

            float visualAimAngle = ModifyVisualAimAngle(aimAngle);

            drawLayer.tex = tex;
            if (GlowmaskTexture != null) drawLayer.glowTex = ModContent.Request<Texture2D>(GlowmaskTexture).Value;

            drawLayer.drawColour = ModifyDrawColour(lightColour);
            drawLayer.glowmaskDrawColour = ModifyGlowColour(Color.White);

            drawLayer.zPos = LayerPosition;
            drawLayer.drawPos = (owner.MountedCenter + holdoutOrigin + new Vector2(ModifyVisualHoldoutOffset(holdoutOffset), 0).RotatedBy(visualAimAngle));
            drawLayer.rotation = visualAimAngle;
            drawLayer.scale = Projectile.scale;
            drawLayer.effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            PostManageDrawLayers();
        }

        public sealed override void PostDraw(Color lightColour)
        {
            if (!Draw(lightColour)) return;
        }

        public sealed override void Kill(int timeLeft)
        {
            drawLayer.active = false;
        }

        public virtual PlayerDrawLayer.Position LayerPosition => new PlayerDrawLayer.Between(PlayerDrawLayers.HeldItem, PlayerDrawLayers.ArmOverItem);

        public virtual string GlowmaskTexture => null;

        public virtual Color ModifyDrawColour(Color originalColour)
        {
            return originalColour;
        }

        public virtual Color ModifyGlowColour(Color originalColour)
        {
            return originalColour;
        }

        public virtual float ModifyVisualAimAngle(float originalAngle)
        {
            return originalAngle;
        }

        public virtual float ModifyVisualHoldoutOffset(float originalOffset)
        {
            return originalOffset;
        }

        public virtual float ModifyVisualScale(float originalScale)
        {
            return originalScale;
        }

        public virtual bool StrictSpriteFlip => true;

        public virtual bool StrictPlayerControl => true;

        public virtual bool Draw(Color lightColour)
        {
            return true;
        }

        public virtual bool PreManageDrawLayers()
        {
            return true;
        }

        public virtual void SafeSetDefaults() { }

        public virtual void FirstFrame() { }

        public virtual void PreUpdateAI() { }

        public virtual void UpdateAI() { }

        public virtual void PostUpdateAi() { }

        public virtual void OnRelease() { }

        public virtual void PostManageDrawLayers() { }
    }

    public static class HeldProjectileUtils
    {
        public static float GetItemRotation(Vector2 normal, int direction)
        {
            return (float)Math.Atan2(normal.Y * direction, normal.X * direction);
        }

        public static void SetItemDefaults(this Item item, int heldProjectileType)
        {
            item.shootSpeed = 0;
            item.shoot = heldProjectileType;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.channel = true;
        }

        public static void SetItemDefaults(this Item item)
        {
            item.shootSpeed = 0;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.channel = true;
        }

        public static void SetAmmoType(this Projectile p, int type)
        {
            (p.ModProjectile as HeldProjectile).ammoType = type;
        }
    }
}
