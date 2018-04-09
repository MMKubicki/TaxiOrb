namespace TaxiOrb.GameState
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class InitState : GameState
	{
		public InitState(Game game) : base(game)
		{
			var contentLoader = game.Services.GetService<ContentManager>();
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			spriteBatch.End();
		}
	}
}
