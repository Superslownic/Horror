using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraBobbingValues
	{
		[field: SerializeField] public bool DependsOnVelocity { get; set; }
		[field: SerializeField] public Vector2 PositionFrequency { get; set; }
		[field: SerializeField] public Vector2 PositionAmplitude { get; set; }
		[field: SerializeField] public Vector2 RotationFrequency { get; set; }
		[field: SerializeField] public Vector2 RotationAmplitude { get; set; }
	}
}