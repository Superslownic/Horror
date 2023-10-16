using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerMovementConfig
	{
		[field: SerializeField] public LayerMask FloorLayer { get; private set; }
		[field: SerializeField] public float Gravity { get; private set; }
		[field: SerializeField] public float GroundCheckThreshold { get; private set; }
		[field: SerializeField] public float Acceleration { get; private set; }
		[field: SerializeField] public float Deceleration { get; private set; }
		[field: SerializeField] public PlayerMovementValues Walking { get; private set; }
		[field: SerializeField] public PlayerMovementValues Running { get; private set; }
		[field: SerializeField] public PlayerMovementValues Crouching { get; private set; }
	}
}