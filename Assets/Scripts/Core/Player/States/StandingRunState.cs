using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingRunState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingRunState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerEntity.GetAbility<FootstepsAbility>().Activate(_gameConfig.Player.Bobbing.RunningValues);
			_playerEntity.GetAbility<BreathAbility>().StartRunning();
			_playerEntity.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Running);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}