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
		private RandomShakeProcessorValues _values;
		private Tween _tween;

		protected override void OnInitialize()
		{
			_processor = new RandomShakeProcessor();
			_shaker = new Shaker
			{
				Type = ShakeProcessorType.Infinite,
				Processor = _processor
			};
			_shakeAbility.AddShaker(_shaker);
			_shaker.Start();
		}

		public void ToStandingIdle()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandingIdleValues);
		}

		public void ToStandingWalk()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandingWalkValues);
		}
		
		public void ToStandingRun()
		{
			ChangeValues(_gameConfig.Player.Wobble.StandingRunValues);
		}

		public void ToCrouchIdle()
		{
			ChangeValues(_gameConfig.Player.Wobble.CrouchIdleValues);
		}
		
		public void ToCrouchWalk()
		{
			ChangeValues(_gameConfig.Player.Wobble.CrouchWalkValues);
		}

		private void ChangeValues(RandomShakeProcessorValues values)
		{
			_values = values;
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween
					.To(() => _processor.PositionFrequency, value => _processor.PositionFrequency = value, _values.PositionFrequency,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.PositionAmplitude, value => _processor.PositionAmplitude = value, _values.PositionAmplitude,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationFrequency, value => _processor.RotationFrequency = value, _values.RotationFrequency,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationAmplitude, value => _processor.RotationAmplitude = value, _values.RotationAmplitude,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic));
		}
	}
}