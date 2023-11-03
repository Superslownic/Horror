using DG.Tweening;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class HeadBobAbility : Ability, IDeactivatableAbility
	{
		[Inject] private readonly GameConfig _gameConfig;
		[Inject] private readonly AudioManager _audioManager;

		private ShakeHeadAbility _shakeHeadAbility;
		private ShakerProcessor _shakerProcessor;
		private BobbingShakeVariant _variant;
		private Sound _footstepSound;
		private int _stepNumber;
		private BobbingShakeProcessorConfig _currentConfig;
		private BobbingShakeProcessorConfig _savedConfig;

		protected override void OnInitialize()
		{
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_variant = new BobbingShakeVariant();
			_shakerProcessor = new ShakerProcessor
			{
				Config = _gameConfig.Player.Footsteps.ShakerConfig,
				Variant = _variant
			};
			_shakeHeadAbility.Shaker.StartShaker(_shakerProcessor);
			_footstepSound = _audioManager.Create(_gameConfig.Audio.Events.Footstep);
		}

		protected override void OnDeactivate()
		{
			_variant.Magnitude = 0;
		}

		protected override void OnUpdate()
		{
			float magnitude = Unit.GetAbility<MovementAbility>().NormalizedActualVelocity.magnitude;

			_variant.Magnitude = magnitude;

			int stepNumber = Mathf.CeilToInt(_variant.PositionTime);
			
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
			ReplaceConfig(_gameConfig.Player.Footsteps.WalkShakeConfig);
		}
		
		public void ToRun()
		{
			ReplaceConfig(_gameConfig.Player.Footsteps.RunShakeConfig);
		}

		public void ToCrouch()
		{
			ReplaceConfig(_gameConfig.Player.Footsteps.CrouchShakeConfig);
		}

		public void ReplaceConfig(BobbingShakeProcessorConfig config)
		{
			_currentConfig = config;
			SetConfig(config);
		}

		public void OverrideConfig(BobbingShakeProcessorConfig config)
		{
			_savedConfig = _currentConfig;
			ReplaceConfig(config);
		}

		public void CancelOverride()
		{
			ReplaceConfig(_savedConfig);
		}

		public void OverrideMagnitude(float value)
		{
			_variant.Magnitude = value;
		}

		private void SetConfig(BobbingShakeProcessorConfig config)
		{
			_variant.PositionAmplitude.ReplaceValue(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.ReplaceValue(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.ReplaceValue(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.ReplaceValue(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}