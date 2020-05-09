using MonoTorrent.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinTorrent.Models;

namespace WinTorrent.Pages
{
	public sealed partial class TorrentDetailsPage : Page
	{
		public TorrentManager Item { get; private set; }

		public TorrentDetailsPage()
		{
			InitializeComponent();
			Window.Current.SetTitleBar(TitleBar);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			Item = e.Parameter as TorrentManager;

			if (ConnectedAnimationService.GetForCurrentView().GetAnimation("ca1") is ConnectedAnimation animation)
				animation.TryStart(caTarget);
		}

		private void BackRequested(object sender, RoutedEventArgs e) =>
			Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
	}
}
