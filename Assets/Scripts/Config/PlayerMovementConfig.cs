using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class PlayerMovementConfig
	{
		[field: SerializeField] public LayerMask FloorLayer { get; private set; }
		[field: SerializeField] public float GroundCheckThreshold { get; private set; }
		[field: SerializeField] public float MaxSpeed { get; private set; }
		[field: SerializeField] public float AccelerationDelta { get; private set; }
	}
}