using System.Collections.Generic;
using Scripts.Core;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Utility.Extensions
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
		
		public static bool TryGetComponentInChildren<T>(this Component component, out T result) where T : Component
		{
			result = component.GetComponentInChildren<T>();
			return result != null;
		}
		
		public static List<T> GetComponentsInClosestChildren<T>(this Component component)
		{
			List<T> list = new List<T>();

			for (int i = 0; i < component.transform.childCount; i++)
				if (component.transform.GetChild(i).TryGetComponent(out T c))
					list.Add(c);

			return list;
		}

		public static bool TryGetUnit(this Component component, out Unit unit)
		{
			if (component.TryGetComponent(out unit))
				return true;

			if (!component.TryGetComponent(out UnitLink unitLink))
				return false;

			unit = unitLink.Unit;
			return true;
		}
	}
}