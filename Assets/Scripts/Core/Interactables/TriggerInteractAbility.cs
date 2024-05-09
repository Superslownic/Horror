using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class TriggerInteractAbility : Ability
	{
		[SerializeField] private TriggerLink triggerLink;
		
		private CompositeDisposable _disposable = new();
		
		protected override void OnInitialize()
		{
			triggerLink.OnEnter.AddListener(HandleTriggerEnter).AddTo(_disposable);
			triggerLink.OnExit.AddListener(HandleTriggerExit).AddTo(_disposable);
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