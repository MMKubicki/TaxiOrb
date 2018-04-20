namespace TaxiOrb
{
	using GameState;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class CollectorOrb
	{
		public bool IsFinished;
		public Vector3 Position;
		private readonly Model _model;
		private readonly PlayState _parent;

		private bool _dangerous;
		private float _dangerCountdown;

		public CollectorOrb(Vector3 position, Model model, PlayState parent)
		{
			Position = new Vector3(position.X, position.Y, 0.5f);
			_model = model;
			_parent = parent;
		}

		public void Update(GameTime gameTime, PlayerOrb player)
		{
			if (!_dangerous)
			{
				if (!CheckCollision(Position, player.Position)) return;

				_parent.IncScore();
				IsFinished = true;
			}
			else
			{
				if (CheckCollision(Position, player.Position))
				{
					player.Throw();
					IsFinished = true;
				}

				_dangerCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_dangerCountdown <= 0)
					_dangerous = false;
			}

		}

		private static bool CheckCollision(Vector3 @this, Vector3 that)
		{
			var thisVector2 = new Vector2(@this.X, @this.Y);
			var thatVector2 = new Vector2(that.X, that.Y);

			return (thisVector2 - thatVector2).Length() < 1.7f;
		}

		public void Draw(Vector3 camPosition, float aspectRatio)
		{
			foreach (var mesh in _model.Meshes)
			{
				foreach (var effect1 in mesh.Effects)
				{
					var effect = (BasicEffect) effect1;

					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;

					effect.DiffuseColor = _dangerous? new Vector3(0.85f, 0.282f, 0.372f) : new Vector3(0.6f, 0.5f, 0.8f);

					effect.World = GetWorldMatrix();

					effect.View = Matrix.CreateLookAt(camPosition, Vector3.Zero, Vector3.UnitZ);

					effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 2000);
				}

				mesh.Draw();
			}
		}

		private Matrix GetWorldMatrix()
		{
			var translationMatrix = Matrix.CreateTranslation(Position);

			var combined = translationMatrix;
			return combined;
		}

		public void SetDangerous(float countdownInSeconds)
		{
			_dangerCountdown = countdownInSeconds;
			_dangerous = true;
		}
	}
}
