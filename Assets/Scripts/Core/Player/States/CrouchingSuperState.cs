using Scripts.Configs;
using Scripts.FSM.Composite;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public CrouchingSuperState(string name, State initialState) : base(name, initialState)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Crouching);
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = false;
			_playerUnit.GetAbility<CrouchAbility>().PerformCrouch();
			base.Enter();
		}

		public override void Exit()
		{
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<CrouchAbility>().PerformStand();
			base.Exit();
		}
	}
}