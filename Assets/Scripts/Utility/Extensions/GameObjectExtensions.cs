using Scripts.Core;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Utility.Extensions
{
	public static class GameObjectExtensions
	{
		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.TryGetComponent(out T result);
		}
		
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.TryGetComponent(out T result)
				? result
				: gameObject.gameObject.AddComponent<T>();
		}
		
		public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T result) where T : Component
		{
			result = gameObject.GetComponentInChildren<T>();
			return result != null;
		}

		public static bool TryGetUnit(this GameObject gameObject, out Unit unit)
		{
			if (gameObject.TryGetComponent(out unit))
				return true;

			if (!gameObject.TryGetComponent(out UnitLink unitLink))
				return false;

			unit = unitLink.Unit;
			return true;
		}
	}
}