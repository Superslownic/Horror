using System;
using Scripts.Configs;
using Scripts.Reflection;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandRunState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override Type Parent => TypeCache<StandSuperState>.Value;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(_gameConfig.Player.Footsteps.RunShakeConfig);
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.StandRunShakeConfig);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Running);
			_playerUnit.GetAbility<BreathAbility>().StartRunning();
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<BreathAbility>().StopRunning();
		}
	}
}