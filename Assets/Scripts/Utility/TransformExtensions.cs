using UnityEngine;

namespace Game.Utility
{
	public static class TransformExtensions
	{
		public static bool HasComponent<T>(this Transform transform)
		{
			return transform.TryGetComponent(out T result);
		}
	}
}