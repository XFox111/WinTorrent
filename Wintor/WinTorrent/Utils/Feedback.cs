using Microsoft.Services.Store.Engagement;
using System;
using Windows.Services.Store;
using Windows.UI.Xaml.Controls;

namespace WinTorrent.Utils
{
	public static class Feedback
	{
		public static bool HasFeedbackHub => StoreServicesFeedbackLauncher.IsSupported();

		public static async void OpenFeedbackHub()
		{
			if (HasFeedbackHub)
				await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
		}

		public static async void PromptFeedback()
		{
			if (!HasFeedbackHub)
			{
				Settings.PromptFeedback = false;
				return;
			}

			ContentDialog dialog = new ContentDialog
			{
				Title = "Have some thoughts?",

				PrimaryButtonText = "Sure!",
				SecondaryButtonText = "Don't ask me anymore",
				CloseButtonText = "Maybe later",

				DefaultButton = ContentDialogButton.Primary,

				Content = new TextBlock
				{
					Text = "Would you like to share something you like or dislike in the app? Or perhaps you have some ideas? Leave feedback!"
				}
			};

			ContentDialogResult result = await dialog.ShowAsync();

			if (result != ContentDialogResult.None)
				Settings.PromptFeedback = false;

			if (result == ContentDialogResult.Primary)
				OpenFeedbackHub();
		}

		public static async void PromptReview()
		{
			ContentDialog dialog = new ContentDialog
			{
				Title = "Like our app?",

				PrimaryButtonText = "Sure!",
				SecondaryButtonText = "Don't ask me anymore",
				CloseButtonText = "Maybe later",

				DefaultButton = ContentDialogButton.Primary,

				Content = new TextBlock
				{
					Text = "Could you leave a feedback on Microsfot Store page? It's very important for me :)"
				}
			};

			ContentDialogResult result = await dialog.ShowAsync();

			if (result != ContentDialogResult.None)
				Settings.PromptReview = false;

			if (result == ContentDialogResult.Primary)
				await StoreContext.GetDefault().RequestRateAndReviewAppAsync();
		}
	}
}