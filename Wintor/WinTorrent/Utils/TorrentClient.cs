using System;
using System.Collections.Generic;
using WinTorrent.Models;

namespace WinTorrent.Utils
{
	public delegate void TorrentStateChangedEventHandler(TorrentItem sender, TorrentState previousState);

	public static class TorrentClient
	{
		public static event TorrentStateChangedEventHandler TorrentItemStateChanged;

		public static List<TorrentItem> Torrents { get; }

		static TorrentClient()
		{
			Torrents = new List<TorrentItem>
			{
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Downloading,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Completed,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Paused,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Seeding,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Cancelling,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Initializing,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Pausing,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Resuming,
					SeedCount = 15
				},
				new TorrentItem
				{
					Title = "Microsoft Edge",
					TotalSize = new DataSize(3221225472),
					Downloaded = new DataSize(125829120),
					TransmissionSpeed = new DownloadSpeed(33554432),
					RemainingTime = TimeSpan.FromSeconds(36),
					State = TorrentState.Error,
					SeedCount = 15
				}
			};
		}

		public static void AddTorrent()
		{

		}

		public static void PauseTorrent()
		{

		}

		public static void CancelTorrent()
		{

		}

		public static void ResumeTorrent()
		{

		}

		public static void SeedTorrent()
		{

		}

		public static void OnItemStateChanged(TorrentItem item, TorrentState previousState) =>
			TorrentItemStateChanged?.Invoke(item, previousState);
	}
}