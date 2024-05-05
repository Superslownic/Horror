using System;
using Cysharp.Threading.Tasks;
using Scripts.Reactive;

namespace Scripts.Initialization
{
	public interface IInitializationManager
	{
		void AddTask(Func<UniTask> task);
		void InsertTask(int index, Func<UniTask> task);
		UniTask Execute();
		DisposableAction<float> OnProgressChanged { get; }
	}
}