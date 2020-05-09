using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;
using WinTorrent.Models;

namespace WinTorrent.Utils
{
	public static class TorrentClient
	{
		private static ApplicationDataContainer LocalSettings { get; } = ApplicationData.Current.LocalSettings;
		private static StorageFolder LocalStorage { get; } = ApplicationData.Current.LocalFolder;

		private static ClientEngine Engine { get; set; }
		public static EngineSettings Settings => Engine?.Settings;

		public static List<TorrentManager> Torrents { get; } = new List<TorrentManager>();

		private static BEncodedDictionary FastResumeData { get; set; } = new BEncodedDictionary();
		private static List<TorrentMetadataInfo> TorrentsMetadata { get; set; }

		static TorrentClient()
		{
			#region TestData
			//Torrents = new List<TorrentItem>
			//{
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Downloading,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Completed,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Paused,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Seeding,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Cancelling,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Initializing,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Pausing,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Resuming,
			//		SeedCount = 15
			//	},
			//	new TorrentItem
			//	{
			//		Title = "Microsoft Edge",
			//		TotalSize = new DataSize(3221225472),
			//		Downloaded = new DataSize(125829120),
			//		TransmissionSpeed = new DownloadSpeed(33554432),
			//		RemainingTime = TimeSpan.FromSeconds(36),
			//		State = Models.TorrentState.Error,
			//		SeedCount = 15
			//	}
			//};
			#endregion

			EngineSettings engineSettings = new EngineSettings
			{
				SavePath = ApplicationData.Current.LocalFolder.Path,
				MaximumDownloadSpeed = 0,   // Set from settings
				MaximumUploadSpeed = 0
			};

			Engine = new ClientEngine(engineSettings);

			Restore();
		}

		public static async void AddTorrent(Torrent torrent, StorageFolder destinationFolder)
		{
			TorrentMetadataInfo meta = new TorrentMetadataInfo
			{
				DestinationFolderFALToken = StorageApplicationPermissions.FutureAccessList.Add(destinationFolder),
				TorrentFileName = new FileInfo(torrent.TorrentPath).Name
			};

			TorrentManager manager = new TorrentManager(torrent, destinationFolder.Path);
			meta.TorrentHash = manager.InfoHash.ToHex();

			if (TorrentsMetadata.Any(i => i.TorrentHash == meta.TorrentHash))
			{
				ContentDialog alert = new ContentDialog
				{
					Title = "Duplicate torrent",
					Content = "You have previously added this torrent to the queue. Do you want to discard it and start a new download?",
					PrimaryButtonText = "Yes",
					CloseButtonText = "No",
					DefaultButton = ContentDialogButton.Close
				};

				if (await alert.ShowAsync() == ContentDialogResult.Primary)
				{
					FastResumeData.Remove(meta.TorrentHash);
					TorrentsMetadata.RemoveAll(i => i.TorrentHash == meta.TorrentHash);
					foreach(TorrentManager t in Torrents.FindAll(i => i.InfoHash.ToHex() == meta.TorrentHash))
					{
						await t.StopAsync();
						await Engine.Unregister(t);
					}
					Torrents.RemoveAll(i => i.InfoHash.ToHex() == meta.TorrentHash);
				}
				else
					return;
					//return null;
			}

			TorrentsMetadata.Add(meta);
			SaveData();

			if (FastResumeData.ContainsKey(meta.TorrentHash))
				manager.LoadFastResume(new FastResume((BEncodedDictionary)FastResumeData[torrent.InfoHash.ToHex()]));

			await Engine.Register(manager);

			Torrents.Add(manager);

			await manager.StartAsync();

			//return manager;
		}

		public static async void Restore()
		{
			await Engine.EnablePortForwardingAsync(CancellationToken.None);
			TorrentsMetadata = JsonConvert.DeserializeObject<List<TorrentMetadataInfo>>(LocalSettings.Values["TorrentsMetadata"] as string ?? "") ?? new List<TorrentMetadataInfo>();

			try
			{
				StorageFile fastResumeFile = await LocalStorage.CreateFileAsync("TorrentFastResume.data", CreationCollisionOption.OpenIfExists);
				FastResumeData = BEncodedValue.Decode<BEncodedDictionary>(await fastResumeFile.OpenStreamForReadAsync());
			}
			catch { }

			foreach (TorrentMetadataInfo meta in TorrentsMetadata)
				RestoreTorrent(meta);
		}

