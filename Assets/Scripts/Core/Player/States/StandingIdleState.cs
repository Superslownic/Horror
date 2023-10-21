using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingIdleState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerEntity.GetAbility<BreathAbility>().StopRunning();
			_playerEntity.GetAbility<FootstepsAbility>().Deactivate();
			_playerEntity.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Walking);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}