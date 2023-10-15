using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class AudioConfig
	{
		[field: SerializeField] public AudioEventsConfig Events { get; private set; }
		[field: SerializeField] public AudioParametersConfig Parameters { get; private set; }
	}
}