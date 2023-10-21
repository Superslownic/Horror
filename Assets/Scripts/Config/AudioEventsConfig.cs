using System;
using FMODUnity;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class AudioEventsConfig
	{
		[field: SerializeField] public EventReference Breath { get; private set; }
		[field: SerializeField] public EventReference Footstep { get; private set; }
	}
}