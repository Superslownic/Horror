using Scripts.Reactive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Behaviour
{
	[DefaultExecutionOrder(2)]
	public abstract class Toggleable : MonoBehaviour
	{
		public DisposableAction OnActivated { get; } = new();
		public DisposableAction OnDeactivated { get; } = new();

		[ShowInInspector, ReadOnly, ToggleLeft, PropertyOrder(-2)] public bool IsActive { get; private set; }

		[SerializeReference, FoldoutGroup("Controllers")] private IActivationController _activationController;
		[SerializeReference, FoldoutGroup("Controllers")] private IDeactivationController _deactivationController;

		private void Awake()
		{
			UpdateActivation();
		}

		[Button, FoldoutGroup("Manage Activation"), PropertyOrder(-1)]
		public void AddDeactivator(object deactivator)
		{
			_deactivationController ??= new DeactivationController();

			if (_deactivationController.Add(deactivator))
				UpdateActivation();
		}

		[Button, FoldoutGroup("Manage Activation"), PropertyOrder(-1)]
		public void RemoveDeactivator(object deactivator)
		{
			_deactivationController ??= new DeactivationController();

			if (_deactivationController.Remove(deactivator))
				UpdateActivation();
		}

		[Button, FoldoutGroup("Manage Activation"), PropertyOrder(-1)]
		public void AddActivator(object activator)
		{
			_activationController ??= new ActivationController();

			if (_activationController.Add(activator))
				UpdateActivation();
		}

		[Button, FoldoutGroup("Manage Activation"), PropertyOrder(-1)]
		public void RemoveActivator(object activator)
		{
			_activationController ??= new ActivationController();

			if (_activationController.Remove(activator))
				UpdateActivation();
		}

		public void UpdateActivation()
		{
			bool lastState = IsActive;
			bool deactivationControllerState = _deactivationController?.GetState() ?? true;
			bool activationControllerState = _activationController?.GetState() ?? true;
			IsActive = deactivationControllerState && activationControllerState;

			if (lastState == IsActive)
				return;

			if (IsActive)
			{
				OnActivate();
			}
			else
			{
				OnDeactivate();
			}
		}

		protected virtual void OnActivate() { }
		protected virtual void OnDeactivate() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnLateUpdate() { }
		protected virtual void OnFixedUpdate() { }

		private void Update()
		{
			if (IsActive)
				OnUpdate();
		}

		private void LateUpdate()
		{
			if (IsActive)
				OnLateUpdate();
		}

		private void FixedUpdate()
		{
			if (IsActive)
				OnFixedUpdate();
		}
	}
}