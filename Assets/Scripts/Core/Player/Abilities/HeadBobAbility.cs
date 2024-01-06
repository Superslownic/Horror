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

		private MovementAbility _movementAbility;
		private ShakeHeadAbility _shakeHeadAbility;
		private ShakerProcessor _shakerProcessor;
		private HeadBobShakeVariant _variant;
		private Sound _footstepSound;
		private int _stepNumber;
		private HeadBobShakeVaraintConfig _currentConfig;
		private HeadBobShakeVaraintConfig _savedConfig;

		protected override void OnInitialize()
		{
			_movementAbility = Unit.GetAbility<MovementAbility>();
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_variant = new HeadBobShakeVariant();
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
			_variant.Magnitude = _movementAbility.IsGrounded && _movementAbility.IsMoving
				? _movementAbility.NormalizedVelocity.magnitude
				: 0;

			int stepNumber = Mathf.CeilToInt(_variant.PositionTime);

			if (_variant.Magnitude > 0 && _stepNumber != stepNumber)
			{
				_stepNumber = stepNumber;
				_footstepSound.Play();
			}
		}

		public void ReplaceConfig(HeadBobShakeVaraintConfig config)
		{
			_currentConfig = config;
			SetConfig(config);
		}

		public void OverrideConfig(HeadBobShakeVaraintConfig config)
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

		private void SetConfig(HeadBobShakeVaraintConfig config)
		{
			_variant.PositionAmplitude.Tween(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.PositionFrequency.Tween(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationAmplitude.Tween(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_variant.RotationFrequency.Tween(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}