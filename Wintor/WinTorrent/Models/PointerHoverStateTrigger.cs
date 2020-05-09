using MonoTorrent.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinTorrent.Pages;

namespace WinTorrent.Models
{
	class PointerHoverStateTrigger : StateTriggerBase
	{
		public TorrentManager TargetItem
		{
			get => _targetItem;
			set
			{
				if (_targetItem == value)
					return;

				if (_targetElement != null)
				{
					_targetElement.PointerEntered -= PointerEntered;
					_targetElement.PointerExited -= PointerExited;
				}

				_targetItem = value;
				_targetElement = ((Window.Current.Content as Frame).Content as MainPage).list.ContainerFromItem(_targetItem) as FrameworkElement;
				_targetElement.PointerEntered += PointerEntered;
				_targetElement.PointerExited += PointerExited;
			}
		}
		private TorrentManager _targetItem;
		private FrameworkElement _targetElement;

		private void PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) =>
			SetActive(false);

		private void PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) =>
			SetActive(true);
	}
}
