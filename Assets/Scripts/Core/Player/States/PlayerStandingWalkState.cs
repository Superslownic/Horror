using Scripts.Config;
using Scripts.Core.Camera;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStandingWalkState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public PlayerStandingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerEntity.GetAbility<CameraBobbingAbility>().SetConfig(_gameConfig.Camera.Bobbing.WalkingValues);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}