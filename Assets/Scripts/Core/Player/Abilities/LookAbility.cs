using Scripts.Config;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LookAbility : Ability
	{
		[SerializeField] private Transform _mainAnchor;
		[SerializeField] private Transform _floatingAnchor;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private Vector3 _rotation;
		private Vector2 _velocity;

		protected override void OnInitialize()
		{
			_rotation = _mainAnchor.localEulerAngles;
		}

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

			_rotation += new Vector3(-_velocity.y * sensitivity, _velocity.x * sensitivity, 0);
			_rotation.x = Mathf.Clamp(_rotation.x, _gameConfig.Player.Look.VerticalMinAngle, _gameConfig.Player.Look.VerticalMaxAngle);
			
			_mainAnchor.localEulerAngles = _rotation;
			
			_floatingAnchor.position = Vector3.Lerp(_floatingAnchor.position, _mainAnchor.position, _gameConfig.Player.Look.PositionInterpolationSpeed * Time.deltaTime);

			float angle = Quaternion.Angle(_floatingAnchor.localRotation, _mainAnchor.localRotation);
			
			//force limit rotation
			/*if (angle > maxDegrees)
			{
				Quaternion fromRotation = _mainAnchor.localRotation;
				Quaternion toRotation = _floatingAnchor.localRotation;
				
				fromRotation.Normalize();
				toRotation.Normalize();

				Quaternion deltaQuaternion = Quaternion.Inverse(fromRotation) * toRotation;
				deltaQuaternion = Quaternion.RotateTowards(Quaternion.identity, deltaQuaternion, maxDegrees);
				deltaQuaternion.Normalize();

				_floatingAnchor.localRotation = fromRotation * deltaQuaternion;
			}*/
			
			_floatingAnchor.rotation = Quaternion.Lerp(_floatingAnchor.rotation, _mainAnchor.rotation, angle * _gameConfig.Player.Look.RotationInterpolationSpeed * Time.deltaTime);
		}
	}
}