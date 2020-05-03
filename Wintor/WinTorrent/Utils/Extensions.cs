using System;
using System.Linq;

namespace WinTorrent
{
	public static class Extensions
	{
		public static bool BelongsTo<T>(this T obj, params T[] set) =>
			set.Contains(obj);

		public static Uri ToUri(this string str)
		{
			Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out Uri uri);
			return uri;
		}
	}
}
