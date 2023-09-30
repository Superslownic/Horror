using System;

namespace Game.Reactive
{
	public interface IGroupedDisposable
	{
		void Add(IDisposable disposable);
	}
}