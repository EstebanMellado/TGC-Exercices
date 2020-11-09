using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Exam
{
    public class Quad
    {
        private VertexBuffer VertexBuffer { get; set; }
        private IndexBuffer IndexBuffer { get; set; }

        private BasicEffect Effect { get; set; }

        /// <summary>
        ///     Create a quad used in clip space
        /// </summary>
        /// <param name="device">Used to initialize and control the presentation of the graphics device.</param>
        public Quad(GraphicsDevice device)
        {
            CreateVertexBuffer(device);
            CreateIndexBuffer(device);

            // Create a BasicEffect, which will be used to render the primitive.
            Effect = new BasicEffect(device);
            Effect.VertexColorEnabled = false;
            Effect.TextureEnabled = true;
        }

        private void CreateVertexBuffer(GraphicsDevice device)
        {
            var vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(-1f, 0f, -1f);
            vertices[0].TextureCoordinate = new Vector2(0f, 1f);
            vertices[1].Position = new Vector3(-1f, 0f, 1f);
            vertices[1].TextureCoordinate = new Vector2(0f, 0f);
            vertices[2].Position = new Vector3(1f, 0f, -1f);
            vertices[2].TextureCoordinate = new Vector2(1f, 1f);
            vertices[3].Position = new Vector3(1f, 0f, 1f);
            vertices[3].TextureCoordinate = new Vector2(1f, 0f);

            VertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, 4,
                BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice device)
        {
            var indices = new ushort[6];

            indices[0] = 1;
            indices[1] = 0;
            indices[2] = 3;
            indices[3] = 3;
            indices[4] = 0;
            indices[5] = 2;

            IndexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);
        }


        public void Draw(Matrix World, Matrix View, Matrix Projection)
        {
            var graphicsDevice = Effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = IndexBuffer;

            Effect.World = World;
            Effect.View = View;
            Effect.Projection = Projection;

            foreach (var effectPass in Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
        }

        public void Draw(Matrix World, Matrix View, Matrix Projection, Texture2D texture)
        {
            Effect.Parameters["Texture"].SetValue(texture);
            Draw(World, View, Projection);
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
            Effect.Dispose();
        }
    }
}