using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class WaterMoveAbility : Ability
	{
		[SerializeField] private float _speed;
		[SerializeField] private float _maxHeightAboveWater;
		[SerializeField] private float _popupDepth;
		[SerializeField] private float _popupSpeed;

		[Inject] private readonly InputManager _inputManager;

		private PlayerHeadAbility _playerHeadAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private PlayerCheckWaterAbility _checkWaterAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_checkWaterAbility = Unit.GetAbility<PlayerCheckWaterAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

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

			_changeVelocityAbility.SetTargetVelocity(clampedInputMotion);
		}
	}
}