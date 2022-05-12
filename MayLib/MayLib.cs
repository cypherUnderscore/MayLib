using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using MayLib.Utils.Drawing;
using System.Collections.Generic;
using System;
using MayLib.Core.Graphics.Primitives;
using MayLib.Core.Graphics.ParticleSystem;
using MayLib.Core.Graphics.Drawing;

namespace MayLib
{
	public class MayLib : Mod
	{
        internal static MayLib instance;

        public static PrimTrailManager primManager;

        public static MeshRenderer primRenderer;

        //public static AnimatedEffectManager effectManager;

        public static EffectParticleManager particleManager;

        public static AdditiveBlendDrawManager additiveManager;

        public static DrawAbovePlayerManager abovePlayerManager;

        public static List<Action> UpdatePrimTrails = new();

        public MayLib()
        {
            instance = this;
        }

        public override void Load()
        {
            On.Terraria.Main.DrawProjectiles += Main_OnDrawProjs;
            On.Terraria.Main.DrawNPCs += Main_OnDrawNPCs;
            On.Terraria.Main.DrawDust += Main_DrawDust;

            primManager = new PrimTrailManager();
            particleManager = new EffectParticleManager();
            additiveManager = new AdditiveBlendDrawManager();
            abovePlayerManager = new DrawAbovePlayerManager();

            primRenderer = new MeshRenderer();

            primManager.Initialize();
            primRenderer.Initialize();
        }

        public override void Unload()
        {
            On.Terraria.Main.DrawProjectiles -= Main_OnDrawProjs;
            On.Terraria.Main.DrawNPCs -= Main_OnDrawNPCs;
            On.Terraria.Main.DrawDust -= Main_DrawDust;

            instance = null;

            primManager = null;
            additiveManager = null;
            abovePlayerManager = null;

            primRenderer = null;
        }

        private void Main_OnDrawProjs(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            foreach (Action func in UpdatePrimTrails) func?.Invoke();

            if(primManager != null) primManager.DrawAllTrails();
            additiveManager.CallProjAdditiveDraws();

            if (primRenderer != null) primRenderer.RenderAll();
            orig(self);
        }

        private void Main_OnDrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            additiveManager.CallNPCAdditiveDraws();
            orig(self, behindTiles);
        }

        private void Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
        {
            abovePlayerManager.CallProjectileDraws();
            particleManager.DrawParticles();
            orig(self);
        }
    }
}