using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PerpetualEngine.Storage;

namespace PlayFabBuddy.LoginScreen.Example.Android
{
	[Activity(Label = "PlayFabBuddy.LoginScreen.Example.Android"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorPortrait
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SimpleStorage.SetContext(ApplicationContext);

			var g = new Game1();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run();
		}
	}
}

