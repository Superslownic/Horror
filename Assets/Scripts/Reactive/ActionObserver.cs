using System;

namespace Game.Reactive
{
	public class ActionObserver : Observer
	{
		private readonly Action _action;

		public ActionObserver(IObservable observable, Action action) : base(observable)
		{
			_action = action;
		}

		public override void OnNotify()
		{
			_action.Invoke();
		}
	}
	
	public class ActionObserver<T> : Observer<T>
	{
		private readonly Action<T> _action;

		public ActionObserver(IObservable<T> observable, Action<T> action) : base(observable)
		{
			_action = action;
		}

		public override void OnNotify(T payload)
		{
			_action.Invoke(payload);
		}
	}
}