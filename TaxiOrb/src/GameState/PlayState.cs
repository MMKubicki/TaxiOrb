namespace TaxiOrb.GameState
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using System.Collections.Generic;
	using System.Linq;

	public class PlayState : GameState
	{

		private PlayerOrb player;
		private List<CollectorOrb> collectorOrbs;
		private Ground ground;

		private int collectedCounter = 0;

		private TimeSpan countdown;

		private const int BOUNDS = 20;

		private Random _randomGen;

        public PlayState(Game game) : base(game)
		{
			_randomGen = new Random();
			ground = new Ground(game);
            player = new PlayerOrb(new Vector2(0,0), game, Resources.TaxiOrb, this);
			collectorOrbs = new List<CollectorOrb>();
			generateCollectors(5);
			countdown = new TimeSpan(0, 0, 31);
		}

	    public override void Update(GameTime gameTime)
	    {
		    if (collectorOrbs.Count < 5)
		    {
				generateCollectors(5 - collectorOrbs.Count);
		    }

			player.Update(gameTime);
		    foreach (var orb in collectorOrbs)
		    {
			    orb.Update(gameTime, player);
		    }

		    collectorOrbs = collectorOrbs.Where(o => !o.IsFinished).ToList();


		    countdown -= gameTime.ElapsedGameTime;

		    if (countdown.TotalMilliseconds <= 0)
		    {
				TriggerEnd("Time is up", Color.Gray);
		    }
	    }

		public void TriggerEnd(string reason, Color color)
		{
			NextState = new EndState(game, collectedCounter, reason, color);
			Finished = true;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
	        game.GraphicsDevice.Clear(Color.Blue);

			var camPos = new Vector3(-35,35,30);
			//var camPos = new Vector3(-40, 0, 40);

			spriteBatch.Begin();
			spriteBatch.Draw(Resources.backround, new Rectangle(0,0,1280,720 ), Color.White);
			spriteBatch.End();

	        game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

	        ground.DrawGround(camPos, game);

			foreach (var orb in collectorOrbs)
	        {
		        orb.Draw(camPos, game.GraphicsDevice.Viewport.AspectRatio);
	        }
	        player.Draw(spriteBatch, game.GraphicsDevice.Viewport.AspectRatio, camPos);

			DrawOverlay(spriteBatch);

        }

		private void DrawOverlay(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			spriteBatch.DrawString(Resources.Font, "Time: " + countdown.ToString(@"mm\:ss"), new Vector2(20), Color.White);
			spriteBatch.DrawString(Resources.Font, "Collected: " + collectedCounter.ToString(), new Vector2(1080, 20), Color.White);

			spriteBatch.End();
		}

		private void generateCollectors(int count)
		{
			for (var i = 0; i < count; i++)
			{
				var newOrb = new CollectorOrb(GetNewPosition(), Resources.TaxiOrb, this, game);
				if(_randomGen.NextDouble() < 0.27)
					newOrb.SetDangerous(_randomGen.Next(0, 8));
				collectorOrbs.Add(newOrb);
			}
		}

		private Vector3 GetNewPosition()
		{

			Vector3 newVec;

			do
			{
				var xPosNeg = _randomGen.Next() % 2 == 0;
				var yPosNeg = _randomGen.Next() % 2 == 0;

				newVec = new Vector3((float) _randomGen.NextDouble() * BOUNDS * (xPosNeg ? 1 : -1),
					(float) _randomGen.NextDouble() * BOUNDS * (yPosNeg ? 1 : -1), 0.5f);
			} while (!CheckDistances(newVec));

			return newVec;
		}

		private bool CheckDistances(Vector3 check)
		{
			if ((player.Position - check).Length() < 5)
				return false;
			foreach (var orb in collectorOrbs)
			{
				if ((orb.Position - check).Length() < 5)
					return false;
			}

			return true;
		}

		public void IncScore()
		{
			collectedCounter++;
		}
	}
}
