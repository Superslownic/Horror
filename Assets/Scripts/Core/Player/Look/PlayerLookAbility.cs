using Scripts.Config;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.Look
{
	public class PlayerLookAbility : DeactivatableAbility
	{
		[SerializeField] private Transform _horizontalRotationTransform;
		[SerializeField] private Transform _verticalRotationTransform;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private Vector2 _velocity;

		protected override void OnUpdate()
		{
			Vector2 rotationInput = _inputManager.GetRotationValue();
			
			bool isRotating = rotationInput.sqrMagnitude > 0;
			
			Vector2 resultMotion = isRotating ? rotationInput : Vector2.zero;
			
			//gamepad
			{
				float resultDelta = isRotating ? _gameConfig.Player.Look.Acceleration : _gameConfig.Player.Look.Deceleration;
				_velocity = Vector3.MoveTowards(_velocity, resultMotion, resultDelta * Time.deltaTime);
			}
			
			//mouse
			{
				//_velocity = resultMotion;
			}

			Quaternion horizontalRotation = Quaternion.Euler(0, _velocity.x * _gameConfig.Player.Look.Sensitivity, 0);
			Quaternion verticalRotation = Quaternion.Euler(-_velocity.y * _gameConfig.Player.Look.Sensitivity, 0, 0);
			
			_horizontalRotationTransform.rotation *= horizontalRotation;
			_verticalRotationTransform.rotation *= verticalRotation;
		}
	}
}