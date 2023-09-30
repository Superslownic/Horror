using System;

namespace Game.Reactive
{
	public interface IObserver<in T> : IDisposable
	{
		void OnNotify(T payload);
	}

	public interface IObserver : IDisposable
	{
		void OnNotify();
	}
}