using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinTorrent.Utils;
using MonoTorrent;
using System.Security.Cryptography;

namespace WinTorrent.Dialogs
{
	public sealed partial class AddTorrentDialog : ContentDialog
	{
		private StorageFile File { get; set; }
		private Uri MagnetUrl { get; set; }

		public StorageFolder DestinationFolder { get; set; }

		public Torrent Torrent { get; private set; }

		public AddTorrentDialog()
		{
			InitializeComponent();
			RequestedTheme = Settings.Theme switch
			{
				0 => ElementTheme.Light,
				1 => ElementTheme.Dark,
				_ => ElementTheme.Default
			};
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			bool useFile = ((RadioButton)sender).Tag as string == "file";

			filePathField.IsEnabled = useFile;
			filePickerButton.IsEnabled = useFile;

			urlField.IsEnabled = !useFile;

			if (useFile)
				IsPrimaryButtonEnabled = File != null;
			else
				IsPrimaryButtonEnabled = MagnetUrl != null;

			IsPrimaryButtonEnabled = DestinationFolder == null ? false : IsPrimaryButtonEnabled;
		}

		private async void FilePickerButton_Click(object sender, RoutedEventArgs e)
		{
			FileOpenPicker picker = new FileOpenPicker
			{
				CommitButtonText = "Open",
				SuggestedStartLocation = PickerLocationId.Downloads,
			};
			picker.FileTypeFilter.Add(".torrent");

			File = await picker.PickSingleFileAsync();

			if (File == null)
				return;

			filePathField.Text = File.Path;
			IsPrimaryButtonEnabled = DestinationFolder != null;
		}

		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var defferal = args.GetDeferral();
			try
			{
				if (selectFile.IsChecked.Value)
				{
					StorageFile cachedFile = await File.CopyAsync(ApplicationData.Current.LocalFolder, File.Name, NameCollisionOption.GenerateUniqueName);
					Torrent = await Torrent.LoadAsync(cachedFile.Path);
				}
				else
					Torrent = await Torrent.LoadAsync(MagnetUrl, ApplicationData.Current.LocalFolder.Path);
			}
			catch (Exception e)
			{
				args.Cancel = true;
				await new ContentDialog
				{
					Title = "Something went wrong...",
					Content = "We can't load information for source. It may be corrupted or unavailable." +
					"\n" + e.GetType() +
					"\n" + e.Message,
					PrimaryButtonText = "Close",
					DefaultButton = ContentDialogButton.Primary
				}.ShowAsync();
			}

			defferal.Complete();
		}

		private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
		{
			selectFile.Checked += RadioButton_Checked;
			selectUrl.Checked += RadioButton_Checked;
		}

		private async void SavePickerButton_Click(object sender, RoutedEventArgs e)
		{
			FolderPicker folderPicker = new FolderPicker
			{
				SuggestedStartLocation = PickerLocationId.Downloads,
				CommitButtonText = "Choose folder"
			};
			folderPicker.FileTypeFilter.Add("*");
			DestinationFolder = await folderPicker.PickSingleFolderAsync();

			if (DestinationFolder == null)
				return;

			destinationPath.Text = DestinationFolder.Path;
			IsPrimaryButtonEnabled = MagnetUrl != null || File != null;
		}
	}
}
