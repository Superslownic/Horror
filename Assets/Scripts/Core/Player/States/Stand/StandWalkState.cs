using System;
using Scripts.Configs;
using Scripts.Reflection;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandWalkState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override Type Parent => TypeCache<StandSuperState>.Value;

		public override void OnEnter()
		{
			if (_playerUnit.TryGetAbility(out HeadBobAbility headBobAbility))
			{
				headBobAbility.ReplaceConfig(_gameConfig.Player.Footsteps.WalkShakeConfig);
			}

			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.StandWalkShakeConfig);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Walking);
		}
	}
}