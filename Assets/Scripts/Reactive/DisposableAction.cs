using System.Collections.Generic;

namespace Game.Reactive
{
	public class DisposableAction : IObservable
	{
		private readonly LinkedList<IObserver> _observers = new();

		public void AddListener(IObserver observer)
		{
			_observers.AddLast(observer);
		}

		public void RemoveListener(IObserver observer)
		{
			_observers.Remove(observer);
		}

		public void Invoke()
		{
			var node = _observers.First;
			
			while (node != null)
			{
				node.Value.OnNotify();
				node = node.Next;
			}
		}
	}

	public class DisposableAction<T> : IObservable<T>
	{
		private readonly LinkedList<IObserver<T>> _observers = new();

		public void AddListener(IObserver<T> observer)
		{
			_observers.AddLast(observer);
		}

		public void RemoveListener(IObserver<T> observer)
		{
			_observers.Remove(observer);
		}

		public void Invoke(T payload)
		{
			var node = _observers.First;

			while (node != null)
			{
				node.Value.OnNotify(payload);
				node = node.Next;
			}
		}
	}
}