using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Factory
{
	public class ObjectFactory
	{
		[Inject] private readonly DiContainer _diContainer;

		public T CreateInjectedInstance<T>(params object[] extraArgs)
		{
			return _diContainer.Instantiate<T>(extraArgs);
		}
		
		public T CreateInjectedInstance<T>(Type type, params object[] extraArgs)
		{
			return (T) _diContainer.Instantiate(type, extraArgs);
		}

		public void InjectInstance<T>(T instance, params object[] extraArgs)
		{
			_diContainer.Inject(instance, extraArgs);
		}
		
		public T Instantiate<T>(T prefab, Transform parent = null, params object[] extraArgs) where T : Object
		{
			return _diContainer.InstantiatePrefabForComponent<T>(prefab, parent, extraArgs);
		}
		
		public T Instantiate<T>(T prefab, Transform parent, Vector3 position, params object[] extraArgs) where T : Object
		{
			return _diContainer.InstantiatePrefabForComponent<T>(prefab, position, Quaternion.identity, parent, extraArgs);
		}
		
		public T InstantiatePersistent<T>(T prefab, Transform parent = null, params object[] extraArgs) where T : Object
		{
			var instance = _diContainer.InstantiatePrefabForComponent<T>(prefab, parent, extraArgs);
			Object.DontDestroyOnLoad(instance);
			return instance;
		}

		public void InjectGameObject(GameObject gameObject)
		{
			_diContainer.InjectGameObject(gameObject);
		}

		public void Destroy(Object obj)
		{
			if(obj == null)
				return;
			
			Object.Destroy(obj);
		}
	}
}