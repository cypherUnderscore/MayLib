using MayLib.Utils.Debug;
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
    public class MeshRenderer
    {
        public List<MeshData> queue = new List<MeshData>();

        public GraphicsDevice gD;

        public Matrix world = Matrix.CreateTranslation(0, 0, 0);
        public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

        public void Initialize()
        {
            gD = Main.graphics.GraphicsDevice;
        }

        public void RenderAll()
        {
            Main.spriteBatch.Begin();
            foreach (MeshData mesh in queue.ToArray())
            {
                DynamicVertexBuffer vB = new DynamicVertexBuffer(gD, typeof(VertexPositionColorTexture), mesh._vertices.Length, BufferUsage.None);
                DynamicIndexBuffer iB = new DynamicIndexBuffer(gD, IndexElementSize.SixteenBits, mesh._indices.Length, BufferUsage.None);

                vB.SetData(mesh._vertices);
                iB.SetData(mesh._indices);

                mesh._effect.Parameters["transformMatrix"].SetValue(world * view * projection);

                gD.SetVertexBuffer(vB);
                gD.Indices = iB;

                RasterizerState rs = new RasterizerState
                {
                    CullMode = CullMode.None
                };
                gD.RasterizerState = rs;

                foreach (EffectPass p in mesh._effect.CurrentTechnique.Passes)
                {
                    p.Apply();
                    gD.DrawIndexedPrimitives(mesh.meshType, 0, 0, vB.VertexCount, 0, iB.IndexCount / 3);
                }

                foreach(var v in mesh._vertices)
                {
                    DebugUtils.DrawDebugPixel(new Vector2(v.Position.X, v.Position.Y));
                }
            }

            Main.spriteBatch.End();
            queue.Clear();
        }

        public void AddToQueue(MeshData mesh)
        {
            queue.Add(mesh);
        }
    }

    public struct MeshData
    {
        public VertexPositionColorTexture[] _vertices;
        public short[] _indices;

        public PrimitiveType meshType;

        public Effect _effect;

        public MeshData(PrimitiveType type, VertexPositionColorTexture[] vertices, short[] indices, Effect effect)
        {
            _vertices = vertices;
            _indices = indices;
            meshType = type;
            _effect = effect;
        }
    }
}
