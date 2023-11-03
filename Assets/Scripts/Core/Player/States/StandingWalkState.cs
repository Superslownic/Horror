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
			_playerUnit.GetAbility<CrouchAbility>().PerformStand();
			_playerUnit.GetAbility<BreathAbility>().StopRunning();
			_playerUnit.GetAbility<HeadBobAbility>().ToWalk();
			_playerUnit.GetAbility<HeadSwayingAbility>().ToStandingWalk();
			_playerUnit.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Walking);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}