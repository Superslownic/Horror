using Scripts.Config;
using Scripts.Core.Camera;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerCrouchingWalkState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public PlayerCrouchingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerEntity.GetAbility<PlayerCrouchAbility>().Crouch();
			_playerEntity.GetAbility<CameraBobbingAbility>().SetConfig(_gameConfig.Camera.Bobbing.CrouchingValues);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			_playerEntity.GetAbility<PlayerCrouchAbility>().Stand();
		}
	}
}