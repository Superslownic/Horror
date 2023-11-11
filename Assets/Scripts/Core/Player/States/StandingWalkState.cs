using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingWalkState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(_gameConfig.Player.Footsteps.WalkShakeConfig);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.StandWalkShakeConfig);
			_playerUnit.GetAbility<MovementAbility>().ReplaceConfig(_gameConfig.Player.Movement.Walking);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}