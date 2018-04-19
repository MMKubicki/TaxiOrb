namespace TaxiOrb
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class Ground
	{
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		private BasicEffect effect;
		private Matrix world;
		private Matrix view;
		private Matrix projection;

		public Ground(Game game)
		{
			effect = new BasicEffect(game.GraphicsDevice);

			VertexPositionColor[] vertices = new VertexPositionColor[12];

			vertices[0] = new VertexPositionColor(new Vector3(-21, -21, 0), Color.Blue);
			vertices[1] = new VertexPositionColor(new Vector3(21, -21, 0), Color.Blue);
			vertices[2] = new VertexPositionColor(new Vector3(21, 21, 0), Color.Blue);
			vertices[3] = new VertexPositionColor(new Vector3(-21, 21, 0), Color.Blue);

			vertices[4] = new VertexPositionColor(new Vector3(-21, -21, 11), Color.LightBlue);
			vertices[5] = new VertexPositionColor(new Vector3(21, -21, 11), Color.LightBlue);
			vertices[6] = new VertexPositionColor(new Vector3(-21, -21, 0), Color.LightBlue);
			vertices[7] = new VertexPositionColor(new Vector3(21, -21, 0), Color.LightBlue);

			vertices[8] = new VertexPositionColor(new Vector3(21, -21, 11), Color.LightSteelBlue);
			vertices[9] = new VertexPositionColor(new Vector3(21, 21, 11), Color.LightSteelBlue);
			vertices[10] = new VertexPositionColor(new Vector3(21, -21, 0), Color.LightSteelBlue);
			vertices[11] = new VertexPositionColor(new Vector3(21, 21, 0), Color.LightSteelBlue);

			vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
			vertexBuffer.SetData<VertexPositionColor>(vertices);

			var indices = new short[18];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;

			indices[3] = 0;
			indices[4] = 2;
			indices[5] = 3;

			indices[6] = 4;
			indices[7] = 6;
			indices[8] = 7;

			indices[9] = 4;
			indices[10] = 5;
			indices[11] = 7;


			indices[12] = 8;
			indices[13] = 10;
			indices[14] = 11;

			indices[15] = 8;
			indices[16] = 9;
			indices[17] = 11;

			indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
			indexBuffer.SetData(indices);
		}

		public void DrawGround(Vector3 camPos, Game game)
		{
			world = Matrix.CreateTranslation(0, 0, 0);
			view = Matrix.CreateLookAt(camPos, Vector3.Zero, Vector3.UnitZ);
			projection =
				Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 200);

			effect.World = world;
			effect.View = view;
			effect.Projection = projection;
			effect.VertexColorEnabled = true;

			game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			game.GraphicsDevice.Indices = indexBuffer;

			var rasterizerState = new RasterizerState {CullMode = CullMode.None};
			game.GraphicsDevice.RasterizerState = rasterizerState;

			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0,6 );
			}
		}
	}
}
