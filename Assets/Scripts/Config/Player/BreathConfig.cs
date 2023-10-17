using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class BreathConfig
	{
		[field: SerializeField] public float Delay { get; private set; }
		[field: SerializeField] public float IncreaseDuration { get; private set; }
		[field: SerializeField] public float DecreaseDuration { get; private set; }
	}
}