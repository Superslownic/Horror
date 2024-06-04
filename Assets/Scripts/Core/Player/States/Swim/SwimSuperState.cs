using Scripts.Configs;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = true;
			_playerUnit.GetAbility<WaterMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.SwimConfig);
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);

			RigidbodyAbility rigidbodyAbility = _playerUnit.GetAbility<RigidbodyAbility>();
			rigidbodyAbility.Rigidbody.useGravity = false;
			rigidbodyAbility.Rigidbody.linearDamping = 5;
		}

		public override void OnUpdate()
		{
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = _playerUnit.GetAbility<PlayerCheckWaterAbility>().IsInWater;
			_playerUnit.GetAbility<RigidbodyAbility>().Rigidbody.useGravity = !_playerUnit.GetAbility<PlayerCheckWaterAbility>().IsInWater;
			_playerUnit.GetAbility<RigidbodyAbility>().Rigidbody.linearDamping = _playerUnit.GetAbility<PlayerCheckWaterAbility>().IsInWater ? 5 : 0;
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<WaterMoveAbility>().RemoveActivator(this);

			RigidbodyAbility rigidbodyAbility = _playerUnit.GetAbility<RigidbodyAbility>();
			rigidbodyAbility.Rigidbody.useGravity = true;
			rigidbodyAbility.Rigidbody.linearDamping = 0;
		}
	}
}