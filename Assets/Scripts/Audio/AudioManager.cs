using FMOD.Studio;
using FMODUnity;

namespace Scripts.Audio
{
	public class AudioManager
	{
		public EventInstance Play(EventReference eventReference)
		{
			return RuntimeManager.CreateInstance(eventReference);
		}
	}
}