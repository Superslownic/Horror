using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class AudioParametersConfig
	{
		[field: SerializeField] public string BreathVolume { get; private set; }
		[field: SerializeField] public string FootstepType { get; private set; }
	}
}