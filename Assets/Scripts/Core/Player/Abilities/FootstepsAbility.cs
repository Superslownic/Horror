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
		private Sound _footstepSound;
		private int _stepNumber;

		protected override void OnInitialize()
		{
			_processor = new BobbingShakeProcessor();
			_shaker = new Shaker
			{
				Config = _gameConfig.Player.Footsteps.ShakerConfig,
				Processor = _processor
			};
			_shakeAbility.StartShaker(_shaker);
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
			ReplaceConfig(BobbingShakeProcessorConfig.Default);
		}

		public void ToWalk()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 0);
			ReplaceConfig(_gameConfig.Player.Footsteps.WalkShakeConfig);
		}
		
		public void ToRun()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 1);
			ReplaceConfig(_gameConfig.Player.Footsteps.RunShakeConfig);
		}

		public void ToCrouch()
		{
			_footstepSound.SetParameter(_gameConfig.Audio.Parameters.FootstepType, 2);
			ReplaceConfig(_gameConfig.Player.Footsteps.CrouchShakeConfig);
		}
		
		private void ReplaceConfig(BobbingShakeProcessorConfig config)
		{
			_processor.PositionAmplitude.ReplaceValue(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.PositionFrequency.ReplaceValue(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.RotationAmplitude.ReplaceValue(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_processor.RotationFrequency.ReplaceValue(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}