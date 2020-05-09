using MonoTorrent.Client;

namespace WinTorrent.Models
{
	public class TorrentStateValuesEntry
	{
		public object Value { get; set; }
		public TorrentState[] AppliedTo { get; set; }

		public TorrentStateValuesEntry(object value, params TorrentState[] states)
		{
			Value = value;
			AppliedTo = states;
		}
	}
}
