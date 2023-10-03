using System;
using System.Collections.Generic;
using Scripts.Reactive;
using Scripts.Utility;
using UnityEngine;

namespace Scripts.Entities
{
	public sealed class Entity : MonoBehaviour
	{
		public static DisposableAction<Entity> OnInitialized { get; } = new();
		public static DisposableAction<Entity> OnDisposed { get; } = new();
		
		private Dictionary<Type, Ability> _abilities = new();

		private void Awake()
		{
			GatherAbilities(transform);
			OnInitialized.Invoke(this);
		}

		private void OnDestroy()
		{
			OnDisposed.Invoke(this);
		}

		private void GatherAbilities(Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				
				if (child.HasComponent<Entity>())
					continue;

				if (child.TryGetComponent(out Ability ability))
				{
					ability.Initialize(this);
					_abilities.Add(ability.Type, ability);
				}
				
				GatherAbilities(child);
			}
		}
	}
}