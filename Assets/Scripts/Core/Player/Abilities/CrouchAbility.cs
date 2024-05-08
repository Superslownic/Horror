using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class CrouchAbility : Ability
	{
		[SerializeField] private Transform _cameraMainAnchor;
		[SerializeField] private float _duration;
		[SerializeField] private float _heightMultiplier;

		[Inject] private readonly GameConfig _gameConfig;

		private ShakeHeadAbility _shakeHeadAbility;
		private PlayerBodyAbility _playerBodyAbility;
		private float _startHeight;
		private float _startCameraHeight;
		private bool _isCrouching;
		private ShakerProcessor _shakerProcessor;
		private RandomShakeVariant _variant;
		private Tween _tween;

		protected override void OnInitialize()
		{
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			_startHeight = _playerBodyAbility.WalkCollider.height;
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
				return;

			_isCrouching = true;

			ReplaceConfig(_gameConfig.Player.Crouch.PerformShakeProcessorConfig);
			
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _playerBodyAbility.WalkCollider.height, value => _playerBodyAbility.WalkCollider.height = value, _startHeight * _heightMultiplier, _duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _playerBodyAbility.WalkCollider.center, value => _playerBodyAbility.WalkCollider.center = value, new Vector3(0, _startHeight * _heightMultiplier * 0.5f, 0), _duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _startCameraHeight * _heightMultiplier, 0), _duration)
					.SetEase(Ease.InOutQuad)
					.OnComplete(() => ReplaceConfig(RandomShakeProcessorConfig.Default)));
		}
		
		[Button]
		public void PerformStand()
		{
			if (!_isCrouching)
				return;

			_isCrouching = false;
			
			ReplaceConfig(_gameConfig.Player.Crouch.PerformShakeProcessorConfig);
			
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _playerBodyAbility.WalkCollider.height, value => _playerBodyAbility.WalkCollider.height = value, _startHeight, _duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _playerBodyAbility.WalkCollider.center, value => _playerBodyAbility.WalkCollider.center = value, new Vector3(0, _startHeight * 0.5f, 0), _duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _startCameraHeight, 0), _duration)
					.SetEase(Ease.InOutQuad)
					.OnComplete(() => ReplaceConfig(RandomShakeProcessorConfig.Default)));
		}
		
		private void ReplaceConfig(RandomShakeProcessorConfig config)
		{
			_variant.PositionAmplitude.Tween(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.Tween(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.Tween(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.Tween(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}