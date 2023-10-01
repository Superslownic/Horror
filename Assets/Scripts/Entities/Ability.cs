using System;
using UnityEngine;

namespace Scripts.Entities
{
	public abstract class Ability : MonoBehaviour
	{
		public static event Action<Ability> Initialized;
		public static event Action<Ability> Disposed;
		
		public Type Type { get; private set; }
		public Entity Entity { get; private set; }
		public bool IsActive { get; private set; }

		public void Initialize(Entity entity)
		{
			Entity = entity;
			Type = GetType();
			OnInitialize();
			Initialized?.Invoke(this);
		}

		public void SetActive(bool value)
		{
			if(IsActive == value)
			{
				return;
			}
			
			if (value)
			{
				IsActive = true;
				OnActivate();
			}
			else
			{
				IsActive = false;
				OnDeactivate();
			}
		}
		
		private void Update()
		{
			if (IsActive)
			{
				OnUpdate();
			}
		}

		private void OnDestroy()
		{
			OnDispose();
			Disposed?.Invoke(this);
		}
		
		protected virtual void OnInitialize() { }
		protected virtual void OnActivate() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnDeactivate() { }
		protected virtual void OnDispose() { }
	}
}