using System;
using UnityEngine;

namespace Scripts.Configs.Audio
{
	[Serializable]
	public class AudioConfig
	{
		[field: SerializeField] public AudioEventsConfig Events { get; private set; }
		[field: SerializeField] public AudioParametersConfig Parameters { get; private set; }
	}
}