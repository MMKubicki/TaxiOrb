namespace TaxiOrb.GameState
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	public  class EndState : GameState
    {
        private KeyboardState _oldState;

	    private readonly string _reason;
	    private readonly int _score;
	    private readonly Color _screenColor;

        public EndState(Game game, int score, string reason, Color screenColor) : base(game)
        {
            _oldState = Keyboard.GetState();
	        _score = score;
	        _reason = reason;
	        _screenColor = screenColor;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(_screenColor);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            DrawButton(spriteBatch, "Back to Main Menu", true, new Vector2(20, 610));
           
			spriteBatch.DrawString(Resources.Font, _reason, new Vector2(100), Color.White.Minus(_screenColor));
            spriteBatch.DrawString(Resources.Font, $"Score: {_score}", new Vector2(100, 200), Color.White.Minus(_screenColor));

            var acaLogo = Resources.AcaLogo;
            const float scale = 0.25f;

            spriteBatch.Draw(acaLogo, new Rectangle(new Point(880, 460), (new Vector2(acaLogo.Width, acaLogo.Height) * scale).ToPoint()), Color.White);

            spriteBatch.End();
        }

        private static void DrawButton(SpriteBatch spriteBatch, string text, bool isHighlighted, Vector2 position)
        {
            var destinationRectangle = new Rectangle(position.ToPoint(), new Point(360, 60));

            spriteBatch.Draw(Resources.Pixel, destinationRectangle, new Rectangle(0, 0, 1, 1), isHighlighted ? Color.White : Color.Black);

            spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint(), new Point(destinationRectangle.Width, 2)), isHighlighted ? Color.Black : Color.White);
            spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint(), new Point(2, destinationRectangle.Height)), isHighlighted ? Color.Black : Color.White);

            spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint() + new Point(0, destinationRectangle.Height), new Point(destinationRectangle.Width + 2, 2)), isHighlighted ? Color.Black : Color.White);
            spriteBatch.Draw(Resources.Pixel, new Rectangle(position.ToPoint() + new Point(destinationRectangle.Width, 0), new Point(2, destinationRectangle.Height + 2)), isHighlighted ? Color.Black : Color.White);

            var stringSize = Resources.Font.MeasureString(text);
            var textPosition = destinationRectangle.Center - new Point((int)(stringSize.X / 2f), (int)(stringSize.Y / 2f));
            spriteBatch.DrawString(Resources.Font, text, textPosition.ToVector2(), isHighlighted ? Color.Black : Color.White);
        }



        public override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            if (_oldState.IsKeyDown(Keys.Space) && keyState.IsKeyUp(Keys.Space) || _oldState.IsKeyDown(Keys.Enter) && keyState.IsKeyUp(Keys.Enter))
            {
                NextState = new MainMenuState(game);
                Finished = true;
            }

            _oldState = keyState;
        }

    }
}
