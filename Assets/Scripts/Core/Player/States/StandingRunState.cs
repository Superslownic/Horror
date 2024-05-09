using Scripts.Configs;
using Scripts.FSM.Composite;
using Scripts.Units;
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
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.StandRunShakeConfig);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Running);
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