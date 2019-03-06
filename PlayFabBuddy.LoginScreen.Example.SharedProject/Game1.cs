using MenuBuddy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabBuddyLib;
using PlayFabBuddyLib.Auth;
using PlayFabBuddyLib.LoginScreen;
using System;

namespace PlayFabBuddy.LoginScreen.Example
{
#if __IOS__ || ANDROID || WINDOWS_UAP
	public class Game1 : TouchGame
#else
	public class Game1 : MouseGame
#endif
	{
		public Game1()
		{
			Graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;

#if DESKTOP
			IsMouseVisible = true;
#endif

			VirtualResolution = new Point(720, 1280);
			ScreenResolution = new Point(720, 1280);

			var client = new PlayFabClient("YOUR PLAYFAB APP ID!!!");
			var auth = new PlayFabAuthService(client);

			Services.AddService<IPlayFabClient>(client);
			Services.AddService<IPlayFabAuthService>(auth);

			auth.OnDisplayAuthentication += Auth_OnDisplayAuthentication;
			auth.OnLoggingIn += Auth_OnLoggingIn;
			auth.OnLoginSuccess += Auth_OnLoginSuccess;
			auth.OnPlayFabError += Auth_OnPlayFabError;
		}

		private void Auth_OnLoggingIn()
		{
			ScreenManager.AddScreen(new LoggingInMessageBox());
		}

		private void Auth_OnDisplayAuthentication()
		{
			try
			{
				ScreenManager.RemoveScreens<LoggingInMessageBox>();

				ScreenManager.AddScreen(new LoginMessageBox());
			}
			catch (Exception ex)
			{
				ScreenManager.AddScreen(new ErrorScreen(ex));
			}
		}

		private void Auth_OnLoginSuccess(LoginResult success)
		{
			try
			{
				ScreenManager.RemoveScreens<LoggingInMessageBox>();

				ScreenManager.AddScreen(new OkScreen("Successfully logged in!"), null);
			}
			catch (Exception ex)
			{
				ScreenManager.AddScreen(new ErrorScreen(ex));
			}
		}

		private void Auth_OnPlayFabError(PlayFabError error)
		{
			ScreenManager.RemoveScreens<LoggingInMessageBox>();
		}

		/// <summary>
		/// Get the set of screens needed for the main menu
		/// </summary>
		/// <returns>The gameplay screen stack.</returns>
		public override IScreen[] GetMainMenuScreenStack()
		{
			return new IScreen[] { new MainMenuScreen() };
		}

	}
}
