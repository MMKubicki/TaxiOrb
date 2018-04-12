namespace TaxiOrb
{
	using System.Collections.Generic;
	using System.Linq;

	using GameState;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameMain : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private List<GameState.GameState> _stateList;

		private KeyboardState _oldState;

		public GameMain()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferMultiSampling = true,
				PreferredBackBufferHeight = 720,
				PreferredBackBufferWidth = 1280,
				GraphicsProfile = GraphicsProfile.HiDef,
				SynchronizeWithVerticalRetrace = true
			};

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			_oldState = Keyboard.GetState();
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			//Add spriteBatch and ContentManager to Services to easily reach them from the outside
			Services.AddService(_spriteBatch);
			Services.AddService(Content);

			LoadBasicContent();

			//InitState as first State for the game
			_stateList = new List<GameState.GameState> { new InitState(this) };

		}

		private void LoadBasicContent()
		{
			var pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData(new[] { Color.White });
			Resources.Pixel = pixel;

			Resources.Font = Content.Load<SpriteFont>("font16");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && _stateList.Last().GetType() != typeof(InitState))
				Exit();

			var keystate = Keyboard.GetState();

			if(_oldState.IsKeyDown(Keys.F11) && keystate.IsKeyUp(Keys.F11))
				_graphics.ToggleFullScreen();

			//Check all GameStates whether they should be removed or add a new State 
			var currentStateCount = _stateList.Count;
			for (var i = 0; i < currentStateCount; i++)
			{
				var nextState = _stateList[i].GetNextState();
				if (nextState != null)
				{
					_stateList.Add(nextState);
					_stateList[i].ResetNextState();
				}

				if (!_stateList[i].IsFinished()) continue;
				_stateList.RemoveAt(i);
				currentStateCount--;
			}

			//Update all States
			foreach (var gameState in _stateList.Where(s => s.IsUpdatable()).ToList())
			{
				gameState.Update(gameTime);
			}

			_oldState = keystate;

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			//Draw all States
			foreach (var gameState in _stateList.Where(s => s.IsDrawable()).ToList())
			{
				gameState.Draw(_spriteBatch);
			}

			base.Draw(gameTime);
		}
	}
}
