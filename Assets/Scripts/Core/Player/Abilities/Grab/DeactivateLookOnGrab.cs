using System.Linq;
using Scripts.Reactive;
using Scripts.Units;

namespace Scripts.Core.Player
{
	public class DeactivateLookOnGrab : Ability
	{
		private UnitFilter _playerFilter;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerFilter = UnitFilter.Create().With<PlayerMarkerAbility>().Build(Disposable);
			Unit.AbilityAddedAction.AddListener(HandleAbilityAdded).AddTo(Disposable);
			Unit.AbilityRemovedAction.AddListener(HandleAbilityRemoved).AddTo(Disposable);
		}

		private void HandleAbilityAdded(Ability ability)
		{
			if (ability is GrabbedAbility)
			{
				_playerFilter.First().GetAbility<SmoothRotationAbility>().AddDeactivator(this);
			}
		}

		private void HandleAbilityRemoved(Ability ability)
		{
			if (ability is GrabbedAbility)
			{
				_playerFilter.First().GetAbility<SmoothRotationAbility>().RemoveDeactivator(this);
			}
		}
	}
}