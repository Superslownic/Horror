using System;

namespace Scripts.Reactive
{
	public abstract class Observer<T> : IObserver<T>
	{
		private readonly IObservable<T> _observable;

		protected Observer(IObservable<T> observable)
		{
			_observable = observable;
		}

		public void Dispose()
		{
			_observable.RemoveListener(this);
		}

		public abstract void OnNotify(T payload);
	}

	public abstract class Observer : IObserver, IDisposable
	{
		private readonly IObservable _observable;

		protected Observer(IObservable observable)
		{
			_observable = observable;
		}

		public void Dispose()
		{
			_observable.RemoveListener(this);
		}

		public abstract void OnNotify();
	}
}