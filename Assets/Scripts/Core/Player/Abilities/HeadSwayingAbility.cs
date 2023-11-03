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

		public void ToStandingIdle()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandIdleShakeConfig);
		}

		public void ToStandingWalk()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandWalkShakeConfig);
		}
		
		public void ToStandingRun()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandRunShakeConfig);
		}

		public void ToCrouchIdle()
		{
			ChangeValues(_gameConfig.Player.Wobble.CrouchIdleShakeConfig);
		}
		
		public void ToCrouchWalk()
		{
			ChangeValues(_gameConfig.Player.Wobble.CrouchWalkShakeConfig);
		}
		
		private void ChangeValues(RandomShakeProcessorConfig config)
		{
			_variant.PositionAmplitude.ReplaceValue(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.ReplaceValue(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.ReplaceValue(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.ReplaceValue(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}