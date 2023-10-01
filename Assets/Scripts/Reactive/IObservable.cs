namespace Scripts.Reactive
{
	public interface IObservable<T>
	{
		void AddListener(IObserver<T> observer);
		void RemoveListener(IObserver<T> observer);
		void Invoke(T payload);
	}

	public interface IObservable
	{
		void AddListener(IObserver observer);
		void RemoveListener(IObserver observer);
		void Invoke();
	}
}