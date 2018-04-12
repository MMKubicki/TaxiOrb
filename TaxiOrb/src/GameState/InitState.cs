namespace TaxiOrb.GameState
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class InitState : GameState
	{
		private enum InitStateState
		{
			FirstRun,
			Loading,
			Done
		}

		private InitStateState _currentState;
		private readonly string _centerText;
		private string _animationText;
		private float _animationTimeCounter;

		private const int MinMillisecondsLoading = 2000;

		private float _pulseTime;

		public InitState(Game game) : base(game)
		{
			_currentState = InitStateState.FirstRun;
			_centerText = "Loading";
			_animationText = "";
			_animationTimeCounter = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			switch (_currentState)
			{
				case InitStateState.FirstRun:

					var task = new Task(Load);
					_currentState = InitStateState.Loading;
					task.Start();
					return;

				case InitStateState.Loading:
					_animationTimeCounter += gameTime.ElapsedGameTime.Milliseconds;

					if (_animationTimeCounter >= 500)
					{
						_animationText += ".";
						_animationTimeCounter = 0f;
					}

					if (_animationText.Length > 3)
						_animationText = "";

					_pulseTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

					break;
				case InitStateState.Done:
					
					NextState = new MainMenuState(game);
					Finished = true;

					break;
			}
		}

		private async void Load()
		{
			//Make this Method at least 4 Seconds long to see the cool LoadingScreen
			var minWaitTime = Task.Run(() => Thread.Sleep(MinMillisecondsLoading));



			//TODO: Load Content, Write Content to CollectionClass



			await minWaitTime.ConfigureAwait(true);

			_currentState = InitStateState.Done;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			game.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, null);

			spriteBatch.DrawString(Resources.Font, _centerText + _animationText, new Vector2(20, 680), Color.White);

			var centerPosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2f, game.GraphicsDevice.Viewport.Height / 2f) - Resources.Font.MeasureString("TaxiOrb")/2;

			spriteBatch.DrawString(Resources.Font, "TaxiOrb", centerPosition.ToPoint().ToVector2(), new Color(Color.White, (float)Math.Abs(Math.Cos(_pulseTime))));

			spriteBatch.End();
		}
	}
}
