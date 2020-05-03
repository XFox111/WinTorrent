using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.Storage;

namespace WinTorrent.Utils
{
	public static class Metrics
	{
		static readonly ApplicationDataContainer storage = ApplicationData.Current.RoamingSettings;

		static readonly Stopwatch sw = new Stopwatch();
		public static TimeSpan Uptime
		{
			get => (TimeSpan?)storage.Values["Metrics.SpentTime"] ?? TimeSpan.FromSeconds(0);
			set => storage.Values["Metrics.SpentTime"] = value;
		}
		public static string CurrentVersion
		{
			get
			{
				PackageVersion v = Package.Current.Id.Version;
				return $"{v.Major}.{v.Minor}.{v.Revision}.{v.Build}";
			}
		}

		public static void StartSession()
		{
			sw.Start();
			AppCenter.Start("45774462-9ea7-438a-96fc-03982666f39e", typeof(Analytics), typeof(Crashes));
			AppCenter.SetCountryCode(new RegionInfo(CultureInfo.CurrentUICulture.Name).TwoLetterISORegionName);
		}

		public static void EndSession()
		{
			sw.Stop();
			Uptime += sw.Elapsed;

			AddEvent("Session closed",
				("Duration", sw.Elapsed.ToString()),
				("Spend time total", Uptime.ToString()));
		}

		public static void AddEvent(string eventName, params (string key, string value)[] details)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			foreach (var (key, value) in details)
				parameters.Add(key, value);
			Analytics.TrackEvent(eventName, parameters.Count > 0 ? parameters : null);
		}
	}
}