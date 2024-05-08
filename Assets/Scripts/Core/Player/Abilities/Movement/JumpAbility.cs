using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class JumpAbility : Ability
	{
		[SerializeField] private float _force;

		[Inject] private readonly InputManager _inputManager;

		private ChangeVelocityAbility _changeVelocityAbility;
		private CheckGroundAbility _checkGroundAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_inputManager.Jump.WasPressedThisFrame() && _checkGroundAbility.IsGrounded)
			{
				_changeVelocityAbility.SetActualVelocity(y: _force);
			}
		}
	}
}