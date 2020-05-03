using System;
using System.Collections.Generic;

namespace WinTorrent.Models
{
	public struct DataSize : IComparable<DataSize>, IEquatable<DataSize>, IComparer<DataSize>
	{
		public long ByteSize { get; set; }

		public DataSize(long sizeInBytes) =>
			ByteSize = sizeInBytes;

		public override string ToString() =>
			$"{ByteSize} B";

		public int CompareTo(DataSize other) =>
			(int)Math.Clamp(ByteSize - other.ByteSize, -1, 1);

		public bool Equals(DataSize other) =>
			ByteSize == other.ByteSize;

		public int Compare(DataSize x, DataSize y) =>
			(int)Math.Clamp(x.ByteSize - y.ByteSize, -1, 1);

		public static double operator /(DataSize s1, DataSize s2) =>
			s1.ByteSize / s2.ByteSize;
		public static bool operator ==(DataSize s1, DataSize s2) =>
			s1.ByteSize == s2.ByteSize;
		public static bool operator !=(DataSize s1, DataSize s2) =>
			s1.ByteSize != s2.ByteSize;
		public static bool operator ==(DataSize s1, int s2) =>
			s1.ByteSize == s2;
		public static bool operator !=(DataSize s1, int s2) =>
			s1.ByteSize != s2;
	}

	public struct DownloadSpeed : IComparable<DownloadSpeed>, IEquatable<DownloadSpeed>, IComparer<DownloadSpeed>
	{
		public long BytesPerSecond { get; set; }

		public DownloadSpeed(long bps) =>
			BytesPerSecond = bps;

		public override string ToString() =>
			$"{BytesPerSecond} B/s";

		public int CompareTo(DownloadSpeed other) =>
			(int)Math.Clamp(BytesPerSecond - other.BytesPerSecond, -1, 1);

		public bool Equals(DownloadSpeed other) =>
			BytesPerSecond == other.BytesPerSecond;

		public int Compare(DownloadSpeed x, DownloadSpeed y) =>
			(int)Math.Clamp(x.BytesPerSecond - y.BytesPerSecond, -1, 1);

		public static double operator /(DownloadSpeed s1, DownloadSpeed s2) =>
			s1.BytesPerSecond / s2.BytesPerSecond;
		public static bool operator ==(DownloadSpeed s1, DownloadSpeed s2) =>
			s1.BytesPerSecond == s2.BytesPerSecond;
		public static bool operator !=(DownloadSpeed s1, DownloadSpeed s2) =>
			s1.BytesPerSecond != s2.BytesPerSecond;
	}
}
