using System;
using UnityEngine;

namespace Scripts.Configs.Audio
{
	[Serializable]
	public class AudioParametersConfig
	{
		[field: SerializeField] public string BreathVolume { get; private set; }
		[field: SerializeField] public string FootstepType { get; private set; }
	}
}