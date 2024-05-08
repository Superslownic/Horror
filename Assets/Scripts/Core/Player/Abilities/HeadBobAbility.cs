using DG.Tweening;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class HeadBobAbility : Ability
	{
		[Inject] private readonly GameConfig _gameConfig;
		[Inject] private readonly AudioManager _audioManager;

		private GroundMoveAbility _groundMoveAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private CheckGroundAbility _checkGroundAbility;
		private ShakeHeadAbility _shakeHeadAbility;
		private ShakerProcessor _shakerProcessor;
		private HeadBobShakeVariant _shakerVariant;
		private Sound _footstepSound;
		private int _stepNumber;
		private HeadBobShakeVaraintConfig _currentConfig;
		private HeadBobShakeVaraintConfig _savedConfig;

		protected override void OnInitialize()
		{
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
			_shakeHeadAbility = Unit.GetAbility<ShakeHeadAbility>();
			_shakerVariant = new HeadBobShakeVariant();
			_shakerProcessor = new ShakerProcessor
			{
				Config = _gameConfig.Player.Footsteps.ShakerConfig,
				Variant = _shakerVariant
			};
			_footstepSound = _audioManager.Create(_gameConfig.Audio.Events.Footstep);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			_shakeHeadAbility.Shaker.StartShaker(_shakerProcessor);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_shakeHeadAbility.Shaker.StopShaker(_shakerProcessor);
		}

		protected override void OnUpdate()
		{
			_shakerVariant.Magnitude = _checkGroundAbility.IsGrounded && _groundMoveAbility.IsMoving
				? _changeVelocityAbility.ActualVelocity.normalized.magnitude
				: Mathf.Lerp(_shakerVariant.Magnitude, 0, Time.deltaTime);

			int stepNumber = Mathf.CeilToInt(_shakerVariant.PositionTime);

			if (_shakerVariant.Magnitude > 0 && _stepNumber != stepNumber)
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
			_shakerVariant.Magnitude = value;
		}

		private void SetConfig(HeadBobShakeVaraintConfig config)
		{
			_shakerVariant.PositionAmplitude.Tween(config.PositionAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_shakerVariant.PositionFrequency.Tween(config.PositionFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_shakerVariant.RotationAmplitude.Tween(config.RotationAmplitude, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
			_shakerVariant.RotationFrequency.Tween(config.RotationFrequency, _gameConfig.Player.Footsteps.ChangeValuesDuration, Ease.InOutCubic);
		}
	}
}