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
    public static class SpriteDrawingUtils
    {
        public static void DrawMultiframeSprite(Vector2 drawPos, float rotation, float scale, Texture2D sprite, int frameCount, int currentFrame, Color drawColour, SpriteEffects effects = SpriteEffects.None)
        {
            Rectangle frame = new Rectangle(0, (sprite.Height / frameCount) * currentFrame, sprite.Width, sprite.Height / frameCount);
            Vector2 drawOrigin = new Vector2(sprite.Width * 0.5f, (sprite.Height / frameCount) * 0.5f);
            Vector2 pos = drawPos - Main.screenPosition;
            Main.EntitySpriteDraw(sprite, pos, frame, drawColour, rotation, drawOrigin, scale, effects, 0);
        }

        public static void DrawSpriteWithBatch(SpriteBatch spriteBatch, Vector2 drawPos, float rotation, float scale, Texture2D sprite, Color drawColour, SpriteEffects effects = SpriteEffects.None)
        {
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 drawOrigin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Vector2 pos = drawPos - Main.screenPosition;
            spriteBatch.Draw(sprite, pos, frame, drawColour, rotation, drawOrigin, scale, effects, 0);
        }

        public static void DrawSpriteWithBatch(SpriteBatch spriteBatch, Vector2 drawPos, float rotation, Vector2 scale, Texture2D sprite, Color drawColour, SpriteEffects effects = SpriteEffects.None)
        {
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 drawOrigin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Vector2 pos = drawPos - Main.screenPosition;
            spriteBatch.Draw(sprite, pos, frame, drawColour, rotation, drawOrigin, scale, effects, 0);
        }

        public static void DrawSimpleSprite(Vector2 drawPos, float rotation, float scale, Texture2D sprite, Color drawColour, SpriteEffects effects = SpriteEffects.None)
        {
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 drawOrigin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Vector2 pos = drawPos - Main.screenPosition;
            Main.EntitySpriteDraw(sprite, pos, frame, drawColour, rotation, drawOrigin, scale, effects, 0);
        }

        public static void DrawSimpleSprite(Vector2 drawPos, float rotation, Vector2 scale, Texture2D sprite, Color drawColour, SpriteEffects effects = SpriteEffects.None)
        {
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 drawOrigin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Vector2 pos = drawPos - Main.screenPosition;
            Main.EntitySpriteDraw(sprite, pos, frame, drawColour, rotation, drawOrigin, scale, effects, 0);
        }
    }
}
