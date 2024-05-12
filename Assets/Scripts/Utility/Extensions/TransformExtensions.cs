using UnityEngine;

namespace Scripts.Utility.Extensions
{
	public static class TransformExtensions
	{
		public static bool HasComponent<T>(this Transform transform) where T : Component
		{
			return transform.TryGetComponent(out T result);
		}

		public static Vector3 LerpPosition(this Transform transform, Vector3 target, float delta)
		{
			return transform.position = Vector3.Lerp(transform.position, target, delta);
		}
	}
}