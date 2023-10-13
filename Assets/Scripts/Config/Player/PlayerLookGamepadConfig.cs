using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerLookGamepadConfig
	{
		[field: SerializeField] public float Sensitivity { get; private set; }
		[field: SerializeField] public float Acceleration { get; private set; }
		[field: SerializeField] public float Deceleration { get; private set; }
	}
}