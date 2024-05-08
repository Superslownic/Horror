using Scripts.Config;
using Scripts.FSM.Composite;
using Scripts.Units;
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
			if (_playerUnit.TryGetAbility(out HeadBobAbility headBobAbility))
			{
				headBobAbility.ReplaceConfig(_gameConfig.Player.Footsteps.WalkShakeConfig);
			}
			
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.StandWalkShakeConfig);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Walking);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}