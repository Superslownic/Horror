using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimIdleToStandIdleTransition : Transition<SwimIdleState, StandIdleState>
	{
		[Inject] private readonly Unit _playerUnit;

		public override bool IsValid => _playerCheckWaterAbility.CanStand;

		private PlayerCheckWaterAbility _playerCheckWaterAbility;

		public override void Initialize()
		{
			_playerCheckWaterAbility = _playerUnit.GetAbility<PlayerCheckWaterAbility>();
		}

		public override void Execute()
		{
			_playerUnit.GetAbility<ChangeHeightAbility>().ResizeToFitSurface();
			FinishTransition();
		}
	}
}