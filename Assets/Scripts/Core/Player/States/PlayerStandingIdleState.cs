using Scripts.Config;
using Scripts.Core.Camera;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStandingIdleState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public PlayerStandingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerEntity.GetAbility<CameraBreathAbility>().StopRunning();
			_playerEntity.GetAbility<PlayerBreathSoundAbility>().StopRunning();
			_playerEntity.GetAbility<PlayerMovementAbility>().SetConfig(_gameConfig.Player.Movement.WalkingValues);
		}

		public override void Update()
		{
			_playerEntity.GetAbility<CameraBreathAbility>().Decrease();
		}

		public override void Exit()
		{
		}
	}
}