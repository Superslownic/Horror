using DG.Tweening;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Core.Player.Shake;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class FootstepsAbility : Ability
	{
		[SerializeField] private ShakeAbility _shakeAbility;
		[SerializeField] private MovementAbility _movementAbility;

		[Inject] private readonly GameConfig _gameConfig;
		[Inject] private readonly AudioManager _audioManager;

		private Shaker _shaker;
		private BobbingShakeProcessor _processor;
		private BobbingValues _bobbingValues;
		private Sound _footstepSound;
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
			_footstepSound = _audioManager.Create(_gameConfig.Audio.Events.Footstep);
		}

		protected override void OnUpdate()
		{
			if (_counter != Mathf.RoundToInt(_processor.PositionTime))
			{
				_counter = Mathf.RoundToInt(_processor.PositionTime);
				_footstepSound.SetVolume(_movementAbility.NormalizedActualVelocity.magnitude);
				_footstepSound.Play();
			}
			
			_processor.Magnitude = _movementAbility.NormalizedActualVelocity.magnitude;
		}

		public void Activate(BobbingValues values)
		{
			_bobbingValues = values;
			UpdateValues();
		}

		public void Deactivate()
		{
			_bobbingValues = BobbingValues.Default;
			UpdateValues();
		}

		private void UpdateValues()
		{
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween
					.To(() => _processor.PositionFrequency, value => _processor.PositionFrequency = value, _bobbingValues.PositionFrequency,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.PositionAmplitude, value => _processor.PositionAmplitude = value, _bobbingValues.PositionAmplitude,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationFrequency, value => _processor.RotationFrequency = value, _bobbingValues.RotationFrequency,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic))
				.Join(DOTween
					.To(() => _processor.RotationAmplitude, value => _processor.RotationAmplitude = value, _bobbingValues.RotationAmplitude,
						_gameConfig.Player.Footsteps.ChangeValuesDuration).SetEase(Ease.InOutCubic));
		}
	}
}