using Scripts.Configs;
using Scripts.FSM.Composite;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimmingSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public SwimmingSuperState(string name, State initialState) : base(name, initialState)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = true;
			_playerUnit.GetAbility<WaterMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.SwimConfig, _gameConfig.Player.ChangeHeight.Duration);
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);

			RigidbodyAbility rigidbodyAbility = _playerUnit.GetAbility<RigidbodyAbility>();
			rigidbodyAbility.Rigidbody.useGravity = false;
			rigidbodyAbility.Rigidbody.linearDamping = 5;

			base.Enter();
		}

		public override void Exit()
		{
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<WaterMoveAbility>().RemoveActivator(this);

			RigidbodyAbility rigidbodyAbility = _playerUnit.GetAbility<RigidbodyAbility>();
			rigidbodyAbility.Rigidbody.useGravity = true;
			rigidbodyAbility.Rigidbody.linearDamping = 0;

			base.Exit();
		}
	}
}