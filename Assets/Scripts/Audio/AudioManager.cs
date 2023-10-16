using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Scripts.Audio
{
	public class AudioManager
	{
		public EventInstance CreateInstance(EventReference eventReference)
		{
			return RuntimeManager.CreateInstance(eventReference);
		}
	}
}