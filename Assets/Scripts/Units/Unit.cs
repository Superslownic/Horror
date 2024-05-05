using System;
using System.Collections.Generic;
using Scripts.Reactive;
using Scripts.Reflection;
using Scripts.Utility.Extensions;
using UnityEngine;

namespace Scripts.Units
{
	[DefaultExecutionOrder(1)]
	public class Unit : MonoBehaviour
	{
		public static HashSet<Unit> List { get; } = new();
		public static DisposableAction<Unit> InitializedAction { get; } = new();
		public static DisposableAction<Unit> DisposedAction { get; } = new();

		public DisposableAction<Ability> AbilityAddedAction { get; } = new();
		public DisposableAction<Ability> AbilityRemovedAction { get; } = new();

		private readonly Dictionary<Type, Ability> _abilities = new();

		private Transform _abilityParent;

		private void Awake()
		{
			_abilityParent = transform.Find("Abilities");

			if (_abilityParent == null)
			{
				_abilityParent = new GameObject("Abilities").transform;
				_abilityParent.SetParent(transform);
			}

			GatherAbilities(transform);
			InitializeAbilities();
			InitializedAction.Invoke(this);
			List.Add(this);
		}

		private void OnDestroy()
		{
			DisposedAction.Invoke(this);
		}

		public bool HasAbility(Type type)
		{
			return _abilities.ContainsKey(type);
		}

		public bool HasAbility<T>() where T : Ability
		{
			return HasAbility(TypeCache<T>.Value);
		}

		public T GetAbility<T>() where T : Ability
		{
			return (T)_abilities[TypeCache<T>.Value];
		}

		public void AddAbility<T>() where T : Ability
		{
			Type type = TypeCache<T>.Value;

			GameObject go = new GameObject(type.Name);
			go.transform.SetParent(_abilityParent);

			Ability ability = go.AddComponent<T>();
			_abilities.Add(type, ability);
			ability.Initialize(this);
			
			AbilityAddedAction?.Invoke(ability);
		}

		public void RemoveAbility<T>() where T : Ability
		{
			Type type = TypeCache<T>.Value;
			Ability ability = _abilities[type];

			if(_abilities.Remove(type))
			{
				AbilityRemovedAction?.Invoke(ability);
				Destroy(ability.gameObject);
			}
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

				foreach (Ability ability in child.GetComponents<Ability>())
					if (ability.gameObject.activeSelf && ability.enabled)
						_abilities.Add(ability.GetType(), ability);

				GatherAbilities(child);
			}
		}

		private void InitializeAbilities()
		{
			foreach (Ability ability in _abilities.Values)
				ability.Initialize(this);
		}
	}
}