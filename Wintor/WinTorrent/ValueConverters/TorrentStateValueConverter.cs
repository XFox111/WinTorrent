using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using WinTorrent.Models;

namespace WinTorrent.ValueConverters
{
	public class TorrentStateValueConverter : IValueConverter
	{
		private static Dictionary<string, TorrentStateValuesEntry[]> StateDictionary { get; } = new Dictionary<string, TorrentStateValuesEntry[]>
		{
			{
				"AccentColor",
				new []
				{
					new TorrentStateValuesEntry(new SolidColorBrush(Colors.DeepSkyBlue),
												TorrentState.Downloading),
					new TorrentStateValuesEntry(new SolidColorBrush(Colors.Orange),
												TorrentState.Hashing,
												TorrentState.Metadata,
												TorrentState.Starting,
												TorrentState.Stopping),
					new TorrentStateValuesEntry(new SolidColorBrush(Colors.Gray),
												TorrentState.HashingPaused,
												TorrentState.Paused,
												TorrentState.Stopped),
					new TorrentStateValuesEntry(new SolidColorBrush(Color.FromArgb(255, 85, 187, 85)),
												TorrentState.Seeding),
					new TorrentStateValuesEntry(new SolidColorBrush(Colors.Red),
												TorrentState.Error),
				}
			},
			{
				"ProgressIndeterminate",
				new [] { new TorrentStateValuesEntry(true,
													TorrentState.Stopping,
													TorrentState.Hashing,
													TorrentState.Metadata,
													TorrentState.Starting) }
			},
			{
				"StatsVisibility",
				new [] { new TorrentStateValuesEntry(Visibility.Visible, TorrentState.Downloading) }
			},
			{
				"ErrorVisibility",
				new [] { new TorrentStateValuesEntry(Visibility.Visible, TorrentState.Error) }
			},
			{
				"ProgressValue",
				new [] { new TorrentStateValuesEntry(1, TorrentState.Error,
														TorrentState.Seeding,
														TorrentState.Stopped) }
			},
			{
				"PauseButtonVisibility",
				new [] { new TorrentStateValuesEntry(Visibility.Visible,
													TorrentState.Downloading,
													TorrentState.Error,
													TorrentState.Paused,
													TorrentState.HashingPaused,
													TorrentState.Stopped) }
			},
			{
				"PauseButtonTooltip",
				new [] 
				{
					new TorrentStateValuesEntry("Pause", TorrentState.Downloading),
					new TorrentStateValuesEntry("Retry", TorrentState.Error),
					new TorrentStateValuesEntry("Resume", TorrentState.Paused, TorrentState.HashingPaused),
					new TorrentStateValuesEntry("Start", TorrentState.Stopped)
				}
			},
			{
				"PauseButtonIcon",
				new [] 
				{
					new TorrentStateValuesEntry("\xE103", TorrentState.Downloading),
					new TorrentStateValuesEntry("\xE149", TorrentState.Error),
					new TorrentStateValuesEntry("\xE102", TorrentState.Paused,
														  TorrentState.HashingPaused,
														  TorrentState.Stopped)
				}
			},
			{
				"StatusLabel",
				new []
				{
					new TorrentStateValuesEntry("Downloading", TorrentState.Downloading),
					new TorrentStateValuesEntry("Error", TorrentState.Error),
					new TorrentStateValuesEntry("Hashing", TorrentState.Hashing),
					new TorrentStateValuesEntry("Hashing paused", TorrentState.HashingPaused),
					new TorrentStateValuesEntry("Metadata", TorrentState.Metadata),
					new TorrentStateValuesEntry("Paused", TorrentState.Paused),
					new TorrentStateValuesEntry("Seeding", TorrentState.Seeding),
					new TorrentStateValuesEntry("Starting", TorrentState.Starting),
					new TorrentStateValuesEntry("Stopped", TorrentState.Stopped),
					new TorrentStateValuesEntry("Stopping", TorrentState.Stopping),
				}
			},
			{
				"StatusIcon",
				new []
				{
					new TorrentStateValuesEntry("\xE121",
												TorrentState.Stopping,
												TorrentState.Starting,
												TorrentState.Metadata,
												TorrentState.Hashing),
					new TorrentStateValuesEntry("\xE103",
												TorrentState.HashingPaused,
												TorrentState.Paused),
					new TorrentStateValuesEntry("\xE118",
												TorrentState.Downloading),
					new TorrentStateValuesEntry("\xE11C",
												TorrentState.Seeding),
					new TorrentStateValuesEntry("\xE7BA",
												TorrentState.Error),
					new TorrentStateValuesEntry("\xE15B",
												TorrentState.Stopped)
				}
			}
		};

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			try { return StateDictionary[parameter as string].FirstOrDefault(i => i.AppliedTo.Contains((TorrentState)value))?.Value; }
			catch { return value; }
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) =>
			null;
	}
}
