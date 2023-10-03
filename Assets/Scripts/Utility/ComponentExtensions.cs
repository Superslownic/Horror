using UnityEngine;

namespace Scripts.Utility
{
	public static class ComponentExtensions
	{
		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.TryGetComponent(out T result);
		}
		
		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			return component.TryGetComponent(out T result)
				? result
				: component.gameObject.AddComponent<T>();
		}
	}
}