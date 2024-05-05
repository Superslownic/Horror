using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Reactive;
using UnityEngine;
using Zenject;

namespace Scripts.Initialization
{
	public class InitializationManager : IInitializationManager, IInitializable
	{
		public DisposableAction<float> OnProgressChanged { get; } = new();
		public IProgress<float> LocalProgressProvider { get; private set; }

		[Inject] private readonly IEnumerable<IInitializableBase> _initializables;

		private readonly List<Func<UniTask>> _tasks = new();

		private int _completedTaskCount;
		private float _globalProgress;
		private float _localProgress;

		public void Initialize()
		{
			LocalProgressProvider = Cysharp.Threading.Tasks.Progress.Create<float>(HandleLocalProgressChanged);

			foreach (IInitializableBase initializable in _initializables)
			{
				switch (initializable)
				{
					case IInitializableAsync initializableAsync:
						_tasks.Add(() => initializableAsync.Initialize(LocalProgressProvider));
						break;
					
					case IInitializableSync initializableSync:
						_tasks.Add(async () => initializableSync.Initialize());
						break;
				}
			}
		}

		public void AddTask(Func<UniTask> task)
		{
			_tasks.Add(task);
		}

		public void InsertTask(int index, Func<UniTask> task)
		{
			_tasks.Insert(index, task);
		}

		public async UniTask Execute()
		{
			_completedTaskCount = 0;
			NotifyProgressChanged();
			
			foreach (Func<UniTask> task in _tasks)
			{
				await task.Invoke();
				_completedTaskCount++;
				_globalProgress = (float)_completedTaskCount / _tasks.Count;
				NotifyProgressChanged();
			}
		}

		private void HandleLocalProgressChanged(float value)
		{
			if(_localProgress < value)
			{
				_localProgress = value;
				NotifyProgressChanged();
			}
		}

		private void NotifyProgressChanged()
		{
			OnProgressChanged.Invoke(Mathf.Clamp01(_globalProgress + _localProgress * (1f / _tasks.Count)));
		}
	}
}