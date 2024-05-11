using Scripts.TFSM;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimWalkToStandWalkTransition : Transition<SwimWalkState, StandWalkState>
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