using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraBobbingValues
	{
		[field: SerializeField] public Vector2 PositionFrequency { get; private set; }
		[field: SerializeField] public Vector2 PositionAmplitude { get; private set; }
		[field: SerializeField] public Vector2 RotationFrequency { get; private set; }
		[field: SerializeField] public Vector2 RotationAmplitude { get; private set; }
	}
}