		private static async void RestoreTorrent(TorrentMetadataInfo meta)
		{
			try
			{
				StorageFile torrentFile = await LocalStorage.GetFileAsync(meta.TorrentFileName);
				Torrent torrent = await Torrent.LoadAsync(await torrentFile.OpenStreamForReadAsync());
				StorageFolder destinationFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(meta.DestinationFolderFALToken);

				TorrentManager manager = new TorrentManager(torrent, destinationFolder.Path);

				if (FastResumeData.ContainsKey(meta.TorrentHash))
					manager.LoadFastResume(new FastResume((BEncodedDictionary)FastResumeData[meta.TorrentHash]));

				await Engine.Register(manager);

				Torrents.Add(manager);

				if (meta.IsActive)
					await manager.StartAsync();
			}
			catch
			{
				TorrentsMetadata.Remove(meta);
				SaveData();
			}
		}

		public static async Task Suspend()
		{
			SaveData();

			BEncodedDictionary fastResume = new BEncodedDictionary();
			foreach (TorrentManager item in Torrents)
			{
				await item.StopAsync();
				if (item.HashChecked)
					fastResume.Add(item.InfoHash.ToHex(), item.SaveFastResume().Encode());
			}

			StorageFile fastResumeFile = await LocalStorage.CreateFileAsync("TorrentFastResume.data", CreationCollisionOption.OpenIfExists);
			while (true)
				try
				{
					await File.WriteAllBytesAsync(fastResumeFile.Path, fastResume.Encode());
					break;
				}
				catch { }

			Engine.Dispose();
		}

		private static void SaveData()
		{
			foreach (TorrentManager item in Torrents)
				TorrentsMetadata.Find(i => i.TorrentHash == item.InfoHash.ToHex()).IsActive = !item.State.BelongsTo(TorrentState.Paused, TorrentState.HashingPaused, TorrentState.Error, TorrentState.Stopped, TorrentState.Stopping);
			LocalSettings.Values["TorrentsMetadata"] = JsonConvert.SerializeObject(TorrentsMetadata);
		}

		public static async void PauseTorrent(TorrentManager torrent)
		{
			await torrent.PauseAsync();
			if (!torrent.HashChecked)
				return;

			if (FastResumeData.ContainsKey(torrent.InfoHash.ToHex()))
				FastResumeData[torrent.InfoHash.ToHex()] = torrent.SaveFastResume().Encode();
			else
				FastResumeData.Add(torrent.InfoHash.ToHex(), torrent.SaveFastResume().Encode());

			try
			{
				StorageFile fastResumeFile = await LocalStorage.CreateFileAsync("TorrentFastResume.data", CreationCollisionOption.OpenIfExists);
				await File.WriteAllBytesAsync(fastResumeFile.Path, FastResumeData.Encode());
			}
			catch { }
		}

		public static async void RemoveTorrent(TorrentManager torrent, bool removeFiles = false)
		{
			await torrent.StopAsync();
			TorrentMetadataInfo meta = TorrentsMetadata.Find(i => i.TorrentHash == torrent.InfoHash.ToHex());

			if (removeFiles)
			{
				StorageFolder destinationFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(meta.DestinationFolderFALToken);
				foreach (TorrentFile file in torrent.Torrent.Files)
					await (await destinationFolder.GetFileAsync(file.Path)).DeleteAsync(StorageDeleteOption.PermanentDelete);
			}

			StorageApplicationPermissions.FutureAccessList.Remove(meta.DestinationFolderFALToken);
			await (await LocalStorage.GetFileAsync(meta.TorrentFileName))?.DeleteAsync(StorageDeleteOption.PermanentDelete);

			FastResumeData.Remove(meta.TorrentHash);

			TorrentsMetadata.RemoveAll(i => i.TorrentHash == torrent.InfoHash.ToHex());

			SaveData();

			try
			{
				StorageFile fastResumeFile = await LocalStorage.CreateFileAsync("TorrentFastResume.data", CreationCollisionOption.OpenIfExists);
				await File.WriteAllBytesAsync(fastResumeFile.Path, FastResumeData.Encode());
			}
			catch { }
		}
	}
}