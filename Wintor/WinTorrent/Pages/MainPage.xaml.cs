using MonoTorrent.Client;
using System;
using System.IO;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinTorrent.Dialogs;
using WinTorrent.Utils;

namespace WinTorrent.Pages
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
			
			sharedShadow.Receivers.Add(shadowRecieverGrid);
			search.Translation += new System.Numerics.Vector3(0, 0, 10);

			navigationList.SelectedIndex = 0;

			//File.Create("E://FUCKYOU.txt");
			//File.WriteAllText("E://FUCKYOU.txt", "FUCK YOU!");
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			Window.Current.SetTitleBar(TitleBar);
		}

		private void OpenSettings(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
			Frame.Navigate(typeof(SettingsPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });

		private async void AddTorrent(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			AddTorrentDialog addDialog = new AddTorrentDialog();
			await addDialog.ShowAsync();

			if (addDialog.Torrent == null)
				return;

			TorrentClient.AddTorrent(addDialog.Torrent, addDialog.DestinationFolder);

			// TODO: Configure torrent
		}

		private void NavigationViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			list.ItemsSource = (navigationList.SelectedItem as NavigationViewItem)?.Tag as string switch
			{
				"downloading" => TorrentClient.Torrents.FindAll(i => i.State.BelongsTo(TorrentState.Downloading, TorrentState.Hashing, TorrentState.Error, TorrentState.Metadata, TorrentState.Starting, TorrentState.Stopping)),
				"seeding" => TorrentClient.Torrents.FindAll(i => i.State.BelongsTo(TorrentState.Seeding)),
				"completed" => TorrentClient.Torrents.FindAll(i => i.State.BelongsTo(TorrentState.Stopped, TorrentState.Seeding)),
				"paused" => TorrentClient.Torrents.FindAll(i => i.State.BelongsTo(TorrentState.Paused, TorrentState.HashingPaused)),
				_ => TorrentClient.Torrents
			};
			search.ItemsSource = null;
		}

		private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
				return;

			sender.ItemsSource = null;
			navigationList.SelectedIndex = 0;
			var result = TorrentClient.Torrents.FindAll(i =>
					i.Torrent.Name.ToLowerInvariant().Contains(sender.Text.ToLowerInvariant()) ||
					(i?.SavePath.ToLowerInvariant().Contains(sender.Text.ToLowerInvariant()) ?? false) ||
					i.State.ToString().ToLowerInvariant().Contains(sender.Text.ToLowerInvariant()));

			list.ItemsSource = result;

			if (result.Count < 1)
			{
				if (sender.Text.ToUri() is Uri magnetUrl && magnetUrl.IsAbsoluteUri)
				{
					sender.ItemsSource = new[] { $"Add torrent from URL: {magnetUrl.AbsoluteUri}" };
				}
				else
				{
					sender.ItemsSource = new[] { "No results found" };
				}
			}
		}

		private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			if (args.SelectedItem as string == "No results found")
				return;
			// TODO: Add torrent from URL
		}

		private void PauseItem(object sender, RoutedEventArgs e)
		{

		}

		private void list_ItemClick(object sender, ItemClickEventArgs e) =>
			OpenItem(e.ClickedItem as TorrentManager);

		private void ViewItem(object sender, RoutedEventArgs e) =>
			OpenItem(list.ItemFromContainer(((sender as FrameworkElement).Parent as FrameworkElement).Parent) as TorrentManager);

		private void OpenItem(TorrentManager item)
		{
			if (list.ContainerFromItem(item) as ListViewItem != null)
				list.PrepareConnectedAnimation("ca1", item, "caTarget");

			Frame.Navigate(typeof(TorrentDetailsPage), item);
		}

		private void SetSeeding(object sender, RoutedEventArgs e)
		{

		}

		private void SetPriority(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteItem(object sender, RoutedEventArgs e)
		{

		}
	}
}