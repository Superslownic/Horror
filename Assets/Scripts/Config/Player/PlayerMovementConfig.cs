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
		[field: SerializeField] public PlayerMovementValues WalkingValues { get; private set; }
		[field: SerializeField] public PlayerMovementValues RunningValues { get; private set; }
		[field: SerializeField] public PlayerMovementValues CrouchValues { get; private set; }
	}
}