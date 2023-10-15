using System;
using FMODUnity;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class AudioEventsConfig
	{
		[field: SerializeField] public EventReference BreathRun { get; private set; }
		[field: SerializeField] public EventReference BreathStop { get; private set; }
	}
}