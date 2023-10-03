using Scripts.Config;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class PlayerMovementAbility : DeactivatableAbility
	{
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;
		
		private Camera _camera;
		private CharacterController _characterController;
		private Vector3 _velocity;

		protected override void OnInitialize()
		{
			if (!Entity.TryGetComponent(out _characterController))
			{
				Debug.LogError("There is no CharacterController on Entity");
				SetActive(false);
				return;
			}
		}

		protected override void OnUpdate()
		{
			bool isGrounded = GetGroundNormal(out Vector3 groundNormal);

			Vector3 projectNormal = isGrounded ? groundNormal : Vector3.up;
			
			Vector3 forwardDirection = Vector3.Project(_camera.transform.forward, projectNormal);
			Vector3 sideDirection = Vector3.Project(_camera.transform.right, projectNormal);
			
			Vector2 moveInput = _inputManager.GetMoveValue();

			forwardDirection *= moveInput.y;
			sideDirection *= moveInput.x;

			Vector3 resultDirection = Vector3.ClampMagnitude(forwardDirection + sideDirection, _gameConfig.Player.Movement.MaxSpeed);

			_velocity = Vector3.Lerp(_velocity, resultDirection, _gameConfig.Player.Movement.AccelerationDelta * Time.deltaTime);

			_characterController.Move(_velocity);
		}

		private bool GetGroundNormal(out Vector3 result)
		{
			Vector3 origin = _characterController.center;
			float radius = _characterController.radius;
			Vector3 direction = Vector3.down;
			float distance = _characterController.height * 0.5f - _characterController.radius + _gameConfig.Player.Movement.GroundCheckThreshold;
			LayerMask layer = _gameConfig.Player.Movement.FloorLayer;

			if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, distance, layer))
			{
				result = hit.normal;
				return true;
			}

			result = default;
			return false;
		}
	}
}