using DG.Tweening;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class BreathAbility : Ability
	{
		[SerializeField] private FootstepsAbility bobbingAbility;
		
		[Inject] private readonly GameConfig _gameConfig;
		[Inject] private readonly AudioManager _audioManager;
		
		private Sound _sound;
		private float _volume;
		private float _delay;
		private bool _isPlaying;

		protected override void OnInitialize()
		{
			_sound = _audioManager.Create(_gameConfig.Audio.Events.Breath);
		}

		public void StartRunning()
		{
			_isPlaying = true;
		}

		public void StopRunning()
		{
			/*_isPlaying = false;
			
			BobbingValues values = new BobbingValues
			{
				PositionAmplitude = Vector2.Lerp(_gameConfig.Player.Bobbing.IdleMinValues.PositionAmplitude, _gameConfig.Player.Bobbing.IdleMaxValues.PositionAmplitude, _volume),
				PositionFrequency = Vector2.Lerp(_gameConfig.Player.Bobbing.IdleMinValues.PositionFrequency, _gameConfig.Player.Bobbing.IdleMaxValues.PositionFrequency, _volume),
				RotationAmplitude = Vector2.Lerp(_gameConfig.Player.Bobbing.IdleMinValues.RotationAmplitude, _gameConfig.Player.Bobbing.IdleMaxValues.RotationAmplitude, _volume),
				RotationFrequency = Vector2.Lerp(_gameConfig.Player.Bobbing.IdleMinValues.RotationFrequency, _gameConfig.Player.Bobbing.IdleMaxValues.RotationFrequency, _volume),
				DependsOnVelocity = false
			};
			
			bobbingAbility.SetValues(values).OnComplete(() =>
			{
				bobbingAbility.SetValues(_gameConfig.Player.Bobbing.IdleMinValues,
					_gameConfig.Player.Bobbing.BreatheChangeDuration);
			});*/
		}

		protected override void OnUpdate()
		{
			if (_isPlaying)
			{
				if (_delay < _gameConfig.Player.Breath.Delay)
				{
					_delay += Time.deltaTime;
				}
				else
				{
					if (_sound.PlaybackState != PlaybackState.Playing)
					{
						_sound.Play();
					}
					
					_volume += 1 / _gameConfig.Player.Breath.IncreaseDuration * Time.deltaTime;
				}
			}
			else
			{
				if (_volume <= 0)
				{
					_delay -= Time.deltaTime;
				}
				
				_volume -= 1 / _gameConfig.Player.Breath.DecreaseDuration * Time.deltaTime;
			}
			
			_delay = Mathf.Clamp(_delay, 0, _gameConfig.Player.Breath.Delay);
			_volume = Mathf.Clamp(_volume, 0, _gameConfig.Player.Breath.IncreaseDuration);

			_sound?.SetParameter(_gameConfig.Audio.Parameters.BreathVolume, _volume / _gameConfig.Player.Breath.IncreaseDuration);
		}
	}
}