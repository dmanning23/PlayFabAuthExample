using InputHelper;
using MenuBuddy;
using Microsoft.Xna.Framework;
using PlayFabBuddyLib.Auth;
using ResolutionBuddy;
using System;
using System.Threading.Tasks;

namespace PlayFabBuddy.LoginScreen.Example
{
	/// <summary>
	/// The main menu screen is the first thing displayed when the game starts up.
	/// </summary>
	public class MainMenuScreen : MenuScreen, IMainMenu
	{
		IPlayFabAuthService Auth { get; set; }
		IMenuEntry _clearMenuEntry;
		IMenuEntry _clearAuthMenuEntry;
		ILabel _id;

		/// <summary>
		/// Constructor fills in the menu contents.
		/// </summary>
		public MainMenuScreen()
			: base("Main Menu")
		{
		}

		public override async Task LoadContent()
		{
			await base.LoadContent();

			Auth = ScreenManager.Game.Services.GetService<IPlayFabAuthService>();
			Auth.OnLoginSuccess -= Auth_OnLoginSuccess;
			Auth.OnLoginSuccess += Auth_OnLoginSuccess;

			var entry3 = new MenuEntry("Login", Content);
			entry3.OnClick += ((object obj, ClickEventArgs e) =>
			{
				try
				{
					Auth.Authenticate();
				}
				catch (Exception ex)
				{
					ScreenManager.AddScreen(new ErrorScreen(ex));
				}
			});
			AddMenuEntry(entry3);

			_id = new Label("", Content, FontSize.Small)
			{
				Horizontal = HorizontalAlignment.Left,
				Vertical = VerticalAlignment.Top,
				Position = new Point(Resolution.TitleSafeArea.Left, Resolution.TitleSafeArea.Top),
			};
			AddItem(_id);

			AddCleanRememberMeOption();
			AddClearAuthOption();
			AddId();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			Auth.OnLoginSuccess -= Auth_OnLoginSuccess;
		}

		private void Auth_OnLoginSuccess(PlayFab.ClientModels.LoginResult success)
		{
			AddCleanRememberMeOption();
			AddClearAuthOption();
			AddId();
		}

		private void AddCleanRememberMeOption()
		{
			if (Auth.RememberMe && null == _clearMenuEntry)
			{
				_clearMenuEntry = new MenuEntry("Clear RememberMe", Content);
				_clearMenuEntry.OnClick += ((object obj, ClickEventArgs e) =>
				{
					Auth.ClearRememberMe();
					RemoveItem(_clearMenuEntry);
					MenuEntries.RemoveItem(_clearMenuEntry);
					_clearMenuEntry = null;
				});
				AddMenuEntry(_clearMenuEntry);
			}
		}

		private void AddClearAuthOption()
		{
			if (Auth.AuthType != AuthType.None && null == _clearAuthMenuEntry)
			{
				_clearAuthMenuEntry = new MenuEntry(Auth.AuthType.ToString(), Content);
				_clearAuthMenuEntry.OnClick += ((object obj, ClickEventArgs e) =>
				{
					Auth.AuthType = AuthType.None;
					RemoveItem(_clearAuthMenuEntry);
					MenuEntries.RemoveItem(_clearAuthMenuEntry);
					_clearAuthMenuEntry = null;
				});
				AddMenuEntry(_clearAuthMenuEntry);
			}
		}

		private void AddId()
		{
			if (!string.IsNullOrEmpty(Auth.PlayFabId))
			{
				_id.Text = Auth.PlayFabId;
			}
		}
	}
}