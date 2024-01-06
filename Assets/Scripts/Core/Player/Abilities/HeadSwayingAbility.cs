using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using Zenject;

namespace Scripts.Core.Player
{
	public class HeadSwayingAbility : Ability
	{
		[Inject] private readonly GameConfig _gameConfig;

		private ShakeHeadAbility _shakeHeadAbility;
		private ShakerProcessor _shakerProcessor;
		private RandomShakeVariant _variant;

		protected override void OnInitialize()
		{
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_variant = new RandomShakeVariant();
			_shakerProcessor = new ShakerProcessor
			{
				Config = _gameConfig.Player.Wobble.ShakerConfig,
				Variant = _variant
			};
			_shakeHeadAbility.Shaker.StartShaker(_shakerProcessor);
		}
		
		public void ReplaceConfig(RandomShakeProcessorConfig config)
		{
			_variant.PositionAmplitude.Tween(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.Tween(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.Tween(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.Tween(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}