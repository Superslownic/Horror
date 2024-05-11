namespace Scripts.Reactive
{
	public class ObservableTrigger
	{
		public ObservableTrigger(IObservable observable)
		{
			observable.AddListener(() => _wasInvoked = true);
		}

		public bool IsChecked
		{
			get
			{
				if (!_wasInvoked)
					return false;

				_wasInvoked = false;
				return true;
			}
		}

		private bool _wasInvoked;
	}
}