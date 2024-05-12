using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class CrouchAbility : Ability
	{
		[SerializeField] private float _duration;

		[Inject] private readonly GameConfig _gameConfig;

		private ShakeHeadAbility _shakeHeadAbility;
		private ChangeHeightAbility _changeHeightAbility;
		private bool _isCrouching;
		private ShakerProcessor _shakerProcessor;
		private RandomShakeVariant _variant;

		protected override void OnInitialize()
		{
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_changeHeightAbility = Unit.GetAbility<ChangeHeightAbility>();
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
			_changeHeightAbility.Execute(_gameConfig.Player.ChangeHeight.CrouchConfig, _duration, () => ReplaceConfig(RandomShakeProcessorConfig.Default));
		}
		
		[Button]
		public void PerformStand()
		{
			if (!_isCrouching)
				return;

			_isCrouching = false;
			ReplaceConfig(_gameConfig.Player.Crouch.PerformShakeProcessorConfig);
			_changeHeightAbility.Execute(_gameConfig.Player.ChangeHeight.StandConfig, _duration, () => ReplaceConfig(RandomShakeProcessorConfig.Default));
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