using Scripts.Reactive;
using Scripts.Units;

namespace Scripts.Core.Player
{
	public class ToggleKinematicOnGrab : Ability
	{
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Unit.AbilityAddedAction.AddListener(HandleAbilityAdded).AddTo(Disposable);
			Unit.AbilityRemovedAction.AddListener(HandleAbilityRemoved).AddTo(Disposable);
		}

		private void HandleAbilityAdded(Ability ability)
		{
			if (ability is GrabbedAbility)
			{
				Unit.GetAbility<RigidbodyAbility>().Rigidbody.isKinematic = false;
			}
		}
		
		private void HandleAbilityRemoved(Ability ability)
		{
			if (ability is GrabbedAbility)
			{
				Unit.GetAbility<RigidbodyAbility>().Rigidbody.isKinematic = true;
			}
		}
	}
}