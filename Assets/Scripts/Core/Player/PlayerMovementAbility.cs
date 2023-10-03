using Scripts.Config;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class PlayerMovementAbility : DeactivatableAbility
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private CharacterController _characterController;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;
		
		private Vector3 _velocity;

		protected override void OnUpdate()
		{
			if (IsGrounded(out Vector3 groundNormal))
			{
				Vector2 moveInput = _inputManager.GetMoveValue();
			
				Vector3 forwardDirection = Vector3.ProjectOnPlane(_camera.transform.forward, groundNormal) * moveInput.y;
				Vector3 sideDirection = Vector3.ProjectOnPlane(_camera.transform.right, groundNormal) * moveInput.x;
				
				Vector3 inputMotion = (forwardDirection + sideDirection) * _gameConfig.Player.Movement.Speed;
				Vector3 clampedMotion = Vector3.ClampMagnitude(inputMotion, _gameConfig.Player.Movement.Speed);

				bool isMoving = moveInput.sqrMagnitude > 0;

				Vector3 resultMotion = isMoving ? clampedMotion : Vector3.zero;
				float resultDelta = isMoving ? _gameConfig.Player.Movement.Acceleration : _gameConfig.Player.Movement.Deceleration;
				
				_velocity = Vector3.MoveTowards(_velocity, resultMotion, resultDelta * Time.deltaTime);
				_characterController.Move(_velocity * Time.deltaTime);
			}
			
			_characterController.Move(Vector3.down * (_gameConfig.Player.Movement.Gravity * Time.deltaTime));
		}

		private bool IsGrounded(out Vector3 result)
		{
			Vector3 origin = transform.position + _characterController.center;
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