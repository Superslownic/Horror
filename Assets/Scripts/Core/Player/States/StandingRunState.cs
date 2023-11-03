using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingRunState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingRunState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerUnit.GetAbility<CrouchAbility>().PerformStand();
			_playerUnit.GetAbility<HeadBobAbility>().ToRun();
			_playerUnit.GetAbility<BreathAbility>().StartRunning();
			_playerUnit.GetAbility<HeadSwayingAbility>().ToStandingRun();
			_playerUnit.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Running);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}