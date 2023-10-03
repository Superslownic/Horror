using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Factory
{
	public interface IObjectFactory
	{
		T CreateInjectedInstance<T>(params object[] extraArgs);
		T CreateInjectedInstance<T>(Type type, params object[] extraArgs);
		void InjectInstance<T>(T instance, params object[] extraArgs);
		T Instantiate<T>(T prefab, Transform parent, params object[] extraArgs) where T : Object;
		T Instantiate<T>(T prefab, Transform parent, Vector3 position, params object[] extraArgs) where T : Object;
		T InstantiatePersistent<T>(T prefab, Transform parent = null, params object[] extraArgs) where T : Object;
		void InjectGameObject(GameObject gameObject);
	}
}