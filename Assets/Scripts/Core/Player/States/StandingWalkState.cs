using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingWalkState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerEntity.GetAbility<BreathAbility>().StopRunning();
			_playerEntity.GetAbility<BobbingAbility>().SetConfig(_gameConfig.Camera.Bobbing.WalkingValues);
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