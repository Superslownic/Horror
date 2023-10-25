using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Core.Player.Shake;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class WobbleAbility : Ability
	{
		[SerializeField] private ShakeAbility _shakeAbility;
		
		[Inject] private readonly GameConfig _gameConfig;

		private Shaker _shaker;
		private RandomShakeProcessor _processor;

		protected override void OnInitialize()
		{
			_processor = new RandomShakeProcessor();
			_shaker = new Shaker
			{
				Config = _gameConfig.Player.Wobble.ShakerConfig,
				Processor = _processor
			};
			_shakeAbility.StartShaker(_shaker);
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
			_processor.PositionAmplitude.ReplaceValue(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.PositionFrequency.ReplaceValue(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.RotationAmplitude.ReplaceValue(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.RotationFrequency.ReplaceValue(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}