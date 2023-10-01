using UnityEngine.Events;

namespace Scripts.Reactive
{
	public class UnityEventObserver : IObserver
	{
		private readonly UnityEvent _unityEvent;
		private readonly UnityAction _action;

		public UnityEventObserver(UnityEvent unityEvent, UnityAction action)
		{
			_unityEvent = unityEvent;
			_action = action;
		}

		public void Subscribe()
		{
			_unityEvent.AddListener(_action);
		}

		public void OnNotify()
		{
			_action.Invoke();
		}

		public void Dispose()
		{
			_unityEvent.RemoveListener(_action);
		}
	}
}