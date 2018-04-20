namespace TaxiOrb.GameState
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Xna.Framework.Input;

	public class PlayState : GameState
	{

		private readonly PlayerOrb _player;
		private List<CollectorOrb> _collectorOrbs;
		private readonly Ground _ground;

		private int _collectedCounter;

		private TimeSpan _countdown;

		private const int Bounds = 20;

		private readonly Random _randomGen;
		private readonly Vector3 _cameraPosition;

		private KeyboardState _oldState;

        public PlayState(Game game) : base(game)
		{
			_randomGen = new Random();
			_ground = new Ground(game);
            _player = new PlayerOrb(new Vector2(0,0), Resources.TaxiOrb, this);
			_collectorOrbs = new List<CollectorOrb>();
			GenerateCollectors(5);
			_countdown = new TimeSpan(0, 0, 31);
			_cameraPosition = new Vector3(-35, 35, 30);
			_oldState = Keyboard.GetState();
		}

	    public override void Update(GameTime gameTime)
	    {
		    var keystate = Keyboard.GetState();
		    if (_collectorOrbs.Count < 5)
		    {
				GenerateCollectors(5 - _collectorOrbs.Count);
		    }

			_player.Update(gameTime);
		    foreach (var orb in _collectorOrbs)
		    {
			    orb.Update(gameTime, _player);
		    }

		    _collectorOrbs = _collectorOrbs.Where(o => !o.IsFinished).ToList();


		    _countdown -= gameTime.ElapsedGameTime;

		    if (_countdown.TotalMilliseconds <= 0)
		    {
				TriggerEnd("Time is up", Color.Gray);
		    }

		    if (keystate.IsKeyUp(Keys.P) && _oldState.IsKeyDown(Keys.P))
		    {
			    Finished = true;
				NextState = new MainMenuState(game);
		    }

		    _oldState = keystate;
	    }

		public void TriggerEnd(string reason, Color color)
		{
			NextState = new EndState(game, _collectedCounter, reason, color);
			Finished = true;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
	        game.GraphicsDevice.Clear(Color.CornflowerBlue);

			

			spriteBatch.Begin();
			spriteBatch.Draw(Resources.Background, new Rectangle(0,0,game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height ), Color.White);
			spriteBatch.End();

	        game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


			foreach (var orb in _collectorOrbs)
	        {
		        orb.Draw(_cameraPosition, game.GraphicsDevice.Viewport.AspectRatio);
	        }
	        _player.Draw(spriteBatch, game.GraphicsDevice.Viewport.AspectRatio, _cameraPosition);


	        _ground.DrawGround(_cameraPosition, game);

			DrawOverlay(spriteBatch);

        }

		private void DrawOverlay(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			spriteBatch.DrawString(Resources.Font, "Time: " + _countdown.ToString(@"ss") + "s", new Vector2(20), Color.White);
			spriteBatch.DrawString(Resources.Font, "Collected: " + _collectedCounter.ToString(), new Vector2(1080, 20), Color.White);

			spriteBatch.End();
		}

		private void GenerateCollectors(int count)
		{
			for (var i = 0; i < count; i++)
			{
				var newOrb = new CollectorOrb(GetNewPosition(), Resources.TaxiOrb, this);
				if(_randomGen.NextDouble() < 0.27)
					newOrb.SetDangerous(_randomGen.Next(0, 10));
				_collectorOrbs.Add(newOrb);
			}
		}

		private Vector3 GetNewPosition()
		{

			Vector3 newVector3;

			do
			{
				var xPositionNeg = _randomGen.Next() % 2 == 0;
				var yPositionNeg = _randomGen.Next() % 2 == 0;

				newVector3 = new Vector3((float) _randomGen.NextDouble() * Bounds * (xPositionNeg ? 1 : -1),
					(float) _randomGen.NextDouble() * Bounds * (yPositionNeg ? 1 : -1), 0.5f);
			} while (!CheckDistances(newVector3));

			return newVector3;
		}

		private bool CheckDistances(Vector3 check)
		{
			if ((_player.Position - check).Length() < 5)
				return false;
			foreach (var orb in _collectorOrbs)
			{
				if ((orb.Position - check).Length() < 5)
					return false;
			}

			return true;
		}

		public void IncScore()
		{
			_collectedCounter++;
		}
	}
}
