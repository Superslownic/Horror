using Scripts.Entities;
using Scripts.Reactive;
using UnityEngine;

namespace Scripts.Core
{
	public class TriggerInteractAbility : Ability
	{
		[SerializeField] private TriggerProvider _triggerProvider;
		
		private CompositeDisposable _disposable = new();
		
		protected override void OnInitialize()
		{
			_triggerProvider.OnEnter.AddListener(HandleTriggerEnter).AddTo(_disposable);
			_triggerProvider.OnExit.AddListener(HandleTriggerExit).AddTo(_disposable);
		}

		protected override void OnDispose()
		{
			_disposable.Dispose();
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if(!unit.TryGetAbility(out Interactable interactableAbility))
				return;

			interactableAbility.Activate(Unit);
		}

		private void HandleTriggerExit(Unit unit)
		{
			if(!unit.TryGetAbility(out Interactable interactableAbility))
				return;

			interactableAbility.Deactivate();
		}
	}
}