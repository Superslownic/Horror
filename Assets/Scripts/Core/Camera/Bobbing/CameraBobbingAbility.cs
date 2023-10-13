using DG.Tweening;
using DG.Tweening.Core;
using Scripts.Config;
using Scripts.Config.Camera;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Camera
{
	public class CameraBobbingAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private PlayerMovementAbility _movementAbility;

		[Inject] private readonly GameConfig _gameConfig;

		[ShowInInspector, ReadOnly] private CameraBobbingValues _config;
		[ShowInInspector, ReadOnly] private Vector2 _positionFrequency;
		[ShowInInspector, ReadOnly] private Vector2 _positionAmplitude;
		[ShowInInspector, ReadOnly] private Vector2 _rotationFrequency;
		[ShowInInspector, ReadOnly] private Vector2 _rotationAmplitude;
		[ShowInInspector, ReadOnly] private Vector2 _positionTime;
		[ShowInInspector, ReadOnly] private Vector2 _rotationTime;

		protected override void OnInitialize()
		{
			SetConfig(_gameConfig.Camera.Bobbing.WalkingValues);
			ResetValues();
		}

		public void SetConfig(CameraBobbingValues cameraBobbingValuesConfig)
		{
			_config = cameraBobbingValuesConfig;
			ChangeParameter(() => _positionFrequency, value => _positionFrequency = value, _config.PositionFrequency);
			ChangeParameter(() => _positionAmplitude, value => _positionAmplitude = value, _config.PositionAmplitude);
			ChangeParameter(() => _rotationFrequency, value => _rotationFrequency = value, _config.RotationFrequency);
			ChangeParameter(() => _rotationAmplitude, value => _rotationAmplitude = value, _config.RotationAmplitude);
		}

		public void ResetValues()
		{
			_positionFrequency = _config.PositionFrequency;
			_positionAmplitude = _config.PositionAmplitude;
			_rotationFrequency = _config.RotationFrequency;
			_rotationAmplitude = _config.RotationAmplitude;
		}

		protected override void OnUpdate()
		{
			if (!_movementAbility.IsGrounded)
			{
				return;
			}
			
			if (_movementAbility.ActualVelocity.magnitude <= _gameConfig.Camera.Bobbing.Threshold)
			{
				return;
			}

			float strength = _movementAbility.NormalizedActualVelocity.magnitude;
			
			_positionTime.x += Time.deltaTime * strength * _positionFrequency.x;
			_positionTime.y += Time.deltaTime * strength * _positionFrequency.y;
			
			_rotationTime.x += Time.deltaTime * strength * _rotationFrequency.x;
			_rotationTime.y += Time.deltaTime * strength * _rotationFrequency.y;
			
			Vector3 position = _anchor.localPosition;
			position.x = Mathf.Cos(_positionTime.x * 0.5f) * _positionAmplitude.x;
			position.y = Mathf.Cos(_positionTime.y) * _positionAmplitude.y;
			
			Vector3 rotation = _anchor.localEulerAngles;
			rotation.y = Mathf.Sin(_rotationTime.x) * _rotationAmplitude.x;
			rotation.x = Mathf.Sin(_rotationTime.y * 0.5f) * _rotationAmplitude.y;
			
			_anchor.localPosition = position;
			_anchor.localEulerAngles = rotation;
		}

		private void ChangeParameter(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue)
		{
			DOTween.To(getter, setter, endValue, _gameConfig.Camera.Bobbing.ChangeDuration).SetEase(Ease.InOutCubic);
		}
	}
}