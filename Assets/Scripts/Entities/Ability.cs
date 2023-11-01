using Scripts.Reactive;
using UnityEngine;

namespace Scripts.Entities
{
	public abstract class Ability : MonoBehaviour
	{
		public static DisposableAction<Ability> OnInitialized { get; } = new();
		public static DisposableAction<Ability> OnActivated { get; } = new();
		public static DisposableAction<Ability> OnDeactivated { get; } = new();
		public static DisposableAction<Ability> OnDisposed { get; } = new();

		public Unit Unit { get; private set; }
		public bool IsActive { get; private set; }

		public void Initialize(Unit unit)
		{
			Unit = unit;
			OnInitialize();
			OnInitialized.Invoke(this);
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
				OnActivated.Invoke(this);
			}
			else
			{
				IsActive = false;
				OnDeactivate();
				OnDeactivated.Invoke(this);
			}
		}
		
		private void Update()
		{
			if (IsActive)
			{
				OnUpdate();
			}
		}

		private void LateUpdate()
		{
			if (IsActive)
			{
				OnLateUpdate();
			}
		}

		private void OnDestroy()
		{
			OnDispose();
			OnDisposed.Invoke(this);
		}
		
		protected virtual void OnInitialize() { }
		protected virtual void OnActivate() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnLateUpdate() { }
		protected virtual void OnDeactivate() { }
		protected virtual void OnDispose() { }
	}
}