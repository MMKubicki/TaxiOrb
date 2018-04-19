namespace TaxiOrb.GameState
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	public class CreditState : GameState
	{
		private KeyboardState _oldState;
		private readonly GameState _parent;


		public CreditState(Game game, GameState parent) : base(game)
		{
			_oldState = Keyboard.GetState();
			_parent = parent;
		}

		public override void Update(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();

			if (_oldState.IsKeyDown(Keys.Space) && keyState.IsKeyUp(Keys.Space) || _oldState.IsKeyDown(Keys.Enter) && keyState.IsKeyUp(Keys.Enter))
			{
				_parent.SetUpdatable(true);
				Finished = true;
			}

			_oldState = keyState;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			game.GraphicsDevice.Clear(Color.Gray);
			spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			spriteBatch.DrawString(Resources.Font, "Press Enter or Space to return.", new Vector2(20, 680), Color.Black);

			const string centerText = "          Made by      \n\n" +
									  "      Olja Mozheiko   \n" +
			                          "Michael Mario Kubicki" ;
			var textSize = Resources.Font.MeasureString(centerText);
			spriteBatch.DrawString(Resources.Font, centerText, new Vector2((game.GraphicsDevice.Viewport.Width / 2f) - (textSize.X/2f),  (game.GraphicsDevice.Viewport.Height/2f) - (textSize.Y/2f)), Color.Black);

			var acaLogo = Resources.AcaLogo;
			const float scale = 0.25f;
			spriteBatch.Draw(acaLogo, new Rectangle(new Point(880,460), (new Vector2(acaLogo.Width, acaLogo.Height) * scale).ToPoint()), Color.White);

			spriteBatch.End();
		}
	}
}
