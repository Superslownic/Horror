using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Reactive
{
	public static class ObservableExtensions
	{
		public static IDisposable AddListener(this Button button, UnityAction action)
		{
			var observer = new UnityEventObserver(button.onClick, action);
			observer.Subscribe();
			return observer;
		}
		
		public static IObserver AddListener(this IObservable observable, Action action)
		{
			var observer = new ActionObserver(observable, action);
			observable.AddListener(observer);
			return observer;
		}
		
		public static IObserver<T> AddListener<T>(this IObservable<T> observable, Action<T> action)
		{
			var observer = new ActionObserver<T>(observable, action);
			observable.AddListener(observer);
			return observer;
		}

		public static IObserver WithInvoke(this IObserver observer)
		{
			observer.OnNotify();
			return observer;
		}
		
		public static IObserver<T> WithInvoke<T>(this IObserver<T> observer, T value)
		{
			observer.OnNotify(value);
			return observer;
		}

		public static void AddTo(this IDisposable disposable, IGroupedDisposable groupedDisposable)
		{
			groupedDisposable.Add(disposable);
		}
	}
}