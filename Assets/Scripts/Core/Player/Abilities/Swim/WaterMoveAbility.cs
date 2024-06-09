using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class WaterMoveAbility : Ability
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _speed;
		[SerializeField] private float _maxHeightAboveWater;
		[SerializeField] private float _popupDepth;
		[SerializeField] private float _popupSpeed;
		[SerializeField] private float _acceleration;

		[Inject] private readonly InputManager _inputManager;

		private PlayerHeadAbility _playerHeadAbility;
		private PlayerCheckWaterAbility _checkWaterAbility;
		private Vector3 _velocity;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_checkWaterAbility = Unit.GetAbility<PlayerCheckWaterAbility>();
		}

		protected override void OnActivate()
		{
			_velocity = _rigidbody.linearVelocity;
		}

		protected override void OnUpdate()
		{
			float playerHeadHeight = _playerHeadAbility.HeadDetachedAnchor.position.y;
			float waterSurfaceHeight = _checkWaterAbility.WaterSurfaceHeight;
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			Vector3 forwardInputMotion = _playerHeadAbility.HeadFloatingAnchor.forward * moveInput.y;
			Vector3 sideInputMotion = _playerHeadAbility.HeadFloatingAnchor.right * moveInput.x;
			Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
			Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _speed);

			// Popup
			if (playerHeadHeight > waterSurfaceHeight - _popupDepth && playerHeadHeight < waterSurfaceHeight + _maxHeightAboveWater && clampedInputMotion.y >= 0)
				clampedInputMotion.y = (waterSurfaceHeight + _maxHeightAboveWater - playerHeadHeight) * _popupSpeed;

			// Clamp Height
			if (playerHeadHeight >= waterSurfaceHeight + _maxHeightAboveWater && clampedInputMotion.y > 0)
				clampedInputMotion.y = 0;

			_velocity = Vector3.Lerp(_velocity, clampedInputMotion, _acceleration * Time.deltaTime);
			_rigidbody.linearVelocity = _velocity;
		}
	}
}