using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Config.Camera;
using Scripts.Entities;
using UnityEngine;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Scripts.Core.Camera
{
	public class CameraBreathAbility : Ability
	{
		[SerializeField] private CameraBobbingAbility _cameraBobbingAbility;

		[Inject] private readonly GameConfig _gameConfig;
		
		private float _amount;
		
		public void StopRunning()
		{
			CameraBobbingValues values = new CameraBobbingValues
			{
				PositionAmplitude = Vector2.Lerp(_gameConfig.Camera.Bobbing.IdleMinValues.PositionAmplitude, _gameConfig.Camera.Bobbing.IdleMaxValues.PositionAmplitude, _amount),
				PositionFrequency = Vector2.Lerp(_gameConfig.Camera.Bobbing.IdleMinValues.PositionFrequency, _gameConfig.Camera.Bobbing.IdleMaxValues.PositionFrequency, _amount),
				RotationAmplitude = Vector2.Lerp(_gameConfig.Camera.Bobbing.IdleMinValues.RotationAmplitude, _gameConfig.Camera.Bobbing.IdleMaxValues.RotationAmplitude, _amount),
				RotationFrequency = Vector2.Lerp(_gameConfig.Camera.Bobbing.IdleMinValues.RotationFrequency, _gameConfig.Camera.Bobbing.IdleMaxValues.RotationFrequency, _amount),
				DependsOnVelocity = false
			};
			
			_cameraBobbingAbility.SetConfig(values).OnComplete(ChangeToMinValues);
		}

		public void Increase()
		{
			_amount += 1 / _gameConfig.Player.BreathIncreaseDuration * Time.deltaTime;
			_amount = Mathf.Clamp01(_amount);
		}
		
		public void Decrease()
		{
			_amount -= 1 / _gameConfig.Player.BreathDecreaseDuration * Time.deltaTime;
			_amount = Mathf.Clamp01(_amount);
		}

		private void ChangeToMinValues()
		{
			_cameraBobbingAbility.SetConfig(_gameConfig.Camera.Bobbing.IdleMinValues,
				_gameConfig.Camera.Bobbing.BreatheChangeDuration);
		}
	}
}