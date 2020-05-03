using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinTorrent.Pages;
using WinTorrent.Utils;

namespace WinTorrent
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
	{
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
		}

		public static void UpdateTitleBar(bool isDark)
		{
			ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveForegroundColor = Colors.Gray;

			if (isDark)
			{
				titleBar.ButtonForegroundColor =
					titleBar.ButtonHoverForegroundColor =
						titleBar.ButtonPressedForegroundColor = Colors.White;
				titleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 255, 255, 255);
				titleBar.ButtonPressedBackgroundColor = Color.FromArgb(30, 255, 255, 255);
			}
			else
			{
				titleBar.ButtonForegroundColor =
					titleBar.ButtonHoverForegroundColor =
						titleBar.ButtonPressedForegroundColor = Colors.Black;
				titleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 0, 0, 0);
				titleBar.ButtonPressedBackgroundColor = Color.FromArgb(70, 0, 0, 0);
			}
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				Window.Current.Content = rootFrame;
			}

			if (!e.PrelaunchActivated)
			{
				if (rootFrame.Content == null)
					rootFrame.Navigate(typeof(MainPage), e.Arguments);

				Window.Current.Activate();
			}

			rootFrame.RequestedTheme = Settings.Theme switch
			{
				0 => ElementTheme.Light,
				1 => ElementTheme.Dark,
				_ => RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light
			};
			if (rootFrame.RequestedTheme == ElementTheme.Dark)
				rootFrame.Background = new AcrylicBrush
				{
					TintOpacity = .8,
					TintColor = Colors.Black,
					BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
					FallbackColor = Color.FromArgb(255, 31, 31, 31)
				};
			else
				rootFrame.Background = new AcrylicBrush
				{
					TintOpacity = .5,
					TintColor = Colors.White,
					BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
					FallbackColor = Color.FromArgb(255, 230, 230, 230)
				};
			UpdateTitleBar(rootFrame.RequestedTheme == ElementTheme.Dark);
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
