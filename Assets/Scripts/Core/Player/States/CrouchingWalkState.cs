using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingWalkState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public CrouchingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerUnit.GetAbility<CrouchAbility>().PerformCrouch();
			_playerUnit.GetAbility<BreathAbility>().StopRunning();
			_playerUnit.GetAbility<FootstepsAbility>().ToCrouch();
			_playerUnit.GetAbility<WobbleAbility>().ToCrouchWalk();
			_playerUnit.GetAbility<MovementAbility>().SetConfig(_gameConfig.Player.Movement.Crouching);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}