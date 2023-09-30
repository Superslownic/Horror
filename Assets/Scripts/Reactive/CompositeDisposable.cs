using System;
using System.Collections.Generic;

namespace Game.Reactive
{
	public class CompositeDisposable : IGroupedDisposable, IDisposable
	{
		private readonly List<IDisposable> _list = new();

		public void Add(IDisposable disposable)
		{
			_list.Add(disposable);
		}

		public void Dispose()
		{
			foreach (IDisposable disposable in _list)
				disposable.Dispose();
			
			_list.Clear();
		}
	}
}