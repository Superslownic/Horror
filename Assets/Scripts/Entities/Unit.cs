using System;
using System.Collections.Generic;
using Scripts.Reactive;
using Scripts.Reflection;
using Scripts.Utility;
using UnityEngine;

namespace Scripts.Entities
{
	public class Unit : MonoBehaviour
	{
		public static DisposableAction<Unit> OnInitialized { get; } = new();
		public static DisposableAction<Unit> OnDisposed { get; } = new();
		
		private Dictionary<Type, Ability> _abilities = new();

		private void Awake()
		{
			GatherAbilities(transform);
			InitializeAbilities();
			OnInitialized.Invoke(this);
		}

		private void OnDestroy()
		{
			OnDisposed.Invoke(this);
		}

		public bool HasAbility<T>() where T : Ability
		{
			return _abilities.ContainsKey(TypeCache<T>.Value);
		}

		public T GetAbility<T>() where T : Ability
		{
			return (T)_abilities[TypeCache<T>.Value];
		}

		public bool TryGetAbility<T>(out T result) where T : Ability
		{
			if (_abilities.TryGetValue(TypeCache<T>.Value, out Ability ability))
			{
				result = (T)ability;
				return true;
			}
			
			result = default;
			return false;
		}

		private void GatherAbilities(Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				
				if (child.HasComponent<Unit>())
					continue;

				if (child.TryGetComponent(out Ability ability))
					_abilities.Add(ability.GetType(), ability);
				
				GatherAbilities(child);
			}
		}

		private void InitializeAbilities()
		{
			foreach (Ability ability in _abilities.Values)
			{
				ability.Initialize(this);
					
				switch (ability)
				{
					case IActivatableAbility:
						ability.gameObject.AddComponent<Activator>();
						break;
						
					case IDeactivatableAbility:
						ability.gameObject.AddComponent<Deactivator>();
						break;
						
					default:
						ability.SetActive(true);
						break;
				}
			}
		}
	}
}