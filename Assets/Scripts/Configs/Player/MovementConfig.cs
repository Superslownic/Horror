using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class MovementConfig
	{
		[field: SerializeField] public LayerMask FloorLayer { get; private set; }
		[field: SerializeField, Range(0, 1)] public float GroundedDrag { get; private set; }
		[field: SerializeField, Range(0, 1)] public float FallDrag { get; private set; }
		[field: SerializeField] public float Gravity { get; private set; }
		[field: SerializeField] public float GroundCheckThreshold { get; private set; }
		[field: SerializeField] public AnimationCurve AccelerationCurve { get; private set; }
		[field: SerializeField] public float Acceleration { get; private set; }
		[field: SerializeField] public float Deceleration { get; private set; }
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public PlayerMovementValues Walking { get; private set; }
		[field: SerializeField] public PlayerMovementValues Running { get; private set; }
		[field: SerializeField] public PlayerMovementValues Crouching { get; private set; }
	}
}