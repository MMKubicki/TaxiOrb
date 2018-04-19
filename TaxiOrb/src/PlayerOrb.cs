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
	using Microsoft.Xna.Framework.Input;

	public class PlayerOrb
	{
		private Game _game;

		public Vector3 Position;
		public Model Model;

		private PlayState _parentPlayState;

		private float _speed;
		private Vector3 _movement;

		public PlayerOrb(Vector2 startPos, Game game, Model model, PlayState parent)
		{
			_game = game;
			Position = new Vector3(startPos.X, startPos.Y, 0.5f);
			Model = model;
			_speed = 0.55f;
			_movement = new Vector3(0);
			_parentPlayState = parent;
		}

		public void Update(GameTime gameTime)
		{
			var keystate = Keyboard.GetState();
			if(keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.Up))
				_movement += new Vector3(1, -1,0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.S) || keystate.IsKeyDown(Keys.Down))
				_movement += new Vector3(-1, 1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left))
				_movement += new Vector3(1, 1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
			if (keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right))
				_movement += new Vector3(-1, -1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;

			var tempPos = Position + _movement;
			if (tempPos.X >= 20.2f)
			{
				_movement.X = -_movement.X;
			}
			else if (tempPos.Y <= -20.2f)
			{
				_movement.Y = -_movement.Y;
			}
			else if (tempPos.X <= -20.5f)
			{
				_parentPlayState.TriggerEnd("Fell out of arena", new Color(217, 72, 95));
			}
			else if (tempPos.Y >= 20.5f)
			{
				_parentPlayState.TriggerEnd("Fell out of arena", new Color(217, 72, 95));
			}

			Position += _movement;
		}

		public void Draw(SpriteBatch spriteBatch, float aspectRatio, Vector3 camPos)
		{
			foreach (var mesh in Model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;

					//effect.AmbientLightColor = new Vector3(0.75f,0,0.2f);
					effect.DiffuseColor = new Vector3(1 , 0.9f , 0.2f);

					effect.World = GetWorldMatrix();

					effect.View = Matrix.CreateLookAt(camPos, Vector3.Zero, Vector3.UnitZ);
					//effect.View

					effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 2000);
				}

				mesh.Draw();
			}

		}

		Matrix GetWorldMatrix()
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
