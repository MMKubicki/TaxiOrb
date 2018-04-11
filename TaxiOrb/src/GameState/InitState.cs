namespace TaxiOrb.GameState
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class InitState : GameState
	{
		private enum InitStateState
		{
			FirstRun,
			Loading,
			Done
		}

		private InitStateState currentState;
		private string centerText;
		private string animationText;
		private float animationTimeCounter;


		public InitState(Game game) : base(game)
		{
			var contentLoader = game.Services.GetService<ContentManager>();
			currentState = InitStateState.FirstRun;
			centerText = "Loading";
			animationText = "";
			animationTimeCounter = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			switch (currentState)
			{
				case InitStateState.FirstRun:

					var task = new Task(Load);
					currentState = InitStateState.Loading;
					task.Start();
					return;

				case InitStateState.Loading:
					animationTimeCounter += gameTime.ElapsedGameTime.Milliseconds;

					if (animationTimeCounter >= 500)
					{
						animationText += ".";
						animationTimeCounter = 0f;
					}

					if (animationText.Length > 3)
						animationText = "";

					break;
				case InitStateState.Done:
					
					NextState = new MainMenuState(game);
					Finished = true;

					break;
				default:
					break;
			}
		}

		private async void Load()
		{
			//Make this Method at least 4 Seconds long to see the cool LoadingScreen
			var minWaitTime = Task.Run(() => Thread.Sleep(4000));



			//TODO: Load Content, Write Content to CollectionClass



			await minWaitTime.ConfigureAwait(true);

			currentState = InitStateState.Done;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			game.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			spriteBatch.DrawString(Resources.Font, centerText + animationText, new Vector2(20, 680), Color.White);

			spriteBatch.End();
		}
	}
}
