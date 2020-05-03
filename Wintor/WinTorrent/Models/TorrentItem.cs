using System;
using System.ComponentModel;
using Windows.Storage;
using WinTorrent.Utils;

namespace WinTorrent.Models
{
	public enum TorrentState
	{ 
		Initializing = 0,
		Downloading = 1,
		Cancelling = 2,
		Pausing = 3,
		Paused = 4,
		Resuming = 5,
		Completed = 6,
		Seeding = 7,
		Error = 8
	}

	public class TorrentItem : INotifyPropertyChanged
	{
		public event TorrentStateChangedEventHandler StateChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public string Title
		{
			get => _title;
			set
			{
				if (_title == value)
					return;

				_title = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}
		private string _title;

		public TorrentState State
		{
			get => _state;
			set
			{
				if (_state == value)
					return;

				TorrentState previousState = _state;
				_state = value;
				StateChanged?.Invoke(this, previousState);
				TorrentClient.OnItemStateChanged(this, previousState);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
			}
		}
		private TorrentState _state = TorrentState.Initializing;

		public DownloadSpeed TransmissionSpeed
		{
			get => _transmissionSpeed;
			set
			{
				if (_transmissionSpeed == value)
					return;

				_transmissionSpeed = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransmissionSpeed)));
			}
		}
		private DownloadSpeed _transmissionSpeed;

		public DataSize Downloaded
		{
			get => _downloaded;
			set
			{
				if (_downloaded == value)
					return;

				_downloaded = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Downloaded)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
			}
		}
		private DataSize _downloaded;

		public DataSize TotalSize
		{
			get => _totalSize;
			set
			{
				if (_totalSize == value)
					return;

				_totalSize = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalSize)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
			}
		}
		private DataSize _totalSize;

		public double Progress => TotalSize != 0 ? ((double)Downloaded.ByteSize / TotalSize.ByteSize) : 0;

		public int SeedCount
		{
			get => _seedCount;
			set
			{
				if (_seedCount == value)
					return;

				_seedCount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeedCount)));
			}
		}
		private int _seedCount;

		public TimeSpan RemainingTime
		{
			get => _remainingTime;
			set
			{
				if (_remainingTime == value)
					return;

				_remainingTime = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeedCount)));
			}
		}
		private TimeSpan _remainingTime;

		public IStorageItem Files { get; set; }
	}
}
