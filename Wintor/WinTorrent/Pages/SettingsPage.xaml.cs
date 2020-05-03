using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinTorrent.Utils;

namespace WinTorrent.Pages
{
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			InitializeComponent();
			Window.Current.SetTitleBar(TitleBar);

			feedback.Visibility = Feedback.HasFeedbackHub ? Visibility.Visible : Visibility.Collapsed;

			switch (Settings.Theme)
			{
				case 0:
					light.IsChecked = true;
					break;
				case 1:
					dark.IsChecked = true;
					break;
				case 2:
					system.IsChecked = true;
					break;
			}

			// TODO: Add settings management
		}

		private void BackRequested(object sender, RoutedEventArgs e) =>
			Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());

		private void ThemeChanged(object sender, RoutedEventArgs e)
		{
			if (Settings.Theme.ToString() == (string)(sender as RadioButton).Tag)
				return;

			Settings.Theme = int.Parse((sender as RadioButton).Tag as string);

			Frame.RequestedTheme = (sender as RadioButton).Name switch
			{
				"light" => ElementTheme.Light,
				"dark" => ElementTheme.Dark,
				_ => Application.Current.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light
			};

			if (Frame.RequestedTheme == ElementTheme.Dark)
				Frame.Background = new AcrylicBrush
				{
					TintOpacity = .8,
					TintColor = Colors.Black,
					BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
					FallbackColor = Color.FromArgb(255, 31, 31, 31)
				};
			else
				Frame.Background = new AcrylicBrush
				{
					TintOpacity = .5,
					TintColor = Colors.White,
					BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
					FallbackColor = Color.FromArgb(255, 230, 230, 230)
				};
			App.UpdateTitleBar(Frame.RequestedTheme == ElementTheme.Dark);
		}
	}
}