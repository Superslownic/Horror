using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerConfig
	{
		[field: SerializeField] public PlayerMovementConfig Movement { get; private set; }
		[field: SerializeField] public PlayerLookConfig Look { get; private set; }
		[field: SerializeField] public float BreathDelay { get; private set; }
		[field: SerializeField] public float BreathIncreaseDuration { get; private set; }
		[field: SerializeField] public float BreathDecreaseDuration { get; private set; }
	}
}