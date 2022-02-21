using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

using MayLib;
using static MayLib.Utils.Drawing.PrimTrailManager;

namespace MayLib.Utils.Drawing //* means possible error point
{
    public abstract class PrimTrailBase
    {
        /*public PrimTrailBase(Entity owner, Vector2 _drawOffset)
        {
            trailOwner = owner;
            drawOffset = _drawOffset;
        }*/

        public VertexBuffer vertexBuffer;

        public IndexBuffer indexBuffer;

        public Effect effect;

        public int trailLength;

        public float trailHeadWidth;
        public float trailTailWidth;

        public Vector2 headPos;
        public Vector2 drawOffset;

        public Vector2[] trailPath;

        public VertexPositionColorTexture[] vertices;

        public Color headColour;
        public Color tailColour;

        public virtual Effect ModifyEffect(Effect effect)
        {
            return effect;
        }

        public void Initialize()
        {
            trailPath = new Vector2[trailLength];

            vertices = new VertexPositionColorTexture[(trailLength * 2)];

            vertexBuffer = new VertexBuffer(MayLib.primManager.gD, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.None);

            indexBuffer = new IndexBuffer(MayLib.primManager.gD, IndexElementSize.SixteenBits, 6 * (trailLength - 1), BufferUsage.None);
        }

        public void Destroy()
        {
            MayLib.primManager.trails.Remove(this);
        }
    }
}
