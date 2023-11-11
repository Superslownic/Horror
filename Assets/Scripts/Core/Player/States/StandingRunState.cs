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
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(_gameConfig.Player.Footsteps.RunShakeConfig);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.StandRunShakeConfig);
			_playerUnit.GetAbility<MovementAbility>().ReplaceConfig(_gameConfig.Player.Movement.Running);
			_playerUnit.GetAbility<BreathAbility>().StartRunning();
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			_playerUnit.GetAbility<BreathAbility>().StopRunning();
		}
	}
}