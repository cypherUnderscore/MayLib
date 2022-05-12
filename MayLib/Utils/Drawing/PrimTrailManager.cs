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
    public class PrimTrailManager
    {
        public GraphicsDevice gD;

        public List<PrimTrailBase> trails = new List<PrimTrailBase>();

        public BasicEffect trailBasicEffect;

        public Matrix world = Matrix.CreateTranslation(0, 0, 0);
        public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

        public void Initialize()
        {
            Main.QueueMainThreadAction(() =>
            {
                trailBasicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
            });
            gD = Main.graphics.GraphicsDevice;
        }

        public void DrawAllTrails()
        {
            Main.spriteBatch.Begin();
            foreach (PrimTrailBase trail in trails.ToArray())
            {
                trail.effect = trail.ModifyEffect(trail.effect);
                HandleTrail(trail);
            }
            Main.spriteBatch.End();
        }

        public void AddTrail(PrimTrailBase trail)
        {
            if (!Main.dedServ)
            {
                trail.Initialize();
                trails.Add(trail);
            }
        }

        private void HandleTrail(PrimTrailBase t)
        {
            OldMeshData mesh = CreateMesh(t);
            RenderTrail(t, mesh);
        }

        /*private VertexPositionColorTexture[] CreateMesh(PrimTrailBase trail)
        {
            VertexPositionColorTexture[] tempMesh = new VertexPositionColorTexture[(trail.trailPath.Length * 2) - 1];

            for(int t = 0; t < trail.trailPath.Length - 1; t++)
            {
                float trailProgress = (float)t / trail.trailPath.Length;
                //float inverseProgress = 1 - trailProgress;

                Vector2 toNextPoint = trail.trailPath[t + 1] - trail.trailPath[t];

                Vector2 perpendicularToPoint = Vector2.Normalize(toNextPoint).RotatedBy(-MathHelper.PiOver2);

                for(int v = 0; v < 2; v++)
                {
                    int side = (v == 0) ? 1 : -1;
                    int mIndex = (t * 2) + v;

                    Vector2 vertexPos = trail.trailPath[t] + (perpendicularToPoint * side * MathHelper.Lerp(trail.trailHeadWidth, trail.trailTailWidth, trailProgress));

                    Vector2 vertexTexCoord = new(trailProgress, v);

                    Color vertexColour = Color.Lerp(trail.headColour, trail.tailColour, trailProgress);

                    tempMesh[mIndex] = new VertexPositionColorTexture(new(GetDrawPos(vertexPos), 0), vertexColour, vertexTexCoord);
                }
            }

            tempMesh[^1] = new VertexPositionColorTexture(new(GetDrawPos(trail.trailPath[^1]), 0), trail.tailColour, new(1, 0.5f));

            Vector2 zoom = Main.GameViewMatrix.Zoom;

            for (int i = 0; i < tempMesh.Length; i++)
            {
                tempMesh[i].Position.X *= zoom.X;
                tempMesh[i].Position.Y *= zoom.Y;
            }

            return tempMesh;
        }*/

        private OldMeshData CreateMesh(PrimTrailBase trail)
        {
            VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[(trail.trailPath.Length * 2)];
            List<short> tempIndices = new List<short>();

            int length = trail.trailPath.Length;

            for (int t = 0; t < length; t++)
            {
                float trailProgress = (float)t / (length - 1);
                float width = MathHelper.Lerp(trail.trailHeadWidth, trail.trailTailWidth, trailProgress);

                Vector2 toNextPoint = t == length - 1 ? trail.trailPath[t] - trail.trailPath[t - 1] : trail.trailPath[t + 1] - trail.trailPath[t];
                Vector2 perpendicularToPoint = Vector2.Normalize(toNextPoint).RotatedBy(-MathHelper.PiOver2);
                Vector2 top = trail.trailPath[t] + perpendicularToPoint * width;
                Vector2 bottom = trail.trailPath[t] - (perpendicularToPoint * width);
                Vector2 topTexCoord = new(trailProgress, 0);
                Vector2 bottomTexCoord = new(trailProgress, 1);

                Color vertexColour = Color.Lerp(trail.headColour, trail.tailColour, trailProgress);

                tempVertices[t] = new VertexPositionColorTexture(new Vector3(GetDrawPos(top), 0), vertexColour, topTexCoord);
                tempVertices[t + length] = new VertexPositionColorTexture(new Vector3(GetDrawPos(bottom), 0), vertexColour, bottomTexCoord);
            }

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

            Vector2 zoom = Main.GameViewMatrix.Zoom;

            for (int v = 0; v < tempVertices.Length; v++)
            {
                tempVertices[v].Position.X *= zoom.X;
                tempVertices[v].Position.Y *= zoom.Y;
            }

            return new OldMeshData(tempVertices, tempIndices.ToArray());
        }

        private void RenderTrail(PrimTrailBase t, OldMeshData mesh)
        {
            //t.vertexBuffer.SetData(0, mesh.vertices, 0, mesh.vertices.Length, VertexPositionColorTexture.VertexDeclaration.VertexStride, SetDataOptions.Discard);
            t.vertexBuffer.SetData(mesh._vertices);

            t.indexBuffer.SetData(mesh._indices);

            t.effect.Parameters["transformMatrix"].SetValue(MayLib.primManager.world * MayLib.primManager.view * MayLib.primManager.projection);

            gD.SetVertexBuffer(t.vertexBuffer);
            gD.Indices = t.indexBuffer;

            RasterizerState rs = new RasterizerState
            {
                CullMode = CullMode.None
            };
            gD.RasterizerState = rs;

            foreach (EffectPass p in t.effect.CurrentTechnique.Passes)
            {
                p.Apply();
                gD.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, t.vertexBuffer.VertexCount, 0, t.indexBuffer.IndexCount / 3);
            }
        }

        public static Vector2 GetDrawPos(Vector2 worldPos)
        {
            Vector2 drawPos;
            drawPos.X = worldPos.X - Main.screenPosition.X - (Main.graphics.GraphicsDevice.Viewport.Width / 2);
            drawPos.Y = -(worldPos.Y - Main.screenPosition.Y - (Main.graphics.GraphicsDevice.Viewport.Height / 2));
            return drawPos;
        }

        public struct OldMeshData
        {
            public VertexPositionColorTexture[] _vertices;
            public short[] _indices;

            public OldMeshData(VertexPositionColorTexture[] vertices, short[] indices)
            {
                _vertices = vertices;
                _indices = indices;
            }
        }
    }
}
