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
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private List<GameState.GameState> StateList;

		public GameMain()
		{
			graphics = new GraphicsDeviceManager(this);
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
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//Add spriteBatch and ContentManager to Services to easily reach them from the outside
			this.Services.AddService(spriteBatch);
			this.Services.AddService(Content);

			//InitState as first State for the game
			StateList = new List<GameState.GameState> { new InitState(this) };

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			//Check all GameStates whether they should be removed or add a new State 
			var currentStateCount = StateList.Count;
			for (var i = 0; i < currentStateCount; i++)
			{
				var nextState = StateList[i].GetNextState();
				if(nextState != null) StateList.Add(nextState);

				if (!StateList[i].IsFinished()) continue;
				StateList.RemoveAt(i);
				currentStateCount--;
			}

			//Update all States
			foreach (var gameState in StateList.Where(s => s.IsUpdatable()).ToList())
			{
				gameState.Update(gameTime);
			}

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
			foreach (var gameState in StateList.Where(s => s.IsDrawable()).ToList())
			{
				gameState.Draw(spriteBatch);
			}

			base.Draw(gameTime);
		}
	}
}
