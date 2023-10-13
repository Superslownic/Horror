using Scripts.Config;
using Scripts.Core.Camera;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStandingRunState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public PlayerStandingRunState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerEntity.GetAbility<CameraBobbingAbility>().SetConfig(_gameConfig.Camera.Bobbing.RunningValues);
			_playerEntity.GetAbility<PlayerMovementAbility>().SetConfig(_gameConfig.Player.Movement.RunningValues);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}