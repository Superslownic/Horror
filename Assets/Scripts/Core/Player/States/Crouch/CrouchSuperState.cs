using Scripts.Configs;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().ReplaceConfig(_gameConfig.Player.Movement.Crouching);
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = false;
			_playerUnit.GetAbility<CrouchAbility>().PerformCrouch();
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<CrouchAbility>().PerformStand();
		}
	}
}