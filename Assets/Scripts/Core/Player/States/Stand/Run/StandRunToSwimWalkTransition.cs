using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandRunToSwimWalkTransition : Transition<StandRunState, SwimWalkState>
	{
		[Inject] private readonly Unit _playerUnit;

		private PlayerCheckWaterAbility _playerCheckWaterAbility;

		public override bool IsValid => _playerCheckWaterAbility.IsInWater && !_playerCheckWaterAbility.CanStand;

		public override void Initialize()
		{
			_playerCheckWaterAbility = _playerUnit.GetAbility<PlayerCheckWaterAbility>();
		}
	}
}