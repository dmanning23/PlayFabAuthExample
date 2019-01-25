using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;

namespace PlayFabExample
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;

		private string labelText = "Logging into PlayFab...";

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = 480;
			graphics.PreferredBackBufferHeight = 800;
			graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

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

			// TODO: use this.Content to load your game content here

			try
			{
				font = Content.Load<SpriteFont>(@"Fonts\TestFont");
			}
			catch (Exception ex)
			{
				throw;
			}

			LogIntoPlayFab();
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
#if !__IOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();
#endif

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			
			var labelSize = font.MeasureString(labelText);
			var position = new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (labelSize.X / 2), 32);

			spriteBatch.Begin();
			spriteBatch.DrawString(font, labelText, position, Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		#region PlayFab

		private void LogIntoPlayFab()
		{
			PlayFabSettings.TitleId = "144";
			var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
			var loginTask = PlayFabClientAPI.LoginWithCustomIDAsync(request);
			loginTask.ContinueWith(OnLoginComplete);
		}

		private void OnLoginComplete(Task<PlayFabResult<LoginResult>> task)
		{
			labelText = "Unknown failure";
			if (task.Result.Result != null)
			{
				labelText = "Congratulations!\nYou made your first successful API call!";
			}
			if (task.Result.Error != null)
			{
				labelText = "Something went wrong with your first API call.\n"
					+ "Here's some debug information:\n"
					+ task.Result.Error.GenerateErrorReport();
			}
		}

		#endregion //PlayFab
	}
}
