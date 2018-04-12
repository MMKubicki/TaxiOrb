namespace TaxiOrb.GameState
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	public class MainMenuState : GameState
	{
		private enum HighlightedButton
		{
			Play = 0,
			End = 1,
			Credits = 2
		}

		private HighlightedButton _currentButton;
		private KeyboardState _oldState;

		public MainMenuState(Game game) : base(game)
		{
			_currentButton = HighlightedButton.Play;
			_oldState = Keyboard.GetState();
		}

		public override void Update(GameTime gameTime)
		{
			var keystate = Keyboard.GetState();

			if ((_oldState.IsKeyUp(Keys.W) && keystate.IsKeyDown(Keys.W)) ||
			    (_oldState.IsKeyUp(Keys.Up) && keystate.IsKeyDown(Keys.Up)))
			{
				switch (_currentButton)
				{
					case HighlightedButton.Play:
						_currentButton = HighlightedButton.Credits;
						break;
					case HighlightedButton.End:
						_currentButton = HighlightedButton.Play;
						break;
					case HighlightedButton.Credits:
						_currentButton = HighlightedButton.End;
						break;
				}
			}

			if ((_oldState.IsKeyUp(Keys.S) && keystate.IsKeyDown(Keys.S)) ||
			    (_oldState.IsKeyUp(Keys.Down) && keystate.IsKeyDown(Keys.Down)))
			{
				switch (_currentButton)
				{
					case HighlightedButton.Play:
						_currentButton = HighlightedButton.End;
						break;
					case HighlightedButton.End:
						_currentButton = HighlightedButton.Credits;
						break;
					case HighlightedButton.Credits:
						_currentButton = HighlightedButton.Play;
						break;
				}
			}

			if ((_oldState.IsKeyUp(Keys.Enter) && keystate.IsKeyDown(Keys.Enter)) ||
			    (_oldState.IsKeyUp(Keys.Space) && keystate.IsKeyDown(Keys.Space)))
			{
				switch (_currentButton)
				{
					case HighlightedButton.Play:
						NextState = new PlayState(game);
						Finished = true;
						break;

					case HighlightedButton.End:
						game.Exit();
						break;

					case HighlightedButton.Credits:

						//TODO: Add credits

						break;
				}
			}

			_oldState = keystate;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			game.GraphicsDevice.Clear(Color.Gray);

			spriteBatch.Begin();

			DrawButton(spriteBatch, "Play", _currentButton == HighlightedButton.Play, new Vector2(20, 20));
			DrawButton(spriteBatch, "Exit", _currentButton == HighlightedButton.End, new Vector2(20, 120));
			DrawButton(spriteBatch, "Credits", _currentButton == HighlightedButton.Credits, new Vector2(20, 610));

			spriteBatch.End();
		}

		private static void DrawButton(SpriteBatch spriteBatch, string text, bool isHighlighted, Vector2 position)
		{
			var destinationRectangle = new Rectangle(position.ToPoint(), new Point(360, 60));

			spriteBatch.Draw(Resources.Pixel, destinationRectangle, new Rectangle(0,0,1,1), isHighlighted ? Color.White : Color.Black);

			spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint(), new Point(destinationRectangle.Width, 2)), isHighlighted ? Color.Black : Color.White);
			spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint(), new Point(2, destinationRectangle.Height)), isHighlighted ? Color.Black : Color.White);

			spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint() + new Point(0 ,destinationRectangle.Height), new Point(destinationRectangle.Width + 2, 2)), isHighlighted ? Color.Black : Color.White);
			spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint() + new Point(destinationRectangle.Width, 0), new Point(2, destinationRectangle.Height + 2)), isHighlighted ? Color.Black : Color.White);

			var stringSize = Resources.Font.MeasureString(text);
			var textPosition = destinationRectangle.Center - new Point((int)(stringSize.X / 2f),(int)(stringSize.Y / 2f));
			spriteBatch.DrawString(Resources.Font, text, textPosition.ToVector2(), isHighlighted ? Color.Black : Color.White);
		}
	}
}
