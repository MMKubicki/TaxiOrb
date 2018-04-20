namespace TaxiOrb
{
	using GameState;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	public class PlayerOrb
	{
		public Vector3 Position;
		public Model Model;

		private readonly PlayState _parentPlayState;

		private readonly float _speed;
		private Vector3 _movement;


		private Vector3 _moveUp;
		private Vector3 _moveDown;
		private Vector3 _moveLeft;
		private Vector3 _moveRight;

		public PlayerOrb(Vector2 startPosition, Model model, PlayState parent)
		{
			Position = new Vector3(startPosition.X, startPosition.Y, 0.5f);
			Model = model;
			_speed = 0.55f;
			_movement = new Vector3(0);
			_parentPlayState = parent;

			_moveUp = new Vector3(1,-1,0);
			_moveUp.Normalize();
			_moveDown = new Vector3(-1, 1, 0);
			_moveDown.Normalize();
			_moveLeft = new Vector3(1,1,0);
			_moveLeft.Normalize();
			_moveRight = new Vector3(-1,-1,0);
			_moveRight.Normalize();
		}

		public void Update(GameTime gameTime)
		{
			var keystate = Keyboard.GetState();

			if(keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.Up))
				_movement += _moveUp * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.S) || keystate.IsKeyDown(Keys.Down))
				_movement += _moveDown * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left))
				_movement += _moveLeft * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right))
				_movement += _moveRight * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;

			var tempPosition = Position + _movement;
			if (tempPosition.X >= 20.2f)
			{
				_movement.X = -_movement.X;
			}
			else if (tempPosition.Y <= -20.2f)
			{
				_movement.Y = -_movement.Y;
			}
			else if (tempPosition.X <= -20.5f)
			{
				_parentPlayState.TriggerEnd("Fell out of the arena", Color.Red);
			}
			else if (tempPosition.Y >= 20.5f)
			{
				_parentPlayState.TriggerEnd("Fell out of the arena", Color.Red);
			}

			Position += _movement;
		}

		public void Draw(SpriteBatch spriteBatch, float aspectRatio, Vector3 camPosition)
		{
			foreach (var mesh in Model.Meshes)
			{
				foreach (var effect1 in mesh.Effects)
				{
					var effect = (BasicEffect) effect1;
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;

					effect.DiffuseColor = new Vector3(1 , 0.9f , 0.2f);

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

		public void Throw()
		{
			_movement.X = -_movement.X;
			_movement.Y = -_movement.Y;
		}
	}
}
