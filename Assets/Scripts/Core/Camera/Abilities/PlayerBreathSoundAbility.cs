using FMOD.Studio;
using Scripts.Audio;
using Scripts.Config;
using Scripts.Entities;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Scripts.Core.Camera
{
	public class PlayerBreathSoundAbility : Ability
	{
		[Inject] private readonly GameConfig _gameConfig;
		[Inject] private readonly AudioManager _audioManager;
		
		private EventInstance _eventInstance;
		
		public void StartRunning()
		{
			_eventInstance = _audioManager.Play(_gameConfig.Audio.Events.BreathRun);
			_eventInstance.start();
		}

		public void StopRunning()
		{
			_eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}
}