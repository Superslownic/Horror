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
		private BobbingShakeProcessorValues _bobbingValues;
		private Sound _footstepSound;
		private Tween _tween;
		private int _stepNumber;

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
			float magnitude = _movementAbility.NormalizedActualVelocity.magnitude;
			
			_processor.Magnitude = magnitude;

			int stepNumber = Mathf.CeilToInt(_processor.PositionTime);
			
			if (magnitude > 0 && _stepNumber != stepNumber)
			{
				_stepNumber = stepNumber;
				_footstepSound.Play();
			}
		}

		public void ToIdle()
		{
			ChangeValues(BobbingShakeProcessorValues.Default);
		}

		public void ToWalk()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 0);
			ChangeValues(_gameConfig.Player.Footsteps.WalkingValues);
		}
		
		public void ToRun()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 1);
			ChangeValues(_gameConfig.Player.Footsteps.RunningValues);
		}

		public void ToCrouch()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 2);
			ChangeValues(_gameConfig.Player.Footsteps.CrouchingValues);
		}

		private void ChangeValues(BobbingShakeProcessorValues values)
		{
			_bobbingValues = values;
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