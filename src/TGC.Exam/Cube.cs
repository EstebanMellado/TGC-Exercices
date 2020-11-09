using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.Exam
{
    class Cube
    {
        private VertexBuffer VertexBuffer { get; set; }
        private IndexBuffer IndexBuffer { get; set; }

        private BasicEffect Effect { get; set; }

        /// <summary>
        ///     Constructs a new cube primitive.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        public Cube(GraphicsDevice graphicsDevice)
        {
            CreateVertexBuffer(graphicsDevice);
            CreateIndexBuffer(graphicsDevice);

            // Create a BasicEffect, which will be used to render the primitive.
            Effect = new BasicEffect(graphicsDevice);
            Effect.VertexColorEnabled = true;
            Effect.TextureEnabled = false;
        }

        private void CreateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColor.VertexDeclaration, 8, BufferUsage.WriteOnly);

            VertexPositionColor[] vertices = new VertexPositionColor[8]
            {
                new VertexPositionColor(new Vector3(-1f, -1f, 1f), Color.White),
                new VertexPositionColor(new Vector3(1f, -1f, 1f), Color.White),
                new VertexPositionColor(new Vector3(-1f, 1f, 1f), Color.White),
                new VertexPositionColor(new Vector3(1f, 1f, 1f), Color.White),
                new VertexPositionColor(new Vector3(-1f, 1f, -1f), Color.White),
                new VertexPositionColor(new Vector3(1f, 1f, -1f), Color.White),
                new VertexPositionColor(new Vector3(-1f, -1f, -1f), Color.White),
                new VertexPositionColor(new Vector3(1f, -1f, -1f), Color.White)
            };

            VertexBuffer.SetData(vertices);
        }
        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            IndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 6 * 6, BufferUsage.WriteOnly);

            ushort[] indices = new ushort[6 * 6]
            {
                7, 1, 0,
                0, 6, 7,

                6, 0, 2,
                2, 4, 6,

                4, 2, 3,
                3, 5, 4,

                5, 3, 1,
                1, 7, 5,

                3, 2, 0,
                0, 1, 3,

                7, 6, 4,
                4, 5, 7
            };

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

                var primitiveCount = IndexBuffer.IndexCount / 3;
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, primitiveCount);
            }
        }

        public void Draw(Matrix World, Matrix View, Matrix Projection, Color color)
        {
            Effect.DiffuseColor = color.ToVector3();
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
