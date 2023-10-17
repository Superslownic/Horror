using FMODUnity;
using UnityEngine;
using Zenject;

namespace Scripts.Audio
{
	public class AudioManager
	{
		[Inject] private readonly Sound.Pool _soundPool;
		
		public void PlayOneShot(EventReference eventReference)
		{
			RuntimeManager.PlayOneShot(eventReference);
		}
		
		public void PlayOneShot(EventReference eventReference, Vector3 position)
		{
			RuntimeManager.PlayOneShot(eventReference, position);
		}
		
		public void PlayOneShot(EventReference eventReference, GameObject gameObject)
		{
			RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
		}
		
		public Sound Create(EventReference eventReference)
		{
			return _soundPool.Spawn(RuntimeManager.CreateInstance(eventReference));
		}

		public void Dispose(Sound sound)
		{
			sound.Dispose();
			_soundPool.Despawn(sound);
		}
	}
}