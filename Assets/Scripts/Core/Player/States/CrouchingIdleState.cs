using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingIdleState : State
	{
		[Inject] private readonly Entity _playerEntity;
		[Inject] private readonly GameConfig _gameConfig;
		
		public CrouchingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerEntity.GetAbility<CrouchAbility>().Crouch();
			_playerEntity.GetAbility<BreathAbility>().StopRunning();
			_playerEntity.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Crouching);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			_playerEntity.GetAbility<CrouchAbility>().Stand();
		}
	}
}