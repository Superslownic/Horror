using FMOD.Studio;
using Zenject;

namespace Scripts.Audio
{
	public class Sound
	{
		public EventInstance EventInstance { get; private set; }

		public PlaybackState PlaybackState
		{
			get
			{
				EventInstance.getPlaybackState(out PLAYBACK_STATE playbackState);

				return playbackState switch
				{
					PLAYBACK_STATE.PLAYING => PlaybackState.Playing,
					PLAYBACK_STATE.SUSTAINING => PlaybackState.Sustaining,
					PLAYBACK_STATE.STOPPED => PlaybackState.Stopped,
					PLAYBACK_STATE.STARTING => PlaybackState.Starting,
					PLAYBACK_STATE.STOPPING => PlaybackState.Stopping,
				};
			}
		}

		public void Play()
		{
			EventInstance.start();
		}

		public void StopImmediate()
		{
			EventInstance.stop(STOP_MODE.IMMEDIATE);
		}
		
		public void StopFaded()
		{
			EventInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}

		public void SetParameter(string name, float value)
		{
			EventInstance.setParameterByName(name, value);
		}

		public void Dispose()
		{
			EventInstance.release();
		}
		
		public class Pool : MemoryPool<EventInstance, Sound>
		{
			protected override void Reinitialize(EventInstance eventInstance, Sound sound)
			{
				sound.EventInstance = eventInstance;
			}
		}
	}
}