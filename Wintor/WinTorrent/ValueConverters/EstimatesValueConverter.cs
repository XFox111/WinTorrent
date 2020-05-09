using MonoTorrent.Client;
using System;
using Windows.UI.Xaml.Data;

namespace WinTorrent.ValueConverters
{
	public class EstimatesValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			TorrentManager item = value as TorrentManager;
			try
			{
				return TimeSpan.FromSeconds((item.Torrent.Size - item.Monitor.DataBytesDownloaded) / item.Monitor.DownloadSpeed);
			}
			catch(DivideByZeroException)
			{
				return "";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) =>
			null;
	}
}
