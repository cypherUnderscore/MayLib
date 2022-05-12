using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;


namespace MayLib.Core.Graphics.Primitives
{
    public static class MeshGenerator
    {
        public static MeshData FixMeshZoom(this MeshData mesh)
        {
            Vector2 zoom = Main.GameViewMatrix.Zoom;

            for (int v = 0; v < mesh._vertices.Length; v++)
            {
                mesh._vertices[v].Position.X *= zoom.X;
                mesh._vertices[v].Position.Y *= zoom.Y;
            }
            return mesh;
        }

        public static Vector2 WorldToScreenPos(this Vector2 worldPos)
        {
            Vector2 drawPos;
            drawPos.X = worldPos.X - Main.screenPosition.X - (Main.graphics.GraphicsDevice.Viewport.Width / 2);
            drawPos.Y = -(worldPos.Y - Main.screenPosition.Y - (Main.graphics.GraphicsDevice.Viewport.Height / 2));
            return drawPos;
        }

        #region TaperedTrail
        public delegate float TTWidthFunc(float progress);
        public delegate Color TTColourFunc(float progress);

        private static MeshData GenerateTaperedTrailMesh(TaperedTrailData trail)
        {
            VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[(trail.path.Length * 2)];
            List<short> tempIndices = new List<short>();

            int length = trail.path.Length;

            for (int t = 0; t < length; t++)
            {
                float trailProgress = (float)t / (length - 1);
                //float width = MathHelper.Lerp(trail.trailHeadWidth, trail.trailTailWidth, trailProgress);
                float width = trail.widthMultFunction(trailProgress) * trail.baseWidth;

                Vector2 toNextPoint = t == length - 1 ? trail.path[t] - trail.path[t - 1] : trail.path[t + 1] - trail.path[t];
                Vector2 perpendicularToPoint = Vector2.Normalize(toNextPoint).RotatedBy(-MathHelper.PiOver2);
                Vector2 top = trail.path[t] + perpendicularToPoint * width;
                Vector2 bottom = trail.path[t] - (perpendicularToPoint * width);
                Vector2 topTexCoord = new(trailProgress, 0);
                Vector2 bottomTexCoord = new(trailProgress, 1);

                //Color vertexColour = Color.Lerp(trail.headColour, trail.tailColour, trailProgress);
                Color vertexColour = trail.colourFunction(trailProgress);
                tempVertices[t] = new VertexPositionColorTexture(new Vector3(top.WorldToScreenPos(), 0), vertexColour, topTexCoord);
                tempVertices[t + length] = new VertexPositionColorTexture(new Vector3(bottom.WorldToScreenPos(), 0), vertexColour, bottomTexCoord);
                Main.NewText(width);
                Main.NewText(vertexColour);
            }

            Main.NewText(length);

            for (short i = 0; i < length - 1; i++)
            {
                short[] segmentIndices = new short[]
                {
                    i,
                    (short)(i + length),
                    (short)(i + length + 1),

                    (short)(i + length + 1),
                    (short)(i + 1),
                    i
                };

                tempIndices.AddRange(segmentIndices);
            }

            /*Vector2 zoom = Main.GameViewMatrix.Zoom;

            for (int v = 0; v < tempVertices.Length; v++)
            {
                tempVertices[v].Position.X *= zoom.X;
                tempVertices[v].Position.Y *= zoom.Y;
            }*/

            Main.NewText(tempVertices.Length);
            Main.NewText(tempIndices.ToArray().Length);
            Main.NewText(trail.path[0].WorldToScreenPos());

            return new MeshData(PrimitiveType.TriangleList, tempVertices, tempIndices.ToArray(), trail.effect).FixMeshZoom();
        }

        public struct TaperedTrailData
        {
            public float baseWidth;

            public Vector2[] path;

            public TTWidthFunc widthMultFunction;
            public TTColourFunc colourFunction;

            public Effect effect;
        }

        public static void ProcessAndQueue(this TaperedTrailData data)
        {
            MayLib.primRenderer.AddToQueue(GenerateTaperedTrailMesh(data));
        }
        #endregion
    }
}
