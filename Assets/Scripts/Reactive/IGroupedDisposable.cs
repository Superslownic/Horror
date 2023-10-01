using System;

namespace Scripts.Reactive
{
	public interface IGroupedDisposable
	{
		void Add(IDisposable disposable);
	}
}