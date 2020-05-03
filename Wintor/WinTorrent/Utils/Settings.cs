using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace WinTorrent.Utils
{
	public static class Settings
	{
		static readonly ApplicationDataContainer settings = ApplicationData.Current.RoamingSettings;

		public static bool Startup
		{
			get => (bool?)settings.Values["Startup"] ?? false;
			set => settings.Values["Startup"] = value;
		}
		public static bool RunOnSaver
		{
			get => (bool?)settings.Values["RunOnSaver"] ?? false;
			set => settings.Values["RunOnSaver"] = value;
		}

		public static bool SeedCompleted
		{
			get => (bool?)settings.Values["SeedCompleted"] ?? true;
			set => settings.Values["SeedCompleted"] = value;
		}
		public static string DefaultFolder
		{
			get => (string)settings.Values["DefaultFolder"] ?? "";
			set => settings.Values["DefaultFolder"] = value;
		}

		public static int DownloadLimit
		{
			get => (int?)settings.Values["DownloadLimit"] ?? 0;
			set => settings.Values["DownloadLimit"] = value;
		}
		public static int UploadLimit
		{
			get => (int?)settings.Values["UploadLimit"] ?? 0;
			set => settings.Values["UploadLimit"] = value;
		}

		public static bool PromptReview
		{
			get => (bool?)settings.Values["PromptReview"] ?? Metrics.Uptime.TotalHours > 24;
			set => settings.Values["PromptReview"] = value;
		}
		public static bool PromptFeedback
		{
			get => (bool?)settings.Values["PromptFeedback"] ?? Metrics.Uptime.TotalHours > 12;
			set => settings.Values["PromptFeedback"] = value;
		}
		public static int Theme
		{
			get => (int?)settings.Values["PreferedUITheme"] ?? 2;  //System
			set => settings.Values["PreferedUITheme"] = value;
		}

		public static async Task<StorageFolder> GetDefaultFolder()
		{
			if (string.IsNullOrWhiteSpace(DefaultFolder))
				return await DownloadsFolder.CreateFolderAsync(Package.Current.DisplayName, CreationCollisionOption.OpenIfExists);
			else
				return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(DefaultFolder);
		}
	}
}