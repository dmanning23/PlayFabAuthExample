using MenuBuddy;
using Microsoft.Xna.Framework;
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
	public class Game1 : ControllerGame
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

			//Store the playfab services in the MonoGame IoC container
			Services.AddService<IPlayFabClient>(client);
			Services.AddService<IPlayFabAuthService>(auth);

			//The OnDisplayAuthentication event is triggered when the user needs to login
			auth.OnDisplayAuthentication += Auth_OnDisplayAuthentication;

			//The OnLoggingIn event is triggered while the auth service is communicating with the PlayFab backend 
			auth.OnLoggingIn += Auth_OnLoggingIn;

			//OnLoginSuccess event is triggered after the user has successfully logged into the PlayFab backend
			auth.OnLoginSuccess += Auth_OnLoginSuccess;

			//There was an error logging in: Could be anything from bad username/password, wifi disconnect, or fatal error
			auth.OnPlayFabError += Auth_OnPlayFabError;
		}

		protected override void InitStyles()
		{
			LoginStyleSheet.DisplayNameFontResource = @"Fonts\ariblk";

			base.InitStyles();
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
