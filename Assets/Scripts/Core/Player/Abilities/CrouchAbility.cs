using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class CrouchAbility : Ability
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Transform _cameraMainAnchor;

		[Inject] private readonly GameConfig _gameConfig;

		private ShakeHeadAbility _shakeHeadAbility;
		private float _startHeight;
		private float _startCameraHeight;
		private bool _isCrouching;
		private ShakerProcessor _shakerProcessor;
		private RandomShakeVariant _variant;
		private Tween _tween;

		protected override void OnInitialize()
		{
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_startHeight = _characterController.height;
			_startCameraHeight = _cameraMainAnchor.localPosition.y;
			_variant = new RandomShakeVariant();
			_shakerProcessor = new ShakerProcessor
			{
				Config = _gameConfig.Player.Crouch.ShakeConfig,
				Variant = _variant
			};
			_shakeHeadAbility.Shaker.StartShaker(_shakerProcessor);
		}

		[Button]
		public void PerformCrouch()
		{
			if (_isCrouching)
			{
				return;
			}

			_isCrouching = true;

			ReplaceConfig(_gameConfig.Player.Crouch.PerformShakeProcessorConfig);
			
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _characterController.height, value => _characterController.height = value, 1, _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, _gameConfig.Player.Crouch.TargetHeight * 0.5f, 0), _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _gameConfig.Player.Crouch.TargetCameraHeight, 0), _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad)
					.OnComplete(() => ReplaceConfig(RandomShakeProcessorConfig.Default)));
		}
		
		[Button]
		public void PerformStand()
		{
			if (!_isCrouching)
			{
				return;
			}

			_isCrouching = false;
			
			ReplaceConfig(_gameConfig.Player.Crouch.PerformShakeProcessorConfig);
			
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _characterController.height, value => _characterController.height = value, _startHeight, _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, _startHeight * 0.5f, 0), _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _startCameraHeight, 0), _gameConfig.Player.Crouch.Duration)
					.SetEase(Ease.InOutQuad)
					.OnComplete(() => ReplaceConfig(RandomShakeProcessorConfig.Default)));
		}
		
		private void ReplaceConfig(RandomShakeProcessorConfig config)
		{
			_variant.PositionAmplitude.ReplaceValue(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.ReplaceValue(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.ReplaceValue(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.ReplaceValue(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}