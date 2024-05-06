using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class JumpAbility : Ability
	{
		[SerializeField] private float _directionMultiplier;
		[SerializeField] private float _force;

		[Inject] private readonly InputManager _inputManager;

		private PlayerHeadAbility _playerHeadAbility;
		private RigidbodyAbility _rigidbodyAbility;
		private CheckGroundAbility _checkGroundAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_inputManager.Jump.WasPressedThisFrame() && _checkGroundAbility.IsGrounded)
			{
				Vector2 input = _inputManager.Move.ReadValue<Vector2>();
				Vector3 forwardInputMotion = Vector3.ProjectOnPlane(_playerHeadAbility.HeadDetachedAnchor.forward, Vector3.up).normalized * input.y;
				Vector3 sideInputMotion = Vector3.ProjectOnPlane(_playerHeadAbility.HeadDetachedAnchor.right, Vector3.up).normalized * input.x;
				Vector3 resultInputMotion = forwardInputMotion + sideInputMotion;
				//_rigidbodyAbility.Rigidbody.linearVelocity = (Vector3.up + resultInputMotion.normalized * _directionMultiplier) * _force;
				Vector3 velocity = _rigidbodyAbility.Rigidbody.linearVelocity;
				velocity.y = _force;
				_rigidbodyAbility.Rigidbody.linearVelocity = velocity;
				//_rigidbodyAbility.Rigidbody.AddForce((Vector3.up + resultInputMotion.normalized * _directionMultiplier) * _force, ForceMode.Impulse);
			}
		}
	}
}