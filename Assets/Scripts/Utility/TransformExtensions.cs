using UnityEngine;

namespace Scripts.Utility
{
	public static class TransformExtensions
	{
		public static bool HasComponent<T>(this Transform transform) where T : Component
		{
			return transform.TryGetComponent(out T result);
		}
	}
}