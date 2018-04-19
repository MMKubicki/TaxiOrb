using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiOrb
{
	using GameState;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class CollectorOrb
	{
		public bool IsFinished = false;
		private Game _game;
		public Vector3 Position;
		private Model _model;
		private PlayState _parent;

		private bool _dangerous = false;
		private float _dangercountdown = 0f;

		public CollectorOrb(Vector3 pos, Model model, PlayState parent, Game game)
		{
			Position = new Vector3(pos.X, pos.Y, 0.5f);
			_game = game;
			_model = model;
			_parent = parent;
		}

		public void Update(GameTime gameTime, PlayerOrb player)
		{
			if (!_dangerous)
			{
				if (CheckCollision(this.Position, player.Position))
				{
					_parent.IncScore();
					IsFinished = true;
				}
			}
			else
			{
				if (CheckCollision(this.Position, player.Position))
				{
					player.Throw();
					IsFinished = true;
				}

				_dangercountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_dangercountdown <= 0)
					_dangerous = false;
			}

		}

		private static bool CheckCollision(Vector3 @this, Vector3 that)
		{
			var thisVector2 = new Vector2(@this.X, @this.Y);
			var thatVector2 = new Vector2(that.X, that.Y);

			return (thisVector2 - thatVector2).Length() < 1.5f;
		}

		public void Draw(Vector3 camPos, float aspectRatio)
		{
			foreach (var mesh in _model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;

					//effect.AmbientLightColor = new Vector3(0.75f,0,0.2f);
					effect.DiffuseColor = _dangerous? new Vector3(0.85f, 0.282f, 0.372f) : new Vector3(0.6f, 0.5f, 0.8f);

					effect.World = GetWorldMatrix();

					effect.View = Matrix.CreateLookAt(camPos, Vector3.Zero, Vector3.UnitZ);
					//effect.View

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
			_dangercountdown = countdownInSeconds;
			_dangerous = true;
		}
	}
}
