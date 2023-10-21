using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Core.Player.Shake;
using Scripts.Entities;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Scripts.Core.Player
{
	public class FootstepsAbility : Ability
	{
		[SerializeField] private UnityEvent _event;
		[SerializeField] private ShakeAbility _shakeAbility;
		[SerializeField] private MovementAbility _movementAbility;

		[Inject] private readonly GameConfig _gameConfig;

		private Shaker _shaker;
		private BobbingShakeProcessor _processor;
		private BobbingValues _values;
		private Tween _tween;
		private int _counter;

		protected override void OnInitialize()
		{
			_processor = new BobbingShakeProcessor();
			_shaker = new Shaker
			{
				Type = ShakeProcessorType.Infinite,
				Processor = _processor
			};
			_shakeAbility.AddShaker(_shaker);
			_shaker.Start();
		}

		protected override void OnUpdate()
		{
			_processor.Magnitude = _movementAbility.NormalizedActualVelocity.magnitude;
		}

		public void Activate(BobbingValues values)
		{
			_values = values;
			UpdateValues();
		}

		public void Deactivate()
		{
			_values = BobbingValues.Default;
			UpdateValues();
		}

		private void UpdateValues()
		{
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween
					.To(() => _processor.PositionFrequency, value => _processor.PositionFrequency = value, _values.PositionFrequency,
						_gameConfig.Player.Bobbing.ChangeDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.PositionAmplitude, value => _processor.PositionAmplitude = value, _values.PositionAmplitude,
						_gameConfig.Player.Bobbing.ChangeDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationFrequency, value => _processor.RotationFrequency = value, _values.RotationFrequency,
						_gameConfig.Player.Bobbing.ChangeDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationAmplitude, value => _processor.RotationAmplitude = value, _values.RotationAmplitude,
						_gameConfig.Player.Bobbing.ChangeDuration).SetEase(Ease.InOutCubic));
		}
	}
}