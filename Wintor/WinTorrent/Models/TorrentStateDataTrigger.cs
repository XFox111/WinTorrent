using Windows.UI.Xaml;

namespace WinTorrent.Models
{
	public class TorrentStateDataTrigger : StateTriggerBase
	{
		public TorrentState TargetState
		{
			get => _targetState;
			set
			{
				if (_targetState == value)
					return;

				_targetState = value;
				UpdateTrigger();
			}
		}
		private TorrentState _targetState;

		public TorrentItem TargetElement
		{
			get => _targetElement;
			set
			{
				if (_targetElement == value)
					return;

				if (_targetElement != null)
					_targetElement.StateChanged -= TargetElementStateChanged;

				_targetElement = value;
				_targetElement.StateChanged += TargetElementStateChanged;

				UpdateTrigger();
			}
		}
		private TorrentItem _targetElement;

		private void UpdateTrigger() =>
			SetActive(TargetElement?.State == TargetState);

		private void TargetElementStateChanged(TorrentItem sender, TorrentState previousState) =>
			UpdateTrigger();
	}
}
