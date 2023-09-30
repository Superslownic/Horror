using System;
using UnityEngine;

namespace Game.Entities
{
	public abstract class Ability : MonoBehaviour
	{
		public static event Action<Ability> OnInitialized;
		public static event Action<Ability> OnDisposed;
		
		public Type Type { get; private set; }
		public Entity Entity { get; private set; }

		public void Initialize(Entity entity)
		{
			Entity = entity;
			Type = GetType();
			Initialized();
			OnInitialized?.Invoke(this);
		}

		private void OnDestroy()
		{
			Disposed();
			OnDisposed?.Invoke(this);
		}

		protected abstract void Initialized();
		protected abstract void Disposed();
	}
}