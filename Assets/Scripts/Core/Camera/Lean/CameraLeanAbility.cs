using Scripts.Config;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Camera.Lean
{
	public class CameraLeanAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private PlayerMovementAbility _movementAbility;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		protected override void OnUpdate()
		{
			if (!_movementAbility.IsGrounded)
			{
				return;
			}
			
			if (_movementAbility.ActualVelocity.magnitude > _gameConfig.Camera.Lean.Threshold)
			{
				float input = _inputManager.Move.ReadValue<Vector2>().x;
				float targetAngle = -input * _movementAbility.NormalizedActualVelocity.magnitude * _gameConfig.Camera.Lean.Angle;
				Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

				_anchor.localRotation = Quaternion.Lerp
				(
					_anchor.localRotation,
					targetRotation,
					_gameConfig.Camera.Lean.Curve.Evaluate(_anchor.localEulerAngles.z / _gameConfig.Camera.Lean.Angle) * _gameConfig.Camera.Lean.Force * Time.deltaTime
				);
			}
			else
			{
				_anchor.localRotation = Quaternion.Lerp
				(
					_anchor.localRotation,
					Quaternion.identity,
					_gameConfig.Camera.Lean.Curve.Evaluate(_anchor.localEulerAngles.z / _gameConfig.Camera.Lean.Angle) * _gameConfig.Camera.Lean.Force * Time.deltaTime
				);
			}
		}
	}
}