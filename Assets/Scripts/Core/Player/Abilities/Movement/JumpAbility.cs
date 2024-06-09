using Scripts.Input;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class JumpAbility : Ability
	{
		[SerializeField] private float _force;

		[Inject] private readonly InputManager _inputManager;

		//private ChangeVelocityAbility _changeVelocityAbility;
		private GroundMoveAbility _groundMoveAbility;
		private RigidbodyAbility _rigidbodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			//_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_inputManager.Jump.WasPressedThisFrame() && _groundMoveAbility.IsGrounded)
			{
				//_changeVelocityAbility.SetActualVelocity(y: _force);
				_groundMoveAbility.SetInAir();
				_rigidbodyAbility.Rigidbody.linearVelocity = _rigidbodyAbility.Rigidbody.linearVelocity.SetY(_force);
			}
		}
	}
}