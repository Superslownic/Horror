using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Camera;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Camera
{
	public class CameraBobbingAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private PlayerMovementAbility _movementAbility;

		[Inject] private readonly GameConfig _gameConfig;

		private CameraBobbingValues _config;
		private Vector2 _positionFrequency;
		private Vector2 _positionAmplitude;
		private Vector2 _rotationFrequency;
		private Vector2 _rotationAmplitude;
		private Vector2 _positionTime;
		private Vector2 _rotationTime;
		private Tween _tween;

		protected override void OnInitialize()
		{
			SetConfig(_gameConfig.Camera.Bobbing.IdleValues);
			ResetValues();
		}

		public Tween SetConfig(CameraBobbingValues config)
		{
			return SetConfig(config, _gameConfig.Camera.Bobbing.ChangeDuration);
		}

		public Tween SetConfig(CameraBobbingValues config, float time)
		{
			_config = config;
			_tween?.Kill();
			return _tween = DOTween.Sequence()
				.Join(DOTween
					.To(() => _positionFrequency, value => _positionFrequency = value, _config.PositionFrequency,
						time).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _positionAmplitude, value => _positionAmplitude = value, _config.PositionAmplitude,
						time).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _rotationFrequency, value => _rotationFrequency = value, _config.RotationFrequency,
						time).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _rotationAmplitude, value => _rotationAmplitude = value, _config.RotationAmplitude,
						time).SetEase(Ease.InOutCubic));
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
			
			if (_config.DependsOnVelocity && _movementAbility.ActualVelocity.magnitude <= _gameConfig.Camera.Bobbing.Threshold)
			{
				return;
			}

			float strength = _config.DependsOnVelocity ? _movementAbility.NormalizedActualVelocity.magnitude : 1;
			
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
	}
}