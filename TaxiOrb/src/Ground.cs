namespace TaxiOrb
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class Ground
	{
		private readonly VertexBuffer _vertexBuffer;
		private readonly IndexBuffer _indexBuffer;

		private readonly BasicEffect _effect;
		private Matrix _world;
		private Matrix _view;
		private Matrix _projection;

		public Ground(Game game)
		{
			_effect = new BasicEffect(game.GraphicsDevice);

			var vertices = new VertexPositionColor[12];

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

			_vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
			_vertexBuffer.SetData(vertices);

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

			_indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
			_indexBuffer.SetData(indices);
		}

		public void DrawGround(Vector3 camPosition, Game game)
		{
			_world = Matrix.CreateTranslation(0, 0, 0);
			_view = Matrix.CreateLookAt(camPosition, Vector3.Zero, Vector3.UnitZ);
			_projection =
				Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 200);

			_effect.World = _world;
			_effect.View = _view;
			_effect.Projection = _projection;
			_effect.VertexColorEnabled = true;

			game.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
			game.GraphicsDevice.Indices = _indexBuffer;

			var rasterizerState = new RasterizerState {CullMode = CullMode.None};
			game.GraphicsDevice.RasterizerState = rasterizerState;

			foreach (var pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0,6 );
			}
		}
	}
}
