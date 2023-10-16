using Scripts.Config;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LookAbility : Ability
	{
		[SerializeField] private Transform _horizontalRotationTransform;
		[SerializeField] private Transform _verticalRotationTransform;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private Vector2 _velocity;

		protected override void OnUpdate()
		{
			Vector2 rotationInput = _inputManager.Look.ReadValue<Vector2>();
			
			bool isRotating = rotationInput.sqrMagnitude > 0;
			
			Vector2 resultMotion = isRotating ? rotationInput : Vector2.zero;

			float sensitivity = 0;

			switch (_inputManager.CurrentInputType)
			{
				case InputType.Gamepad:
				{
					sensitivity = _gameConfig.Player.Look.Gamepad.Sensitivity;
					float resultDelta = isRotating ? _gameConfig.Player.Look.Gamepad.Acceleration : _gameConfig.Player.Look.Gamepad.Deceleration;
					_velocity = Vector3.MoveTowards(_velocity, resultMotion, resultDelta * Time.deltaTime);
					break;
				}
				
				case InputType.Keyboard:
				{
					sensitivity = _gameConfig.Player.Look.Keyboard.Sensitivity;
					_velocity = resultMotion;
					break;
				}
			}

			Quaternion horizontalRotation = Quaternion.Euler(0, _velocity.x * sensitivity, 0);
			Quaternion verticalRotation = Quaternion.Euler(-_velocity.y * sensitivity, 0, 0);
			
			_horizontalRotationTransform.rotation *= horizontalRotation;
			_verticalRotationTransform.rotation *= verticalRotation;
		}
	}
}